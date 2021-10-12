using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Aqua.Core.Mvvm;
using Aqua.Core.Services;
using Aqua.Core.Utils;
using DryIoc;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Aqua.XamarinForms.Core.Services.Navigation
{
    public sealed class NavigationService : INavigationService
    {
	    private readonly INavigationMapper _navigationMapper;
	    private readonly INavigationPageFactory _navigationPageFactory;
	    private readonly IResolver _resolver;
        
		private readonly CanNavigateNow _canNavigateNow = new CanNavigateNow(true);
		private readonly Dictionary<ViewModelBase, IViewModelWrapper> _viewModelWrappers = new Dictionary<ViewModelBase, IViewModelWrapper>();

		private bool _firstSetMainView = true;
		
		private NavigationPage NavigationView
			=> Application.Current.MainPage is FlyoutPage flyoutPage
				? (NavigationPage)flyoutPage.Detail
				: (NavigationPage)Application.Current.MainPage;

		public NavigationService(
			IEnumerable<INavigationModule> navigationModules,
			INavigationMapper navigationMapper,
			INavigationPageFactory navigationPageFactory,
			IResolver resolver)
		{
			_navigationMapper = navigationMapper;
			_navigationPageFactory = navigationPageFactory;
			_resolver = resolver;
			
			navigationModules.ForEach(it => it.Map(navigationMapper));
		}
		
		#region Stacks

		public IReadOnlyList<ViewModelBase> NavigationStack => GetStack(StackType.Navigation);
		
		public IReadOnlyList<ViewModelBase> ModalStack => GetStack(StackType.Modal);

		public IReadOnlyList<ViewModelBase> PopupStack => GetStack(StackType.Popup);

		public IReadOnlyList<ViewModelBase> GetStack(StackType stackType)
		{
			return new StackAlgorithmsFactory(NavigationView)
				.Create(stackType)
				.GetStack();
		}

		public bool TryGetFromStack<TViewModel>(TViewModel viewModel, StackType stackType, out IReadOnlyList<ViewModelBase> stack, out int index)
			where TViewModel : ViewModelBase
		{
			stack = GetStack(stackType);
			index = ((List<ViewModelBase>)stack).IndexOf(viewModel);

			return index >= 0;
		}

		public bool TryGetStackFor<TViewModel>(TViewModel viewModel, [NotNullWhen(true)] out StackType? stackType, out IReadOnlyList<ViewModelBase> stack, out int index)
			where TViewModel : ViewModelBase
		{
			foreach (var type in StackAlgorithmsFactory.StackTypes)
			{
				if (TryGetFromStack(viewModel, type, out stack, out index))
				{
					stackType = type;
					return true;
				}
			}

			stackType = null;
			stack = new List<ViewModelBase>();
			index = -1;
			
			return false;
		}

		#endregion
		
		public ViewModelBase GetParentFor<TViewModel>(TViewModel viewModel)
			where TViewModel : ViewModelBase
		{
			return _viewModelWrappers.GetValueOrDefault(viewModel)?.Parent;
		}

		public ViewModelBase GetMainParentFor<TViewModel>(TViewModel viewModel)
			where TViewModel : ViewModelBase
		{
			var mainParent = viewModel as ViewModelBase;
			
			while (GetParentFor(mainParent) != null)
			{
				mainParent = GetParentFor(mainParent);
			}
			
			return mainParent;
		}

		public IReadOnlyList<ViewModelBase> GetChildrenFor<TViewModel>(TViewModel viewModel)
			where TViewModel : ViewModelBase
		{
			return _viewModelWrappers.GetValueOrDefault(viewModel)?.Children ?? new List<ViewModelBase>();
		}

		public ViewModelBase GetPreviousFor<TViewModel>(TViewModel viewModel) 
			where TViewModel : ViewModelBase
		{
			var mainParent = GetMainParentFor(viewModel);

			if (TryGetStackFor(mainParent, out _, out var stack, out var index))
			{
				return index == 0
					? null
					: stack[index - 1];
			}

			return null;
		}

		#region SetMainView

		public void SetMainView<TViewModel>()
			where TViewModel : ViewModelBase
		{
			SetMainView<TViewModel>(null);
		}
		
		public void SetMainView<TViewModel>(Action<TViewModel> viewModelInitialization)
			where TViewModel : ViewModelBase
		{
			SetMainViewPrivate<TViewModel, object>(null, viewModelInitialization);
		}

		public void SetMainView<TViewModel, TParam>(TParam param)
			where TViewModel : ViewModelBase, IWithInit<TParam>
		{
			SetMainView<TViewModel, TParam>(param, null);
		}

		public void SetMainView<TViewModel, TParam>(TParam param, Action<TViewModel> viewModelInitialization)
			where TViewModel : ViewModelBase, IWithInit<TParam>
		{
			SetMainViewPrivate(param, viewModelInitialization);
		}

		public void SetMainView(Type viewModelType)
		{
			SetMainView(viewModelType, null, null);
		}

		public void SetMainView(Type viewModelType, Action<ViewModelBase> viewModelInitialization)
		{
			SetMainView(viewModelType, null, viewModelInitialization);
		}

		public void SetMainView(Type viewModelType, object param)
		{
			SetMainView(viewModelType, param, null);
		}

		public void SetMainView(Type viewModelType, object param, Action<ViewModelBase> viewModelInitialization)
		{
			if (!typeof(ViewModelBase).IsAssignableFrom(viewModelType))
				throw new ArgumentException($"{viewModelType.Name} must be inherited from the {nameof(ViewModelBase)}");

			ReflectionHelper.CallByReflectionFrom(
				this,
				nameof(SetMainViewPrivate), 
				new [] { viewModelType, param?.GetType() ?? typeof(object) },
				param, 
				viewModelInitialization);
		}

		#endregion
		
		#region NavigateToAsync

		public async Task NavigateToAsync<TViewModel>(
			StackType stackType = StackType.Navigation,
			bool withAnimation = true)
			where TViewModel : ViewModelBase
		{
			await NavigateToAsync<TViewModel>(
				null, 
				stackType, 
				withAnimation);
		}
		
		public async Task NavigateToAsync<TViewModel>(
			Action<TViewModel> viewModelInitialization,
			StackType stackType = StackType.Navigation,
			bool withAnimation = true)
			where TViewModel : ViewModelBase
		{
			await NavigateToPrivateAsync<TViewModel, object, object>(
				null, 
				viewModelInitialization,
				null, 
				stackType,
				withAnimation);
		}

		public async Task NavigateToAsync<TViewModel, TParam>(
			TParam param,
			StackType stackType = StackType.Navigation,
			bool withAnimation = true)
			where TViewModel : ViewModelBase, IWithInit<TParam>
		{
			await NavigateToAsync<TViewModel, TParam>(
				param,
				null,
				stackType,
				withAnimation);
		}

		public async Task NavigateToAsync<TViewModel, TParam>(
			TParam param,
			Action<TViewModel> viewModelInitialization,
			StackType stackType = StackType.Navigation,
			bool withAnimation = true)
			where TViewModel : ViewModelBase, IWithInit<TParam>
		{
			await NavigateToPrivateAsync<TViewModel, TParam, object>(
				param,
				viewModelInitialization,
				null,
				stackType,
				withAnimation);
		}

		public async Task NavigateToAsync<TViewModel, TResult>(
			CallbackParam<TResult> callbackParam,
			StackType stackType = StackType.Navigation,
			bool withAnimation = true)
			where TViewModel : ViewModelBase, IWithResult<TResult>
		{
			await NavigateToAsync<TViewModel, TResult>(
				null,
				callbackParam,
				stackType,
				withAnimation);
		}

		public async Task NavigateToAsync<TViewModel, TResult>(
			Action<TViewModel> viewModelInitialization,
			CallbackParam<TResult> callbackParam,
			StackType stackType = StackType.Navigation,
			bool withAnimation = true)
			where TViewModel : ViewModelBase, IWithResult<TResult>
		{
			await NavigateToPrivateAsync<TViewModel, object, TResult>(
				null,
				viewModelInitialization,
				callbackParam,
				stackType,
				withAnimation);
		}

		public async Task NavigateToAsync<TViewModel, TParam, TResult>(
			TParam param,
			CallbackParam<TResult> callbackParam,
			StackType stackType = StackType.Navigation,
			bool withAnimation = true)
			where TViewModel : ViewModelBase, IWithInit<TParam>, IWithResult<TResult>
		{
			await NavigateToAsync<TViewModel, TParam, TResult>(
				param,
				null,
				callbackParam,
				stackType,
				withAnimation);
		}

		public async Task NavigateToAsync<TViewModel, TParam, TResult>(
			TParam param,
			Action<TViewModel> viewModelInitialization,
			CallbackParam<TResult> callbackParam,
			StackType stackType = StackType.Navigation,
			bool withAnimation = true)
			where TViewModel : ViewModelBase, IWithInit<TParam>, IWithResult<TResult>
		{
			await NavigateToPrivateAsync(
				param,
				viewModelInitialization,
				callbackParam,
				stackType,
				withAnimation);
		}

		public async Task NavigateToAsync(
			Type viewModelType,
			StackType stackType = StackType.Navigation,
			bool withAnimation = true)
		{
			await NavigateToAsync<object>(
				viewModelType,
				null,
				null,
				null,
				stackType,
				withAnimation);
		}
		
		public async Task NavigateToAsync(
			Type viewModelType,
			Action<ViewModelBase> viewModelInitialization,
			StackType stackType = StackType.Navigation,
			bool withAnimation = true)
		{
			await NavigateToAsync<object>(
				viewModelType,
				null,
				viewModelInitialization,
				null,
				stackType,
				withAnimation);
		}

		public async Task NavigateToAsync(
			Type viewModelType,
			object param,
			StackType stackType = StackType.Navigation,
			bool withAnimation = true)
		{
			await NavigateToAsync<object>(
				viewModelType,
				param,
				null,
				null,
				stackType,
				withAnimation);
		}

		public async Task NavigateToAsync(
			Type viewModelType,
			object param,
			Action<ViewModelBase> viewModelInitialization,
			StackType stackType = StackType.Navigation,
			bool withAnimation = true)
		{
			await NavigateToAsync<object>(
				viewModelType,
				param,
				viewModelInitialization,
				null,
				stackType,
				withAnimation);
		}

		public async Task NavigateToAsync<TResult>(
			Type viewModelType,
			CallbackParam<TResult> callbackParam,
			StackType stackType = StackType.Navigation,
			bool withAnimation = true)
		{
			await NavigateToAsync(
				viewModelType,
				null,
				null,
				callbackParam,
				stackType,
				withAnimation);
		}

		public async Task NavigateToAsync<TResult>(
			Type viewModelType,
			Action<ViewModelBase> viewModelInitialization,
			CallbackParam<TResult> callbackParam,
			StackType stackType = StackType.Navigation,
			bool withAnimation = true)
		{
			await NavigateToAsync(
				viewModelType,
				null,
				viewModelInitialization,
				callbackParam,
				stackType,
				withAnimation);
		}

		public async Task NavigateToAsync<TResult>(
			Type viewModelType,
			object param,
			CallbackParam<TResult> callbackParam,
			StackType stackType = StackType.Navigation,
			bool withAnimation = true)
		{
			await NavigateToAsync(
				viewModelType,
				param,
				null,
				callbackParam,
				stackType,
				withAnimation);
		}

		public async Task NavigateToAsync<TResult>(
			Type viewModelType,
			object param,
			Action<ViewModelBase> viewModelInitialization,
			CallbackParam<TResult> callbackParam,
			StackType stackType = StackType.Navigation,
			bool withAnimation = true)
		{
			if (!typeof(ViewModelBase).IsAssignableFrom(viewModelType))
				throw new ArgumentException($"{viewModelType.Name} must be inherited from the {nameof(ViewModelBase)}");

			await ReflectionHelper.CallByReflectionFromAsync(
				this,
				nameof(NavigateToPrivateAsync), 
				new [] { viewModelType, param?.GetType() ?? typeof(object), typeof(TResult) },
				param, 
				viewModelInitialization,
				callbackParam, 
				stackType, 
				withAnimation);
		}

		#endregion

		#region InsertBefore

		public void InsertBefore<TViewModel>(
			ViewModelBase beforeViewModel)
			where TViewModel : ViewModelBase
		{
			InsertBefore<TViewModel>(
				beforeViewModel, 
				null);
		}

		public void InsertBefore<TViewModel>(
			ViewModelBase beforeViewModel,
			Action<TViewModel> viewModelInitialization)
			where TViewModel : ViewModelBase
		{
			InsertBeforePrivate<TViewModel, object, object>(
				beforeViewModel, 
				null, 
				viewModelInitialization, 
				null);
		}

		public void InsertBefore<TViewModel, TParam>(
			ViewModelBase beforeViewModel,
			TParam param)
			where TViewModel : ViewModelBase, IWithInit<TParam>
		{
			InsertBefore<TViewModel, TParam>(
				beforeViewModel, 
				param, 
				null);
		}

		public void InsertBefore<TViewModel, TParam>(
			ViewModelBase beforeViewModel,
			TParam param,
			Action<TViewModel> viewModelInitialization)
			where TViewModel : ViewModelBase, IWithInit<TParam>
		{
			InsertBeforePrivate<TViewModel, TParam, object>(
				beforeViewModel, 
				param, 
				viewModelInitialization, 
				null);
		}

		public void InsertBefore<TViewModel, TResult>(
			ViewModelBase beforeViewModel,
			CallbackParam<TResult> callbackParam)
			where TViewModel : ViewModelBase, IWithResult<TResult>
		{
			InsertBefore<TViewModel, TResult>(
				beforeViewModel, 
				null, 
				callbackParam);
		}

		public void InsertBefore<TViewModel, TResult>(
			ViewModelBase beforeViewModel,
			Action<TViewModel> viewModelInitialization,
			CallbackParam<TResult> callbackParam)
			where TViewModel : ViewModelBase, IWithResult<TResult>
		{
			InsertBeforePrivate<TViewModel, object, TResult>(
				beforeViewModel, 
				null, 
				viewModelInitialization, 
				callbackParam);
		}

		public void InsertBefore<TViewModel, TParam, TResult>(
			ViewModelBase beforeViewModel,
			TParam param,
			CallbackParam<TResult> callbackParam)
			where TViewModel : ViewModelBase, IWithInit<TParam>, IWithResult<TResult>
		{
			InsertBefore<TViewModel, TParam, TResult>(
				beforeViewModel, 
				param, 
				null, 
				callbackParam);
		}

		public void InsertBefore<TViewModel, TParam, TResult>(
			ViewModelBase beforeViewModel,
			TParam param,
			Action<TViewModel> viewModelInitialization,
			CallbackParam<TResult> callbackParam)
			where TViewModel : ViewModelBase, IWithInit<TParam>, IWithResult<TResult>
		{
			InsertBeforePrivate(
				beforeViewModel, 
				param, 
				viewModelInitialization, 
				callbackParam);
		}

		public void InsertBefore(
			ViewModelBase beforeViewModel,
			Type viewModelType)
		{
			InsertBefore<object>(
				beforeViewModel, 
				viewModelType, 
				null, 
				null, 
				null);
		}

		public void InsertBefore(
			ViewModelBase beforeViewModel,
			Type viewModelType,
			Action<ViewModelBase> viewModelInitialization)
		{
			InsertBefore<object>(
				beforeViewModel, 
				viewModelType, 
				null, 
				viewModelInitialization, 
				null);
		}

		public void InsertBefore(
			ViewModelBase beforeViewModel,
			Type viewModelType,
			object param)
		{
			InsertBefore<object>(
				beforeViewModel, 
				viewModelType, 
				param, 
				null, 
				null);
		}

		public void InsertBefore(
			ViewModelBase beforeViewModel,
			Type viewModelType,
			object param,
			Action<ViewModelBase> viewModelInitialization)
		{
			InsertBefore<object>(
				beforeViewModel, 
				viewModelType, 
				param, 
				viewModelInitialization, 
				null);
		}

		public void InsertBefore<TResult>(
			ViewModelBase beforeViewModel,
			Type viewModelType,
			CallbackParam<TResult> callbackParam)
		{
			InsertBefore(
				beforeViewModel, 
				viewModelType, 
				null, 
				null, 
				callbackParam);
		}

		public void InsertBefore<TResult>(
			ViewModelBase beforeViewModel,
			Type viewModelType,
			Action<ViewModelBase> viewModelInitialization,
			CallbackParam<TResult> callbackParam)
		{
			InsertBefore(
				beforeViewModel, 
				viewModelType, 
				null, 
				viewModelInitialization, 
				callbackParam);
		}

		public void InsertBefore<TResult>(
			ViewModelBase beforeViewModel,
			Type viewModelType,
			object param,
			CallbackParam<TResult> callbackParam)
		{
			InsertBefore(
				beforeViewModel, 
				viewModelType, 
				param, 
				null, 
				callbackParam);
		}

        public void InsertBefore<TResult>(
	        ViewModelBase beforeViewModel,
	        Type viewModelType,
	        object param,
	        Action<ViewModelBase> viewModelInitialization,
	        CallbackParam<TResult> callbackParam)
        {
	        if (!typeof(ViewModelBase).IsAssignableFrom(viewModelType))
		        throw new ArgumentException($"{viewModelType.Name} must be inherited from the {nameof(ViewModelBase)}");

	        ReflectionHelper.CallByReflectionFrom(
		        this,
		        nameof(InsertBeforePrivate),
		        new[] { viewModelType, param?.GetType() ?? typeof(object), typeof(TResult) },
		        beforeViewModel,
		        param,
		        viewModelInitialization,
		        callbackParam);
        }
        
        #endregion

        #region CloseAsync

        public async Task CloseAsync<TViewModel>(
	        TViewModel viewModel,
	        bool withAnimation = true)
	        where TViewModel : ViewModelBase
        {
	        if (!CanClose(viewModel, out var stackType))
		        return;

	        await CloseAsync(stackType.Value, withAnimation);
        }

        public async Task CloseAsync<TViewModel, TResult>(
	        TViewModel viewModel,
	        TResult result,
	        bool withAnimation = true)
	        where TViewModel : ViewModelBase, IWithResult<TResult>
        {
	        if (!CanClose(viewModel, out var stackType))
		        return;

	        _viewModelWrappers.TryGetValue(viewModel, out var viewModelWrapper);
	        
	        await ClosePrivateAsync(
		        stackType.Value,
		        (viewModelWrapper as IViewModelWrapperWithResult<TResult>)?.ViewClosing,
		        (viewModelWrapper as IViewModelWrapperWithResult<TResult>)?.ViewClosed,
		        result,
		        withAnimation);
        }
        
        public async Task CloseAsync(
	        StackType stackType, 
	        bool withAnimation = true)
        {
	        await ClosePrivateAsync<object>(
		        stackType,
		        null,
		        null,
		        null,
		        withAnimation);
        }

        #endregion

        #region CloseToRootAsync

        public async Task CloseToRootAsync(
	        bool withAnimation = true)
        {
	        await CloseToRootPrivateAsync(withAnimation);
        }

        public async Task CloseToRootAsync<TViewModel>(
	        bool withAnimation = true)
	        where TViewModel : ViewModelBase
        {
	        await CloseToRootAsync<TViewModel>(
		        null,
		        withAnimation);
        }

        public async Task CloseToRootAsync<TViewModel>(
	        Action<TViewModel> viewModelInitialization,
	        bool withAnimation = true)
	        where TViewModel : ViewModelBase
        {
	        await CloseToRootPrivateAsync<TViewModel, object>(
		        null,
		        viewModelInitialization,
		        withAnimation);
        }
        
        public async Task CloseToRootAsync<TViewModel, TParam>(
	        TParam param,
	        bool withAnimation = true)
	        where TViewModel : ViewModelBase, IWithInit<TParam>
        {
	        await CloseToRootAsync<TViewModel, TParam>(
		        param,
		        null,
		        withAnimation);
        }

        public async Task CloseToRootAsync<TViewModel, TParam>(
	        TParam param,
	        Action<TViewModel> viewModelInitialization,
	        bool withAnimation = true)
	        where TViewModel : ViewModelBase, IWithInit<TParam>
        {
	        await CloseToRootPrivateAsync(
		        param,
		        viewModelInitialization,
		        withAnimation);
        }

        public async Task CloseToRootAsync(
	        Type viewModelType,
	        bool withAnimation = true)
        {
	        await CloseToRootAsync(
		        viewModelType,
		        null,
		        null,
		        withAnimation);
        }
        
        public async Task CloseToRootAsync(
	        Type viewModelType,
	        Action<ViewModelBase> viewModelInitialization,
	        bool withAnimation = true)
        {
	        await CloseToRootAsync(
		        viewModelType,
		        null,
		        viewModelInitialization,
		        withAnimation);
        }
        
        public async Task CloseToRootAsync(
	        Type viewModelType,
	        object param,
	        bool withAnimation = true)
        {
	        await CloseToRootAsync(
		        viewModelType,
		        param,
		        null,
		        withAnimation);
        }

        public async Task CloseToRootAsync(
	        Type viewModelType,
	        object param,
	        Action<ViewModelBase> viewModelInitialization,
	        bool withAnimation = true)
        {
	        if (!typeof(ViewModelBase).IsAssignableFrom(viewModelType))
		        throw new ArgumentException($"{viewModelType.Name} must be inherited from the {nameof(ViewModelBase)}");

	        await ReflectionHelper.CallByReflectionFromAsync(
		        this,
		        nameof(CloseToRootPrivateAsync),
		        new[] { viewModelType, param?.GetType() ?? typeof(object) },
		        param,
		        viewModelInitialization,
		        withAnimation);
        }
        
        #endregion

        #region Remove

        public async Task RemoveByAsync(
	        int index, 
	        StackType stackType = StackType.Navigation,
	        bool withAnimation = true)
        {
	        var view = await new StackAlgorithmsFactory(NavigationView)
		        .Create(stackType)
		        .RemoveAsync(index);
	        
	        Clear(view);
        }

        public async Task RemoveAsync<TViewModel>(
	        TViewModel viewModel,
	        bool withAnimation = true)
	        where TViewModel : ViewModelBase
        {
	        if (viewModel == null)
		        return;

	        var mainParent = GetMainParentFor(viewModel);

	        if (TryGetStackFor(mainParent, out var stackType, out _, out var index))
	        {
		        await RemoveByAsync(index, stackType.Value, withAnimation);
	        }
        }
        
        #endregion

        #region SetDetailView

        public void SetDetailView<TViewModel>(
	        bool withCloseFlyoutView = true)
	        where TViewModel : ViewModelBase
        {
	        SetDetailView<TViewModel>(
		        null,
		        withCloseFlyoutView);
        }
        
        public void SetDetailView<TViewModel>(
	        Action<TViewModel> viewModelInitialization,
	        bool withCloseFlyoutView = true)
	        where TViewModel : ViewModelBase
        {
	        SetDetailViewPrivate<TViewModel, object>(
		        null,
		        viewModelInitialization,
		        withCloseFlyoutView);
        }

        public void SetDetailView<TViewModel, TParam>(
	        TParam param,
	        bool withCloseFlyoutView = true)
	        where TViewModel : ViewModelBase, IWithInit<TParam>
        {
	        SetDetailView<TViewModel, TParam>(
		        param,
		        null,
		        withCloseFlyoutView);
        }

        public void SetDetailView<TViewModel, TParam>(
	        TParam param,
	        Action<TViewModel> viewModelInitialization,
	        bool withCloseFlyoutView = true)
	        where TViewModel : ViewModelBase, IWithInit<TParam>
        {
	        SetDetailViewPrivate(
		        param,
		        viewModelInitialization,
		        withCloseFlyoutView);
        }

        public void SetDetailView(
	        Type viewModelType,
	        bool withCloseFlyoutView = true)
        {
	        SetDetailView(
		        viewModelType,
		        null,
		        null,
		        withCloseFlyoutView);
        }
        
        public void SetDetailView(
	        Type viewModelType,
	        Action<ViewModelBase> viewModelInitialization,
	        bool withCloseFlyoutView = true)
        {
	        SetDetailView(
		        viewModelType,
		        null,
		        viewModelInitialization,
		        withCloseFlyoutView);
        }

        public void SetDetailView(
	        Type viewModelType,
	        object param,
	        bool withCloseFlyoutView = true)
        {
	        SetDetailView(
		        viewModelType,
		        param,
		        null,
		        withCloseFlyoutView);
        }

        public void SetDetailView(
	        Type viewModelType,
	        object param,
	        Action<ViewModelBase> viewModelInitialization,
	        bool withCloseFlyoutView = true)
        {
	        if (!typeof(ViewModelBase).IsAssignableFrom(viewModelType))
		        throw new ArgumentException($"{viewModelType.Name} must be inherited from the {nameof(ViewModelBase)}");

	        ReflectionHelper.CallByReflectionFrom(
		        this,
		        nameof(SetDetailViewPrivate),
		        new[] { viewModelType, param?.GetType() ?? typeof(object) },
		        param,
		        viewModelInitialization,
		        withCloseFlyoutView);
        }
        
        #endregion

        #region CloseFlyoutView

        public void CloseFlyoutView()
		{
			if (!(Application.Current.MainPage is FlyoutPage flyoutPage))
				throw new Exception($"Application.Current.MainPage must be the {nameof(FlyoutPage)} to call the method '{nameof(CloseFlyoutView)}'.");

			flyoutPage.IsPresented = false;
		}

		#endregion

		#region CloseAllPopupViewsAsync

		public async Task CloseAllPopupViewsAsync(bool withAnimation = true)
		{
			if (!_canNavigateNow.Value)
				return;

			using (_canNavigateNow)
			{
				_canNavigateNow.Value = false;
				
				await PopupNavigation.Instance.PopAllAsync(withAnimation);
			}
		}

		#endregion

		#region Private methods

		private void SetMainViewPrivate<TViewModel, TParam>(TParam param, Action<TViewModel> viewModelInitialization)
			where TViewModel : ViewModelBase
		{
			ClearAll();
			
			var newView = CreateView<TViewModel, TParam, object>(param, viewModelInitialization, null);

			Application.Current.MainPage = newView is FlyoutPage ? newView : _navigationPageFactory.Create(newView);
			
			NavigationView.Popped += (sender, args) => Clear(args.Page);

			if (_firstSetMainView)
			{
				_firstSetMainView = false;
				
				PopupNavigation.Instance.Popped += (sender, args) => Clear(args.Page);
				Application.Current.ModalPopped += (sender, args) => Clear(args.Modal);
			}
		}

		private async Task NavigateToPrivateAsync<TViewModel, TParam, TResult>(
			TParam param, 
			Action<TViewModel> viewModelInitialization, 
			CallbackParam<TResult> callbackParam,
			StackType stackType = StackType.Navigation,
			bool withAnimation = true)
			where TViewModel : ViewModelBase
		{
			if (!_canNavigateNow.Value)
				return;

			using (_canNavigateNow)
			{
				_canNavigateNow.Value = false;

				var newView = CreateView(param, viewModelInitialization, callbackParam);

				await new StackAlgorithmsFactory(NavigationView)
					.Create(stackType)
					.NavigateToAsync(newView, withAnimation);
			}
		}

		private void InsertBeforePrivate<TViewModel, TParam, TResult>(
			ViewModelBase beforeViewModel, 
			TParam param, 
			Action<TViewModel> viewModelInitialization,
			CallbackParam<TResult> callbackParam)
			where TViewModel : ViewModelBase
		{
			var mainParent = GetMainParentFor(beforeViewModel);
			
			if (TryGetFromStack(mainParent, StackType.Navigation, out _, out var index))
			{
				var beforeView = NavigationView.Navigation.NavigationStack[index];
				var newView = CreateView(param, viewModelInitialization, callbackParam);
				
				NavigationView.Navigation.InsertPageBefore(newView, beforeView);
			}
		}
		
		private bool CanClose<TViewModel>(TViewModel viewModel, [NotNullWhen(true)] out StackType? stackType)
			where TViewModel : ViewModelBase
		{
			var mainParent = GetMainParentFor(viewModel);

			if (TryGetStackFor(mainParent, out stackType, out var stack, out var index))
			{
				return stack.Count - 1 == index;
			}

			return false;
		}
		
		private async Task ClosePrivateAsync<TResult>(
			StackType stackType,
			Action<TResult, ViewClosingArgs> viewClosing,
			Action<TResult> viewClosed,
			TResult result,
			bool withAnimation = true)
		{
			if (!_canNavigateNow.Value)
				return;

			var args = new ViewClosingArgs();
			viewClosing?.Invoke(result, args);
			
			if (args.Cancel)
				return;

			using (_canNavigateNow)
			{
				_canNavigateNow.Value = false;

				await new StackAlgorithmsFactory(NavigationView)
					.Create(stackType)
					.CloseAsync(withAnimation);
			}
			
			viewClosed?.Invoke(result);
		}

		private async Task CloseToRootPrivateAsync(
			bool withAnimation = true)
		{
			if (!_canNavigateNow.Value)
				return;
			
			using (_canNavigateNow)
			{
				_canNavigateNow.Value = false;

				ClearToRoot();
				await NavigationView.PopToRootAsync(withAnimation);
			}
		}
		
		private async Task CloseToRootPrivateAsync<TViewModel, TParam>(
			TParam param, 
			Action<TViewModel> viewModelInitialization, 
			bool withAnimation = true)
			where TViewModel : ViewModelBase
		{
			InsertBeforePrivate<TViewModel, TParam, object>(
				NavigationStack.First(),
				param, 
				viewModelInitialization, 
				null);

			await CloseToRootPrivateAsync(withAnimation);
		}
		
		private void SetDetailViewPrivate<TViewModel, TParam>(
			TParam param, 
			Action<TViewModel> viewModelInitialization, 
			bool withCloseFlyoutView = true) 
			where TViewModel : ViewModelBase
		{
			if (!(Application.Current.MainPage is FlyoutPage flyoutPage))
				throw new Exception($"Application.Current.MainPage must be the {nameof(FlyoutPage)} to call the method '{nameof(SetDetailView)}'.");
			
			ClearAll();
			
			var newView = CreateView<TViewModel, TParam, object>(param, viewModelInitialization, null);

			flyoutPage.Detail = _navigationPageFactory.Create(newView);
			
			if (withCloseFlyoutView)
				CloseFlyoutView();
			
			NavigationView.Popped += (sender, args) => Clear(args.Page);
		}

		private Page CreateView<TViewModel, TParam, TResult>(
			TParam param, 
			Action<TViewModel> viewModelInitialization, 
			CallbackParam<TResult> callbackParam) 
			where TViewModel : ViewModelBase
		{
			var viewType = _navigationMapper.GetViewTypeFor<TViewModel>();
			var view = (Page)_resolver.Resolve(viewType, param);
			
			var viewModel = (TViewModel)_resolver.Resolve(typeof(TViewModel), param);
			viewModelInitialization?.Invoke(viewModel);

			view.BindingContext = viewModel;
			
			view.Appearing += (sender, args) => viewModel.OnAppearing();
			view.Appearing += async (sender, args) => await viewModel.OnAppearingAsync();
			view.Disappearing += (sender, args) => viewModel.OnDisappearing();
			view.Disappearing += async (sender, args) => await viewModel.OnDisappearingAsync();

			var children = SetBingingContextForChildren(
				view,
				viewModel,
				param,
				viewModelInitialization,
				callbackParam);

			_viewModelWrappers.Add(
				viewModel,
				new ViewModelWrapperWithResult<TResult>(callbackParam)
				{
					Parent = null,
					Children = children
				});

			return view;
		}

		private List<ViewModelBase> SetBingingContextForChildren<TParentViewModel, TParam, TResult>(
			Page view, 
			ViewModelBase parentViewModel,
			TParam param, 
			Action<TParentViewModel> parentViewModelInitialization,
			CallbackParam<TResult> callbackParam)
			where TParentViewModel : ViewModelBase
		{
			var resultChildren = new List<ViewModelBase>();
			
			var childViews = view is TabbedPage tabbedPage
				? tabbedPage.Children
				: view is FlyoutPage flyoutPage
					? new[] { flyoutPage.Flyout, flyoutPage.Detail }
					: Array.Empty<Page>();
			
			foreach (var childView in childViews)
			{
				var currentView = childView is NavigationPage navigationPage ? navigationPage.RootPage : childView;
				var currentViewModel = null as ViewModelBase;

				if (!currentView.GetType().IsDefined(typeof(ParentBindingContextAttribute)))
				{
					var viewModelType = _navigationMapper.GetViewModelTypeFor(currentView);
					currentViewModel = (ViewModelBase)_resolver.Resolve(viewModelType, param);
					if (currentViewModel is TParentViewModel parentViewModelType)
					{
						parentViewModelInitialization?.Invoke(parentViewModelType);
					}

					currentView.BindingContext = currentViewModel;
					
					currentView.Appearing += (sender, args) => currentViewModel.OnAppearing();
					currentView.Appearing += async (sender, args) => await currentViewModel.OnAppearingAsync();
					currentView.Disappearing += (sender, args) => currentViewModel.OnDisappearing();
					currentView.Disappearing += async (sender, args) => await currentViewModel.OnDisappearingAsync();
				}

				var children = SetBingingContextForChildren(
					currentView, 
					currentViewModel ?? parentViewModel,
					param, 
					parentViewModelInitialization,
					callbackParam);

				if (currentViewModel != null)
				{
					resultChildren.Add(currentViewModel);

					_viewModelWrappers.Add(currentViewModel,
						new ViewModelWrapperWithResult<TResult>(callbackParam)
						{
							Parent = parentViewModel,
							Children = children
						});
				}
				else
				{
					resultChildren.AddRange(children);
				}
			}

			return resultChildren;
		}

		private void Clear(Page page)
		{
			var viewModel = (ViewModelBase)page.BindingContext;
			Clear(viewModel);
		}

		private void Clear<TViewModel>(TViewModel viewModel)
			where TViewModel : ViewModelBase
		{
			if (_viewModelWrappers.TryGetValue(viewModel, out var viewModelWrapper))
			{
				foreach (var children in viewModelWrapper.Children)
				{
					Clear(children);
				}

				_viewModelWrappers.Remove(viewModel);
			}
		}

		private void ClearAll()
		{
			_viewModelWrappers.Clear();
		}

		private void ClearToRoot()
		{
			for (var i = 1; i < NavigationStack.Count; i++)
			{
				var viewModel = NavigationStack[i];
				Clear(viewModel);
			}
		}
		
		#endregion

		#region Private classes

		#region CanNavigateNow

		private sealed class CanNavigateNow : IDisposable
		{
			private volatile bool _value;

			public bool Value
			{
				get => _value;
				set => _value = value;
			}

			public CanNavigateNow(bool value)
			{
				Value = value;
			}

			public void Dispose()
			{
				Value = true;
			}
		}
		
		#endregion

		#region ViewModelWrapper
		
		private interface IViewModelWrapper
		{
			ViewModelBase Parent { get; }
			
			List<ViewModelBase> Children { get; }
		}
		
		private interface IViewModelWrapperWithResult<in TResult> : IViewModelWrapper
		{
			Action<TResult, ViewClosingArgs> ViewClosing { get; }
			
			Action<TResult> ViewClosed { get; }
		}

		private sealed class ViewModelWrapperWithResult<TResult> : IViewModelWrapperWithResult<TResult>
		{
			public ViewModelBase Parent { get; set; }

			public List<ViewModelBase> Children { get; set; } = new List<ViewModelBase>();
			
			public Action<TResult, ViewClosingArgs> ViewClosing  { get; }

			public Action<TResult> ViewClosed { get; }

			public ViewModelWrapperWithResult(CallbackParam<TResult> callbackParam)
			{
				ViewClosing = callbackParam?.ViewClosing;
				ViewClosed = callbackParam?.ViewClosed;
			}
		}
		
		#endregion

		#region StackAlgorithms

		private interface IStackAlgorithms
		{
			List<ViewModelBase> GetStack();

			Task<Page> RemoveAsync(int index, bool withAnimation = true);

			Task NavigateToAsync(Page page, bool withAnimation = true);

			Task CloseAsync(bool withAnimation = true);
		}

		private class StackAlgorithmsFactory
		{
			private NavigationPage NavigationPage { get; }
			
			public static readonly StackType[] StackTypes =
			{
				StackType.Navigation,
				StackType.Modal,
				StackType.Popup
			};

			public StackAlgorithmsFactory(NavigationPage navigationPage)
			{
				NavigationPage = navigationPage;
			}

			public IStackAlgorithms Create(StackType stackType)
			{
				switch (stackType)
				{
					case StackType.Navigation: 
						return new NavigationStackAlgorithms(NavigationPage);
					
					case StackType.Modal: 
						return new ModalStackAlgorithms(NavigationPage);
					
					case StackType.Popup: 
						return new PopupStackAlgorithms();
					
					default:
						throw new InvalidEnumArgumentException(
							$"Value {stackType} of enum {nameof(StackType)} is not supported");
				}
			}
			
			private class NavigationStackAlgorithms : IStackAlgorithms
			{
				private NavigationPage NavigationPage { get; }
				
				public NavigationStackAlgorithms(NavigationPage navigationPage)
				{
					NavigationPage = navigationPage;
				}
				
				public List<ViewModelBase> GetStack()
				{
					return NavigationPage.Navigation.NavigationStack
						.Select(it => (ViewModelBase)it.BindingContext)
						.ToList();
				}

				public async Task<Page> RemoveAsync(int index, bool withAnimation = true)
				{
					return await Task.Run(() =>
					{
						var view = NavigationPage.Navigation.NavigationStack[index];
						NavigationPage.Navigation.RemovePage(view);

						return view;
					});
				}

				public async Task NavigateToAsync(Page page, bool withAnimation = true)
				{
					await NavigationPage.PushAsync(page, withAnimation);
				}

				public async Task CloseAsync(bool withAnimation = true)
				{
					await NavigationPage.PopAsync(withAnimation);
				}
			}
			
			private class ModalStackAlgorithms : IStackAlgorithms
			{
				private NavigationPage NavigationPage { get; }

				public ModalStackAlgorithms(NavigationPage navigationPage)
				{
					NavigationPage = navigationPage;
				}
				
				public List<ViewModelBase> GetStack()
				{
					return NavigationPage.Navigation.ModalStack
						.Select(it => (ViewModelBase)it.BindingContext)
						.ToList();
				}

				public Task<Page> RemoveAsync(int index, bool withAnimation = true)
				{
					throw new NotImplementedException();
				}

				public async Task NavigateToAsync(Page page, bool withAnimation = true)
				{
					await NavigationPage.Navigation.PushModalAsync(page, withAnimation);
				}

				public async Task CloseAsync(bool withAnimation = true)
				{
					await NavigationPage.Navigation.PopModalAsync(withAnimation);
				}
			}
			
			private class PopupStackAlgorithms : IStackAlgorithms
			{
				public List<ViewModelBase> GetStack()
				{
					return PopupNavigation.Instance.PopupStack
						.Select(it => (ViewModelBase)it.BindingContext)
						.ToList();
				}

				public async Task<Page> RemoveAsync(int index, bool withAnimation = true)
				{
					var view = PopupNavigation.Instance.PopupStack[index];
					await PopupNavigation.Instance.RemovePageAsync(view, withAnimation);

					return view;
				}

				public async Task NavigateToAsync(Page page, bool withAnimation = true)
				{
					if (!(page is PopupPage popupPage))
						throw new ArgumentException($"{nameof(page)} must be inherited from the {nameof(PopupPage)}");
					
					await PopupNavigation.Instance.PushAsync(popupPage, withAnimation);
				}

				public async Task CloseAsync(bool withAnimation = true)
				{
					await PopupNavigation.Instance.PopAsync(withAnimation);
				}
			}
		}

		#endregion

		#endregion
    }
}
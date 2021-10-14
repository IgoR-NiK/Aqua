using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Aqua.Core.Builders;
using Aqua.Core.Configs;
using Aqua.Core.Ioc;
using Aqua.Core.Mvvm;
using Aqua.Core.Services;
using Aqua.Core.Utils;

using DryIoc;
using Xamarin.Forms;

namespace Aqua.XamarinForms.Core.Services
{
    public sealed class NavigationService : INavigationService
    {
	    private readonly Dictionary<Type, IStackAlgorithm> _stackAlgorithms;
	    private readonly INavigationViewProvider _navigationViewProvider;
	    private readonly IViewModelWrapperStorage _viewModelWrapperStorage;
	    private readonly INavigationMapper _navigationMapper;
	    private readonly INavigationPageFactory _navigationPageFactory;
	    private readonly IConfigService<NavigationConfig> _navigationConfigService;
	    private readonly IResolver _resolver;
        
	    private readonly CanNavigateNow _canNavigateNow = new CanNavigateNow(true);
	    private bool _firstSetMainView = true;

		public NavigationService(
			IEnumerable<KeyValuePair<Type, IStackAlgorithm>> stackAlgorithms,
			IEnumerable<INavigationModule> navigationModules,
			INavigationViewProvider navigationViewProvider,
			IViewModelWrapperStorage viewModelWrapperStorage,
			INavigationMapper navigationMapper,
			INavigationPageFactory navigationPageFactory,
			IConfigService<NavigationConfig> navigationConfigService,
			IResolver resolver)
		{
			_stackAlgorithms = stackAlgorithms.ToDictionary(it => it.Key, it => it.Value);
			_navigationViewProvider = navigationViewProvider;
			_viewModelWrapperStorage = viewModelWrapperStorage;
			_navigationMapper = navigationMapper;
			_navigationPageFactory = navigationPageFactory;
			_navigationConfigService = navigationConfigService;
			_resolver = resolver;
			
			navigationModules.ForEach(it => it.Map(navigationMapper));
		}

		public IReadOnlyList<ViewModelBase> GetStack<TStack>()
			where TStack : IStack
		{
			return _stackAlgorithms[typeof(TStack)].GetStack();
		}

		public bool TryGetFromStack<TViewModel, TStack>(TViewModel viewModel, out IReadOnlyList<ViewModelBase> stack, out int index)
			where TViewModel : ViewModelBase
			where TStack : IStack
		{
			return _stackAlgorithms[typeof(TStack)].TryGetFromStack(viewModel, out stack, out index);
		}

		public ViewModelBase GetParentFor<TViewModel>(TViewModel viewModel)
			where TViewModel : ViewModelBase
		{
			return _viewModelWrapperStorage.GetValueOrDefault(viewModel)?.Parent;
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
			return _viewModelWrapperStorage.GetValueOrDefault(viewModel)?.Children ?? new List<ViewModelBase>();
		}

		public ViewModelBase GetPreviousFor<TViewModel>(TViewModel viewModel) 
			where TViewModel : ViewModelBase
		{
			var mainParent = GetMainParentFor(viewModel);

			if (TryGetStackAlgorithmFor(mainParent, out _, out var stack, out var index))
			{
				return index == 0
					? null
					: stack[index - 1];
			}

			return null;
		}

		public async Task ExecuteInNavigateSafelyAsync(Func<Task> actionAsync)
		{
			if (!_canNavigateNow.Value)
				return;

			using (_canNavigateNow)
			{
				_canNavigateNow.Value = false;
				
				await actionAsync();
			}
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
			Action<NavigationConfigBuilder> config = null)
			where TViewModel : ViewModelBase
		{
			await NavigateToAsync<TViewModel>(
				null, 
				config);
		}
		
		public async Task NavigateToAsync<TViewModel>(
			Action<TViewModel> viewModelInitialization,
			Action<NavigationConfigBuilder> config = null)
			where TViewModel : ViewModelBase
		{
			await NavigateToPrivateAsync<TViewModel, object, object>(
				null, 
				viewModelInitialization,
				null, 
				config);
		}

		public async Task NavigateToAsync<TViewModel, TParam>(
			TParam param,
			Action<NavigationConfigBuilder> config = null)
			where TViewModel : ViewModelBase, IWithInit<TParam>
		{
			await NavigateToAsync<TViewModel, TParam>(
				param,
				null,
				config);
		}

		public async Task NavigateToAsync<TViewModel, TParam>(
			TParam param,
			Action<TViewModel> viewModelInitialization,
			Action<NavigationConfigBuilder> config = null)
			where TViewModel : ViewModelBase, IWithInit<TParam>
		{
			await NavigateToPrivateAsync<TViewModel, TParam, object>(
				param,
				viewModelInitialization,
				null,
				config);
		}

		public async Task NavigateToAsync<TViewModel, TResult>(
			CallbackParam<TResult> callbackParam,
			Action<NavigationConfigBuilder> config = null)
			where TViewModel : ViewModelBase, IWithResult<TResult>
		{
			await NavigateToAsync<TViewModel, TResult>(
				null,
				callbackParam,
				config);
		}

		public async Task NavigateToAsync<TViewModel, TResult>(
			Action<TViewModel> viewModelInitialization,
			CallbackParam<TResult> callbackParam,
			Action<NavigationConfigBuilder> config = null)
			where TViewModel : ViewModelBase, IWithResult<TResult>
		{
			await NavigateToPrivateAsync<TViewModel, object, TResult>(
				null,
				viewModelInitialization,
				callbackParam,
				config);
		}

		public async Task NavigateToAsync<TViewModel, TParam, TResult>(
			TParam param,
			CallbackParam<TResult> callbackParam,
			Action<NavigationConfigBuilder> config = null)
			where TViewModel : ViewModelBase, IWithInit<TParam>, IWithResult<TResult>
		{
			await NavigateToAsync<TViewModel, TParam, TResult>(
				param,
				null,
				callbackParam,
				config);
		}

		public async Task NavigateToAsync<TViewModel, TParam, TResult>(
			TParam param,
			Action<TViewModel> viewModelInitialization,
			CallbackParam<TResult> callbackParam,
			Action<NavigationConfigBuilder> config = null)
			where TViewModel : ViewModelBase, IWithInit<TParam>, IWithResult<TResult>
		{
			await NavigateToPrivateAsync(
				param,
				viewModelInitialization,
				callbackParam,
				config);
		}

		public async Task NavigateToAsync(
			Type viewModelType,
			Action<NavigationConfigBuilder> config = null)
		{
			await NavigateToAsync<object>(
				viewModelType,
				null,
				null,
				null,
				config);
		}
		
		public async Task NavigateToAsync(
			Type viewModelType,
			Action<ViewModelBase> viewModelInitialization,
			Action<NavigationConfigBuilder> config = null)
		{
			await NavigateToAsync<object>(
				viewModelType,
				null,
				viewModelInitialization,
				null,
				config);
		}

		public async Task NavigateToAsync(
			Type viewModelType,
			object param,
			Action<NavigationConfigBuilder> config = null)
		{
			await NavigateToAsync<object>(
				viewModelType,
				param,
				null,
				null,
				config);
		}

		public async Task NavigateToAsync(
			Type viewModelType,
			object param,
			Action<ViewModelBase> viewModelInitialization,
			Action<NavigationConfigBuilder> config = null)
		{
			await NavigateToAsync<object>(
				viewModelType,
				param,
				viewModelInitialization,
				null,
				config);
		}

		public async Task NavigateToAsync<TResult>(
			Type viewModelType,
			CallbackParam<TResult> callbackParam,
			Action<NavigationConfigBuilder> config = null)
		{
			await NavigateToAsync(
				viewModelType,
				null,
				null,
				callbackParam,
				config);
		}

		public async Task NavigateToAsync<TResult>(
			Type viewModelType,
			Action<ViewModelBase> viewModelInitialization,
			CallbackParam<TResult> callbackParam,
			Action<NavigationConfigBuilder> config = null)
		{
			await NavigateToAsync(
				viewModelType,
				null,
				viewModelInitialization,
				callbackParam,
				config);
		}

		public async Task NavigateToAsync<TResult>(
			Type viewModelType,
			object param,
			CallbackParam<TResult> callbackParam,
			Action<NavigationConfigBuilder> config = null)
		{
			await NavigateToAsync(
				viewModelType,
				param,
				null,
				callbackParam,
				config);
		}

		public async Task NavigateToAsync<TResult>(
			Type viewModelType,
			object param,
			Action<ViewModelBase> viewModelInitialization,
			CallbackParam<TResult> callbackParam,
			Action<NavigationConfigBuilder> config = null)
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
				config);
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
	        if (!CanClose(viewModel, out var stackAlgorithm))
		        return;

	        await ClosePrivateAsync<object>(
		        stackAlgorithm,
		        null,
		        null,
		        null,
		        withAnimation);
        }

        public async Task CloseAsync<TViewModel, TResult>(
	        TViewModel viewModel,
	        TResult result,
	        bool withAnimation = true)
	        where TViewModel : ViewModelBase, IWithResult<TResult>
        {
	        if (!CanClose(viewModel, out var stackAlgorithm))
		        return;

	        var viewModelWrapper = _viewModelWrapperStorage.GetValueOrDefault(viewModel);
	        
	        await ClosePrivateAsync(
		        stackAlgorithm,
		        (viewModelWrapper as IViewModelWrapper<TResult>)?.ViewClosing,
		        (viewModelWrapper as IViewModelWrapper<TResult>)?.ViewClosed,
		        result,
		        withAnimation);
        }
        
        public async Task CloseAsync(
	        Action<NavigationConfigBuilder> config)
        {
	        var navigationConfig = new NavigationConfigBuilder(_navigationConfigService.Get(), config).Instance;

	        await ClosePrivateAsync<object>(
		        _stackAlgorithms[navigationConfig.StackType],
		        null,
		        null,
		        null,
		        navigationConfig.WithAnimation);
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
	        Action<NavigationConfigBuilder> config = null)
        {
	        var navigationConfig = new NavigationConfigBuilder(_navigationConfigService.Get(), config).Instance;
	        
	        var view = await _stackAlgorithms[navigationConfig.StackType].RemoveAsync(index);
	        
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

	        if (TryGetStackAlgorithmFor(mainParent, out var stackAlgorithm, out _, out var index))
	        {
		        var view = await stackAlgorithm.RemoveAsync(index);
	        
		        Clear(view);
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


		#region Private methods

		private void SetMainViewPrivate<TViewModel, TParam>(TParam param, Action<TViewModel> viewModelInitialization)
			where TViewModel : ViewModelBase
		{
			_viewModelWrapperStorage.ClearAll();
			
			var newView = CreateView<TViewModel, TParam, object>(param, viewModelInitialization, null);

			Application.Current.MainPage = newView is FlyoutPage ? newView : _navigationPageFactory.Create(newView);
			
			_navigationViewProvider.NavigationView.Popped += (sender, args) => Clear(args.Page);

			if (_firstSetMainView)
			{
				_firstSetMainView = false;
				
				Application.Current.ModalPopped += (sender, args) => Clear(args.Modal);
			}
		}

		private async Task NavigateToPrivateAsync<TViewModel, TParam, TResult>(
			TParam param, 
			Action<TViewModel> viewModelInitialization, 
			CallbackParam<TResult> callbackParam,
			Action<NavigationConfigBuilder> config = null)
			where TViewModel : ViewModelBase
		{
			await ExecuteInNavigateSafelyAsync(async () =>
			{
				var navigationConfig = new NavigationConfigBuilder(_navigationConfigService.Get(), config).Instance;
				var newView = CreateView(param, viewModelInitialization, callbackParam);

				await _stackAlgorithms[navigationConfig.StackType].NavigateToAsync(newView, navigationConfig.WithAnimation);
			});
		}

		private void InsertBeforePrivate<TViewModel, TParam, TResult>(
			ViewModelBase beforeViewModel, 
			TParam param, 
			Action<TViewModel> viewModelInitialization,
			CallbackParam<TResult> callbackParam)
			where TViewModel : ViewModelBase
		{
			var mainParent = GetMainParentFor(beforeViewModel);
			
			if (_stackAlgorithms[typeof(NavigationStack)].TryGetFromStack(mainParent, out _, out var index))
			{
				var beforeView = _navigationViewProvider.NavigationView.Navigation.NavigationStack[index];
				var newView = CreateView(param, viewModelInitialization, callbackParam);
				
				_navigationViewProvider.NavigationView.Navigation.InsertPageBefore(newView, beforeView);
			}
		}
		
		private bool CanClose<TViewModel>(TViewModel viewModel, [NotNullWhen(true)] out IStackAlgorithm stackAlgorithm)
			where TViewModel : ViewModelBase
		{
			var mainParent = GetMainParentFor(viewModel);

			if (TryGetStackAlgorithmFor(mainParent, out stackAlgorithm, out var stack, out var index))
			{
				return stack.Count - 1 == index;
			}

			return false;
		}

		private async Task ClosePrivateAsync<TResult>(
			IStackAlgorithm stackAlgorithm,
			Action<TResult, ViewClosingArgs> viewClosing,
			Action<TResult> viewClosed,
			TResult result,
			bool withAnimation = true)
		{
			await ExecuteInNavigateSafelyAsync(async () =>
			{
				var args = new ViewClosingArgs();
				viewClosing?.Invoke(result, args);

				if (args.Cancel)
					return;

				await stackAlgorithm.CloseAsync(withAnimation);

				viewClosed?.Invoke(result);
			});
		}

		private async Task CloseToRootPrivateAsync(
			bool withAnimation = true)
		{
			await ExecuteInNavigateSafelyAsync(async () =>
			{
				ClearToRoot();
				await _navigationViewProvider.NavigationView.PopToRootAsync(withAnimation);
			});
		}
		
		private async Task CloseToRootPrivateAsync<TViewModel, TParam>(
			TParam param, 
			Action<TViewModel> viewModelInitialization, 
			bool withAnimation = true)
			where TViewModel : ViewModelBase
		{
			InsertBeforePrivate<TViewModel, TParam, object>(
				GetStack<NavigationStack>().First(),
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
			
			_viewModelWrapperStorage.ClearAll();
			
			var newView = CreateView<TViewModel, TParam, object>(param, viewModelInitialization, null);

			flyoutPage.Detail = _navigationPageFactory.Create(newView);
			
			if (withCloseFlyoutView)
				CloseFlyoutView();
			
			_navigationViewProvider.NavigationView.Popped += (sender, args) => Clear(args.Page);
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

			_viewModelWrapperStorage.Add(
				viewModel,
				new ViewModelWrapper<TResult>(callbackParam)
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

					_viewModelWrapperStorage.Add(currentViewModel,
						new ViewModelWrapper<TResult>(callbackParam)
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
			_viewModelWrapperStorage.Clear(viewModel);
		}

		private void ClearToRoot()
		{
			var navigationStack = GetStack<NavigationStack>();
			for (var i = 1; i < navigationStack.Count; i++)
			{
				var viewModel = navigationStack[i];
				_viewModelWrapperStorage.Clear(viewModel);
			}
		}

		private bool TryGetStackAlgorithmFor<TViewModel>(TViewModel viewModel, [NotNullWhen(true)] out IStackAlgorithm stackAlgorithm, out IReadOnlyList<ViewModelBase> stack, out int index)
			where TViewModel : ViewModelBase
		{
			foreach (var (_, value) in _stackAlgorithms)
			{
				if (value.TryGetFromStack(viewModel, out stack, out index))
				{
					stackAlgorithm = value;
					return true;
				}
			}

			stackAlgorithm = null;
			stack = new List<ViewModelBase>();
			index = -1;

			return false;
		}

		#endregion
    }
}
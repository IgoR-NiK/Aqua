using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Aqua.Core.Builders;
using Aqua.Core.Ioc;
using Aqua.Core.Mvvm;
using Aqua.Core.Services;
using Aqua.Core.Utils;

namespace Aqua.XamarinForms.Core.Services
{
    public interface INavigationService : IResolvable
    {
        IReadOnlyList<ViewModelBase> GetStack<TStack>() 
            where TStack : IStack;
        
        bool TryGetFromStack<TViewModel, TStack>(TViewModel viewModel, out IReadOnlyList<ViewModelBase> stack, out int index)
            where TViewModel : ViewModelBase
            where TStack : IStack;
        
        ViewModelBase GetParentFor<TViewModel>(TViewModel viewModel)
            where TViewModel : ViewModelBase;
        
        ViewModelBase GetMainParentFor<TViewModel>(TViewModel viewModel)
            where TViewModel : ViewModelBase;
        
        IReadOnlyList<ViewModelBase> GetChildrenFor<TViewModel>(TViewModel viewModel)
            where TViewModel : ViewModelBase;

        ViewModelBase GetPreviousFor<TViewModel>(TViewModel viewModel)
            where TViewModel : ViewModelBase;

        Task ExecuteInNavigateSafelyAsync(Func<Task> actionAsync);

        #region SetMainView

        void SetMainView<TViewModel>()
            where TViewModel : ViewModelBase;
        
        void SetMainView<TViewModel>(Action<TViewModel> viewModelInitialization)
            where TViewModel : ViewModelBase;
        
        void SetMainView<TViewModel, TParam>(TParam param)
            where TViewModel : ViewModelBase, IWithInit<TParam>;
        
        void SetMainView<TViewModel, TParam>(TParam param, Action<TViewModel> viewModelInitialization)
            where TViewModel : ViewModelBase, IWithInit<TParam>;

        void SetMainView(Type viewModelType);
        
        void SetMainView(Type viewModelType, Action<ViewModelBase> viewModelInitialization);
        
        void SetMainView(Type viewModelType, object param);
        
        void SetMainView(Type viewModelType, object param, Action<ViewModelBase> viewModelInitialization);

        #endregion

        #region NavigateToAsync

        Task NavigateToAsync<TViewModel>(
            Action<NavigationConfigBuilder> config = null)
            where TViewModel : ViewModelBase;
        
        Task NavigateToAsync<TViewModel>(
            Action<TViewModel> viewModelInitialization,
            Action<NavigationConfigBuilder> config = null)
            where TViewModel : ViewModelBase;
        
        Task NavigateToAsync<TViewModel, TParam>(
            TParam param,
            Action<NavigationConfigBuilder> config = null)
            where TViewModel : ViewModelBase, IWithInit<TParam>;
        
        Task NavigateToAsync<TViewModel, TParam>(
            TParam param,
            Action<TViewModel> viewModelInitialization,
            Action<NavigationConfigBuilder> config = null)
            where TViewModel : ViewModelBase, IWithInit<TParam>;
        
        Task NavigateToAsync<TViewModel, TResult>(
            CallbackParam<TResult> callbackParam, 
            Action<NavigationConfigBuilder> config = null)
            where TViewModel : ViewModelBase, IWithResult<TResult>;
        
        Task NavigateToAsync<TViewModel, TResult>(
            Action<TViewModel> viewModelInitialization,
            CallbackParam<TResult> callbackParam, 
            Action<NavigationConfigBuilder> config = null)
            where TViewModel : ViewModelBase, IWithResult<TResult>;
        
        Task NavigateToAsync<TViewModel, TParam, TResult>(
            TParam param,
            CallbackParam<TResult> callbackParam, 
            Action<NavigationConfigBuilder> config = null)
            where TViewModel : ViewModelBase, IWithInit<TParam>, IWithResult<TResult>;
        
        Task NavigateToAsync<TViewModel, TParam, TResult>(
            TParam param,
            Action<TViewModel> viewModelInitialization,
            CallbackParam<TResult> callbackParam, 
            Action<NavigationConfigBuilder> config = null)
            where TViewModel : ViewModelBase, IWithInit<TParam>, IWithResult<TResult>;

        Task NavigateToAsync(
            Type viewModelType,
            Action<NavigationConfigBuilder> config = null);
        
        Task NavigateToAsync(
            Type viewModelType,
            Action<ViewModelBase> viewModelInitialization,
            Action<NavigationConfigBuilder> config = null);
        
        Task NavigateToAsync(
            Type viewModelType,
            object param,
            Action<NavigationConfigBuilder> config = null);
        
        Task NavigateToAsync(
            Type viewModelType,
            object param,
            Action<ViewModelBase> viewModelInitialization,
            Action<NavigationConfigBuilder> config = null);

        Task NavigateToAsync<TResult>(
            Type viewModelType,
            CallbackParam<TResult> callbackParam, 
            Action<NavigationConfigBuilder> config = null);
        
        Task NavigateToAsync<TResult>(
            Type viewModelType,
            Action<ViewModelBase> viewModelInitialization,
            CallbackParam<TResult> callbackParam, 
            Action<NavigationConfigBuilder> config = null);
        
        Task NavigateToAsync<TResult>(
            Type viewModelType,
            object param,
            CallbackParam<TResult> callbackParam, 
            Action<NavigationConfigBuilder> config = null);
        
        Task NavigateToAsync<TResult>(
            Type viewModelType,
            object param,
            Action<ViewModelBase> viewModelInitialization,
            CallbackParam<TResult> callbackParam,
            Action<NavigationConfigBuilder> config = null);

        #endregion

        #region InsertBefore

        void InsertBefore<TViewModel>(
            ViewModelBase beforeViewModel)
            where TViewModel : ViewModelBase;
        
        void InsertBefore<TViewModel>(
            ViewModelBase beforeViewModel,
            Action<TViewModel> viewModelInitialization)
            where TViewModel : ViewModelBase;

        void InsertBefore<TViewModel, TParam>(
            ViewModelBase beforeViewModel, 
            TParam param)
            where TViewModel : ViewModelBase, IWithInit<TParam>;

        void InsertBefore<TViewModel, TParam>(
            ViewModelBase beforeViewModel,
            TParam param,
            Action<TViewModel> viewModelInitialization)
            where TViewModel : ViewModelBase, IWithInit<TParam>;
        
        void InsertBefore<TViewModel, TResult>(
            ViewModelBase beforeViewModel,
            CallbackParam<TResult> callbackParam)
            where TViewModel : ViewModelBase, IWithResult<TResult>;
        
        void InsertBefore<TViewModel, TResult>(
            ViewModelBase beforeViewModel,
            Action<TViewModel> viewModelInitialization,
            CallbackParam<TResult> callbackParam)
            where TViewModel : ViewModelBase, IWithResult<TResult>;
        
        void InsertBefore<TViewModel, TParam, TResult>(
            ViewModelBase beforeViewModel,
            TParam param,
            CallbackParam<TResult> callbackParam)
            where TViewModel : ViewModelBase, IWithInit<TParam>, IWithResult<TResult>;

        void InsertBefore<TViewModel, TParam, TResult>(
            ViewModelBase beforeViewModel,
            TParam param,
            Action<TViewModel> viewModelInitialization,
            CallbackParam<TResult> callbackParam)
            where TViewModel : ViewModelBase, IWithInit<TParam>, IWithResult<TResult>;

        void InsertBefore(
            ViewModelBase beforeViewModel,
            Type viewModelType);
        
        void InsertBefore(
            ViewModelBase beforeViewModel,
            Type viewModelType,
            Action<ViewModelBase> viewModelInitialization);
        
        void InsertBefore(
            ViewModelBase beforeViewModel,
            Type viewModelType,
            object param);

        void InsertBefore(
            ViewModelBase beforeViewModel,
            Type viewModelType,
            object param,
            Action<ViewModelBase> viewModelInitialization);

        void InsertBefore<TResult>(
            ViewModelBase beforeViewModel,
            Type viewModelType,
            CallbackParam<TResult> callbackParam);
        
        void InsertBefore<TResult>(
            ViewModelBase beforeViewModel,
            Type viewModelType,
            Action<ViewModelBase> viewModelInitialization,
            CallbackParam<TResult> callbackParam);
        
        void InsertBefore<TResult>(
            ViewModelBase beforeViewModel,
            Type viewModelType,
            object param,
            CallbackParam<TResult> callbackParam);

        void InsertBefore<TResult>(
            ViewModelBase beforeViewModel,
            Type viewModelType,
            object param,
            Action<ViewModelBase> viewModelInitialization,
            CallbackParam<TResult> callbackParam);

        #endregion

        #region CloseAsync

        Task CloseAsync<TViewModel>(
            TViewModel viewModel,
            bool withAnimation = true)
            where TViewModel : ViewModelBase;
        
        Task CloseAsync<TViewModel, TResult>(
            TViewModel viewModel,
            TResult result,
            bool withAnimation = true)
            where TViewModel : ViewModelBase, IWithResult<TResult>;

        Task CloseAsync(
            Action<NavigationConfigBuilder> config);
        
        #endregion
    
        #region CloseToRootAsync

        Task CloseToRootAsync(
            bool withAnimation = true);
        
        Task CloseToRootAsync<TViewModel>(
            bool withAnimation = true)
            where TViewModel : ViewModelBase;
        
        Task CloseToRootAsync<TViewModel>(
            Action<TViewModel> viewModelInitialization, 
            bool withAnimation = true)
            where TViewModel : ViewModelBase;
        
        Task CloseToRootAsync<TViewModel, TParam>(
            TParam param, 
            bool withAnimation = true)
            where TViewModel : ViewModelBase, IWithInit<TParam>;
        
        Task CloseToRootAsync<TViewModel, TParam>(
            TParam param, 
            Action<TViewModel> viewModelInitialization, 
            bool withAnimation = true)
            where TViewModel : ViewModelBase, IWithInit<TParam>;

        Task CloseToRootAsync(
            Type viewModelType, 
            bool withAnimation = true);
        
        Task CloseToRootAsync(
            Type viewModelType, 
            Action<ViewModelBase> viewModelInitialization, 
            bool withAnimation = true);
        
        Task CloseToRootAsync(
            Type viewModelType, 
            object param, 
            bool withAnimation = true);
        
        Task CloseToRootAsync(
            Type viewModelType, 
            object param, 
            Action<ViewModelBase> viewModelInitialization, 
            bool withAnimation = true);

        #endregion

        #region RemoveAsync

        Task RemoveByAsync(
            int index, 
            Action<NavigationConfigBuilder> config = null);

        Task RemoveAsync<TViewModel>(
            TViewModel viewModel,
            bool withAnimation = true)
            where TViewModel : ViewModelBase;

        #endregion

        #region SetDetailView

        void SetDetailView<TViewModel>(
            bool withCloseFlyoutView = true)
            where TViewModel : ViewModelBase;
        
        void SetDetailView<TViewModel>(
            Action<TViewModel> viewModelInitialization,
            bool withCloseFlyoutView = true)
            where TViewModel : ViewModelBase;
        
        void SetDetailView<TViewModel, TParam>(
            TParam param, 
            bool withCloseFlyoutView = true)
            where TViewModel : ViewModelBase, IWithInit<TParam>;
        
        void SetDetailView<TViewModel, TParam>(
            TParam param,
            Action<TViewModel> viewModelInitialization,
            bool withCloseFlyoutView = true)
            where TViewModel : ViewModelBase, IWithInit<TParam>;

        void SetDetailView(
            Type viewModelType,
            bool withCloseFlyoutView = true);

        void SetDetailView(
            Type viewModelType,
            Action<ViewModelBase> viewModelInitialization,
            bool withCloseFlyoutView = true);
        
        void SetDetailView(
            Type viewModelType, 
            object param, 
            bool withCloseFlyoutView = true);

        void SetDetailView(
            Type viewModelType,
            object param,
            Action<ViewModelBase> viewModelInitialization,
            bool withCloseFlyoutView = true);

        #endregion

        #region CloseFlyoutView

        void CloseFlyoutView();

        #endregion
    }
}
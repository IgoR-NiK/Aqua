using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Aqua.Core.Mvvm;
using Aqua.Core.Utils;
using Xamarin.Forms;

namespace Aqua.XamarinForms.Core.Services.Navigation
{
    public abstract class NavigationServiceConfiguratorBase : INavigationServiceConfigurator
    {
        private Func<Page, NavigationPage> _navigationPageCreator;
        
        protected Dictionary<Type, Type> ViewModelTypeToViewTypeMap { get; } = new Dictionary<Type, Type>();
        protected Dictionary<Type, Type> ViewTypeToViewModelTypeMap { get; } = new Dictionary<Type, Type>();

        public Assembly[] AssembliesForSearch { get; set; }
        
        public bool UseAutoMappingViewModelToView { get; set; } = true;

        public Func<Page, NavigationPage> NavigationPageCreator
        {
            get => _navigationPageCreator ?? (page => new NavigationPage(page));
            set => _navigationPageCreator = value;
        }

        public abstract void Map<TViewModel, TView>()
            where TViewModel : ViewModelBase
            where TView : Page;

        public abstract void Map(Type viewModelType, Type viewType);
        
        void INavigationServiceConfigurator.AutoMappingViewModelToView()
        {
            if (!UseAutoMappingViewModelToView) 
                return;

            var assembliesForSearch =
                (AssembliesForSearch ?? typeof(NavigationService).GetDependentAssemblies())
                    .Union(new[] { typeof(NavigationService).Assembly })
                    .ToArray();
            
            var viewModelTypes = assembliesForSearch
                .SelectMany(it => it.GetTypes())
                .Where(ViewModelPredicate);

            var viewTypes = assembliesForSearch
                .SelectMany(it => it.GetTypes())
                .Where(ViewPredicate)
                .ToDictionary(it => it.Name.ToUpperInvariant());

            viewModelTypes.ForEach(vm =>
            {
                var viewModelName = vm.Name.ToUpperInvariant();

                viewModelName = viewModelName.Replace("VIEWMODEL", string.Empty);

                var view = viewTypes.GetValueOrDefault($"{viewModelName}VIEW");

                if (view == null)
                    throw new ArgumentNullException(nameof(view), $"The view cannot be found for {vm.Name}");

                ViewModelTypeToViewTypeMap.TryAdd(vm, view);
                ViewTypeToViewModelTypeMap.TryAdd(view, vm);
            });
        }

        Type INavigationServiceConfigurator.GetViewTypeFor<TViewModel>()
        {
            return ViewModelTypeToViewTypeMap[typeof(TViewModel)];
        }

        Type INavigationServiceConfigurator.GetViewModelTypeFor(Page view)
        {
            return ViewTypeToViewModelTypeMap[view.GetType()];
        }
        
        protected abstract Func<Type, bool> ViewModelPredicate { get; }
        
        protected abstract Func<Type, bool> ViewPredicate { get; }
    }
}
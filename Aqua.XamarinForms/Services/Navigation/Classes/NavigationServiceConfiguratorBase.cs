using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Aqua.Core.Extensions;
using Aqua.XamarinForms.Mvvm;
using Aqua.XamarinForms.Services.Navigation.Interfaces;

using Xamarin.Forms;

namespace Aqua.XamarinForms.Services.Navigation.Classes
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
            where TView : Page, new();

        void INavigationServiceConfigurator.AutoMappingViewModelToView()
        {
            if (!UseAutoMappingViewModelToView) 
                return;

            var assembliesForSearch =
                (AssembliesForSearch ?? AppDomain.CurrentDomain.GetAssemblies())
                    .Union(new[] { typeof(NavigationService).Assembly })
                    .ToArray();
            
            var viewModelTypes = assembliesForSearch
                .SelectMany(it => it.GetTypes())
                .Where(ViewModelPredicate);

            var viewTypes = assembliesForSearch
                .SelectMany(it => it.GetTypes())
                .Where(it =>
                    typeof(Page).IsAssignableFrom(it)
                    && !it.IsAbstract
                    && it.GetConstructor(Type.EmptyTypes) != null)
                .ToList();

            viewModelTypes.ForEach(vm =>
            {
                var viewModelName = vm.Name.ToUpperInvariant();

                viewModelName = viewModelName.Replace("VIEWMODEL", string.Empty);

                var view = viewTypes.SingleOrDefault(v => v.Name.ToUpperInvariant() == $"{viewModelName}VIEW");

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
    }
}
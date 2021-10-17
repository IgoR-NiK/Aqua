using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aqua.Core.Mvvm;
using Aqua.XamarinForms.Core.Services;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Aqua.XamarinForms.Popup
{
    internal sealed class PopupStackAlgorithm : StackAlgorithmBase
    {
        public PopupStackAlgorithm(IViewModelWrapperStorage viewModelWrapperStorage)
        {
            PopupNavigation.Instance.Popped += (sender, args) =>
            {
                var viewModel = (ViewModelBase)args.Page.BindingContext;
                viewModelWrapperStorage.Clear(viewModel);
            };
        }
        
        public override IReadOnlyList<ViewModelBase> GetStack()
            => PopupNavigation.Instance.PopupStack
                .Select(it => (ViewModelBase)it.BindingContext)
                .ToList();

        public override async Task NavigateToAsync(Page page, bool withAnimation = true)
        {
            if (!(page is PopupPage popupPage))
                throw new ArgumentException($"{nameof(page)} must be inherited from the {nameof(PopupPage)}");
					
            await PopupNavigation.Instance.PushAsync(popupPage, withAnimation);
        }

        public override async Task CloseAsync(bool withAnimation = true)
            => await PopupNavigation.Instance.PopAsync(withAnimation);

        public override async Task<Page> RemoveAsync(int index, bool withAnimation = true)
        {
            var view = PopupNavigation.Instance.PopupStack[index];
            await PopupNavigation.Instance.RemovePageAsync(view, withAnimation);

            return view;
        }
    }
}
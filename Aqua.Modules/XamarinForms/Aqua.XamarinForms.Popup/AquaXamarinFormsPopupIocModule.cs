using Aqua.Core.Ioc;
using Aqua.XamarinForms.Core.Services;
using DryIoc;

namespace Aqua.XamarinForms.Popup
{
    public sealed class AquaXamarinFormsPopupIocModule : IIocModule
    {
        public void RegisterTypes(IRegistrator container)
        {
            container.Register<IStackAlgorithm, PopupStackAlgorithm>(Reuse.Singleton, serviceKey: typeof(PopupStack));
        }
    }
}
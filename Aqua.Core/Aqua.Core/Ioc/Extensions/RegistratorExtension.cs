using Aqua.Core.Mvvm;
using DryIoc;

namespace Aqua.Core.Ioc
{
    public static class RegistratorExtension
    {
        public static void RegisterViewModel<TViewModel>(this IRegistrator registrator)
            where TViewModel : IViewModel
            => registrator.Register<IViewModel, TViewModel>(serviceKey: typeof(TViewModel));

        public static void RegisterView<TView>(this IRegistrator registrator)
            where TView : IView
            => registrator.Register<IView, TView>(serviceKey: typeof(TView));
    }
}
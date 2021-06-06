namespace Aqua.Core.Mvvm
{
    public interface IView<out TViewModel> : IView
        where TViewModel : class, IViewModel
    {
        TViewModel ViewModel { get; }
    }
}
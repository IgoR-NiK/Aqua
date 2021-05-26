using Aqua.Core.Interfaces;

namespace Aqua.Core.Mvvm
{
    public interface IViewModel : IResolvable
    {
        string Title { get; set; }

        void OnAppearing();

        void OnDisappearing();
    }
}
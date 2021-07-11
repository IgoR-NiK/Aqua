using System.Threading.Tasks;

using Aqua.Core.Contexts;

namespace Aqua.Core.Mvvm
{
    public interface IViewModel : IViewModelContext
    {
        Task OnAppearing();

        Task OnDisappearing();
    }
}
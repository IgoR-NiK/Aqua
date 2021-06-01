using System.Threading.Tasks;

using Aqua.Core.Interfaces;

namespace Aqua.Core.Mvvm
{
    public interface IViewModel : IResolvable
    {
        string Title { get; set; }

        Task OnAppearing();

        Task OnDisappearing();
    }
}
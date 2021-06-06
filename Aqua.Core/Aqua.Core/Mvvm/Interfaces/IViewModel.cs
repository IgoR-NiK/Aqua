using System.Threading.Tasks;

using Aqua.Core.IoC;

namespace Aqua.Core.Mvvm
{
    public interface IViewModel : IResolvable
    {
        string Title { get; set; }

        Task OnAppearing();

        Task OnDisappearing();
    }
}
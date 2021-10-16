using System.Threading.Tasks;
using Aqua.Core.Ioc;

namespace Aqua.Core.Mvvm
{
    public interface IViewModel : IResolvable
    {
        string Title { get; set; }
        
        void OnAppearing();
        
        Task OnAppearingAsync();

        void OnDisappearing();
        
        Task OnDisappearingAsync();
    }
}
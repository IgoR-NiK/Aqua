using System.Threading.Tasks;

using Aqua.Core.Utils;

namespace Aqua.Core.Mvvm
{
    public abstract class ViewModelBase : RaisableObject, IViewModel
    {
        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        
        public virtual void OnAppearing() { }

        public virtual Task OnAppearingAsync() => Task.CompletedTask;

        public virtual void OnDisappearing() { }

        public virtual Task OnDisappearingAsync() => Task.CompletedTask;
    }
}
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Aqua.Core.Commands
{
    public interface IAquaCommandBase : ICommand, INotifyPropertyChanged
    {
        bool IsExecuting { get; }
        
        bool IsFaulted { get; }
        
        event Action<bool> IsExecutingChanged;

        event Action CheckCanExecuteFunc;

        bool CanExecuteFunc(object parameter);

        void RaiseCanExecuteChanged();

        void IsNotExecuting();
    }
}
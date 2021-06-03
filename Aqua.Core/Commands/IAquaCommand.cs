using System;
using System.Windows.Input;

namespace Aqua.Core.Commands
{
    public interface IAquaCommand : ICommand
    {
        bool IsExecuting { get; }
        
        event Action<bool> IsExecutingChanged;

        void RaiseCanExecuteChanged();

        void IsNotExecuting();
    }
}
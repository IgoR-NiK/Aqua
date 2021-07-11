﻿using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Aqua.Core.Commands
{
    public interface IAquaCommand : ICommand, INotifyPropertyChanged
    {
        bool IsExecuting { get; }
        
        event Action<bool> IsExecutingChanged;

        void RaiseCanExecuteChanged();

        void IsNotExecuting();
    }
}
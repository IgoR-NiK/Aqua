using System;

namespace Aqua.Core.Commands
{
    public class AquaCommand : AquaCommandBase, IAquaCommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;
        
        public Action<Exception> FaultedHandler { get; set; }
        
        protected AquaCommand() { }

        public AquaCommand(Action execute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public AquaCommand(Action execute, Func<bool> canExecute)
            : this(execute)
        {
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        public void Execute()
        {
            if (!CanExecute())
                return;

            IsExecuting = true;
            IsFaulted = false;

            try
            {
                (_execute ?? ExecuteInternal).Invoke();
            }
            catch (Exception exception)
            {
                IsFaulted = true;
                if (FaultedHandler != null)
                {
                    FaultedHandler(exception);
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                IsExecuting = false;
            }
        }

        public bool CanExecute()
            => !IsExecuting && (_canExecute ?? CanExecuteInternal).Invoke();

        protected virtual void ExecuteInternal() { }

        protected virtual bool CanExecuteInternal() => true;

        private protected sealed override void ExecuteCore(object parameter)
            => Execute();

        private protected sealed override bool CanExecuteCore(object parameter)
            => CanExecute();
        
        private protected sealed override bool CanExecuteFunc(object parameter)
            => (_canExecute ?? CanExecuteInternal).Invoke();
    }
}
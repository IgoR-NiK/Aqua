using System;

namespace Aqua.Core.Commands
{
    public class AquaCommand<TParam> : AquaCommandBase, IAquaCommand<TParam>
    {
        private readonly Action<TParam> _execute;
        private readonly Func<TParam, bool> _canExecute;
        
        public Action<TParam, Exception> FaultedHandler { get; set; }
        
        protected AquaCommand() { }
        
        public AquaCommand(Action<TParam> execute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public AquaCommand(Action<TParam> execute, Func<TParam, bool> canExecute)
            : this(execute)
        {
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        public void Execute(TParam parameter)
        {
            if (!CanExecute(parameter))
                return;
            
            IsExecuting = true;
            IsFaulted = false;

            try
            {
                (_execute ?? ExecuteInternal).Invoke(parameter);
            }
            catch (Exception exception)
            {
                IsFaulted = true;
                if (FaultedHandler != null)
                {
                    FaultedHandler(parameter, exception);
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
        
        public bool CanExecute(TParam parameter)
            => !IsExecuting && (_canExecute ?? CanExecuteInternal).Invoke(parameter);
        
        protected virtual void ExecuteInternal(TParam parameter) { }

        protected virtual bool CanExecuteInternal(TParam parameter) => true;

        private protected sealed override void ExecuteCore(object parameter)
        {
            if (IsValidParameter<TParam>(parameter))
                Execute((TParam)parameter);
        }

        private protected sealed override bool CanExecuteCore(object parameter)
            => IsValidParameter<TParam>(parameter) && CanExecute((TParam)parameter);
        
        private protected sealed override bool CanExecuteFunc(object parameter)
            => IsValidParameter<TParam>(parameter) && (_canExecute ?? CanExecuteInternal).Invoke((TParam)parameter);
    }
}
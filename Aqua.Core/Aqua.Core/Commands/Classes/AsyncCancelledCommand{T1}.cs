using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aqua.Core.Commands
{
    public class AsyncCancelledCommand<T> : AquaCommandBase, IAsyncCancelledCommand
    {
        private CancellationTokenSource _cancellationTokenSource;
        
        private readonly Func<T, CancellationToken, Task> _execute;
        private readonly Func<T, OperationCanceledException, Task> _cancelled;
        private readonly Func<T, bool> _canExecute;
        
        public TimeSpan? Timeout { get; set; }
        
        public AsyncCancelledCommand() { }
        
        public AsyncCancelledCommand(Func<T, CancellationToken, Task> execute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }
        
        public AsyncCancelledCommand(Func<T, CancellationToken, Task> execute, Func<T, OperationCanceledException, Task> cancelled)
            : this(execute)
        {
            _cancelled = cancelled ?? throw new ArgumentNullException(nameof(cancelled));
        }

        public AsyncCancelledCommand(Func<T, CancellationToken, Task> execute, Func<T, bool> canExecute)
            : this(execute)
        {
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }
        
        public AsyncCancelledCommand(Func<T, CancellationToken, Task> execute, Func<T, OperationCanceledException, Task> cancelled, Func<T, bool> canExecute)
            : this(execute, cancelled)
        {
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }
        
        public async Task ExecuteAsync(T parameter)
        {
            if (!CanExecute(parameter))
                return;
            
            IsExecuting = true;

            _cancellationTokenSource = new CancellationTokenSource();
            
            if (Timeout.HasValue)
                _cancellationTokenSource.CancelAfter(Timeout.Value);

            try
            {
                await (_execute ?? ExecuteInternal).Invoke(parameter, _cancellationTokenSource.Token);
            }
            catch (OperationCanceledException exception)
                when (_cancellationTokenSource.IsCancellationRequested)
            {
                await (_cancelled ?? CancelledInternal).Invoke(parameter, exception);
            }
            finally
            {
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
                
                IsExecuting = false;
            }
        }
        
        public bool CanExecute(T parameter)
            => !IsExecuting && (_canExecute ?? CanExecuteInternal).Invoke(parameter);

        public void Cancel() => _cancellationTokenSource?.Cancel();
        
        protected virtual Task ExecuteInternal(T parameter, CancellationToken token) => Task.CompletedTask;

        protected virtual Task CancelledInternal(T parameter, OperationCanceledException exception) => Task.CompletedTask;

        protected virtual bool CanExecuteInternal(T parameter) => true;
        
        private protected sealed override async void ExecuteCore(object parameter)
        {
            if (IsValidParameter<T>(parameter))
                await ExecuteAsync((T)parameter);
        }

        private protected sealed override bool CanExecuteCore(object parameter)
            => IsValidParameter<T>(parameter) && CanExecute((T)parameter);
        
        private protected sealed override bool CanExecuteFunc(object parameter)
            => IsValidParameter<T>(parameter) && (_canExecute ?? CanExecuteInternal).Invoke((T)parameter);
    }
}
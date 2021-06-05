using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aqua.Core.Commands
{
    public class AsyncCancelledCommand : AquaCommandBase, IAsyncCancelledCommand
    {
        private CancellationTokenSource _cancellationTokenSource;
        
        private readonly Func<CancellationToken, Task> _execute;
        private readonly Func<OperationCanceledException, Task> _cancelled;
        private readonly Func<bool> _canExecute;
        
        public TimeSpan? Timeout { get; set; }
        
        public AsyncCancelledCommand() { }
        
        public AsyncCancelledCommand(Func<CancellationToken, Task> execute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }
        
        public AsyncCancelledCommand(Func<CancellationToken, Task> execute, Func<OperationCanceledException, Task> cancelled)
            : this(execute)
        {
            _cancelled = cancelled ?? throw new ArgumentNullException(nameof(cancelled));
        }

        public AsyncCancelledCommand(Func<CancellationToken, Task> execute, Func<bool> canExecute)
            : this(execute)
        {
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }
        
        public AsyncCancelledCommand(Func<CancellationToken, Task> execute, Func<OperationCanceledException, Task> cancelled, Func<bool> canExecute)
            : this(execute, cancelled)
        {
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }
        
        public async Task ExecuteAsync()
        {
            if (!CanExecute())
                return;
            
            IsExecuting = true;

            _cancellationTokenSource = new CancellationTokenSource();
            
            if (Timeout.HasValue)
                _cancellationTokenSource.CancelAfter(Timeout.Value);

            try
            {
                await (_execute ?? ExecuteInternal).Invoke(_cancellationTokenSource.Token);
            }
            catch (OperationCanceledException exception)
                when (_cancellationTokenSource.IsCancellationRequested)
            {
                await (_cancelled ?? CancelledInternal).Invoke(exception);
            }
            finally
            {
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
                
                IsExecuting = false;
            }
        }
        
        public bool CanExecute()
            => !IsExecuting && (_canExecute ?? CanExecuteInternal).Invoke();

        public void Cancel() => _cancellationTokenSource?.Cancel();
        
        protected virtual Task ExecuteInternal(CancellationToken token) => Task.CompletedTask;

        protected virtual Task CancelledInternal(OperationCanceledException exception) => Task.CompletedTask;

        protected virtual bool CanExecuteInternal() => true;
        
        private protected sealed override async void ExecuteCore(object parameter)
            => await ExecuteAsync();

        private protected sealed override bool CanExecuteCore(object parameter)
            => CanExecute();
    }
}
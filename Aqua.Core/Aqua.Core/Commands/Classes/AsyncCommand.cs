using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aqua.Core.Commands
{
    public class AsyncCommand : AquaCommandBase, IAsyncCommand
    {
        private CancellationTokenSource _cancellationTokenSource;
        
        private readonly Func<CancellationToken, Task> _executeAsync;
        private readonly Func<bool> _canExecute;
        
        public TimeSpan? Timeout { get; set; }
        
        public Action<OperationCanceledException> CancelledHandler { get; set; }
        
        public Func<OperationCanceledException, Task> CancelledHandlerAsync { get; set; }

        private bool _isCancelled;
        public bool IsCancelled
        {
            get => _isCancelled;
            private set => SetProperty(ref _isCancelled, value);
        }
        
        protected AsyncCommand() { }
        
        public AsyncCommand(Func<CancellationToken, Task> executeAsync)
        {
            _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
        }

        public AsyncCommand(Func<CancellationToken, Task> executeAsync, Func<bool> canExecute)
            : this(executeAsync)
        {
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }
        
        public async Task ExecuteAsync()
        {
            if (!CanExecute())
                return;

            IsExecuting = true;
            IsCancelled = false;

            _cancellationTokenSource = new CancellationTokenSource();
            
            if (Timeout.HasValue)
                _cancellationTokenSource.CancelAfter(Timeout.Value);

            try
            {
                await (_executeAsync ?? ExecuteAsyncInternal).Invoke(_cancellationTokenSource.Token);
            }
            catch (OperationCanceledException exception)
                when (_cancellationTokenSource.IsCancellationRequested)
            {
                IsCancelled = true;
                (CancelledHandler ?? CancelledHandlerInternal).Invoke(exception);
                await (CancelledHandlerAsync ?? CancelledHandlerAsyncInternal).Invoke(exception);
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
        
        protected virtual Task ExecuteAsyncInternal(CancellationToken token) => Task.CompletedTask;

        protected virtual void CancelledHandlerInternal(OperationCanceledException exception) { }
        
        protected virtual Task CancelledHandlerAsyncInternal(OperationCanceledException exception) => Task.CompletedTask;

        protected virtual bool CanExecuteInternal() => true;
        
        private protected sealed override async void ExecuteCore(object parameter)
            => await ExecuteAsync();

        private protected sealed override bool CanExecuteCore(object parameter)
            => CanExecute();
        
        private protected sealed override bool CanExecuteFunc(object parameter)
            => (_canExecute ?? CanExecuteInternal).Invoke();
    }
}
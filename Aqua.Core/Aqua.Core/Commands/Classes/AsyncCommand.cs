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
        
        public Action<Exception> FaultedHandler { get; set; }
        
        public Func<OperationCanceledException, Task> CancelledHandlerAsync { get; set; }
        
        public Func<Exception, Task> FaultedHandlerAsync { get; set; }

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
            IsFaulted = false;

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
                if (CancelledHandler != null || CancelledHandlerAsync != null)
                {
                    CancelledHandler?.Invoke(exception);
                    await (CancelledHandlerAsync?.Invoke(exception) ?? Task.CompletedTask);
                }
                else
                {
                    throw;
                }
            }
            catch (Exception exception)
            {
                IsFaulted = true;
                if (FaultedHandler != null || FaultedHandlerAsync != null)
                {
                    FaultedHandler?.Invoke(exception);
                    await (FaultedHandlerAsync?.Invoke(exception) ?? Task.CompletedTask);
                }
                else
                {
                    throw;
                }
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

        protected virtual bool CanExecuteInternal() => true;
        
        private protected sealed override async void ExecuteCore(object parameter)
            => await ExecuteAsync();

        private protected sealed override bool CanExecuteCore(object parameter)
            => CanExecute();
        
        private protected sealed override bool CanExecuteFunc(object parameter)
            => (_canExecute ?? CanExecuteInternal).Invoke();
    }
}
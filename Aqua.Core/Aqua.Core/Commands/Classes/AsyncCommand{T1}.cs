using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aqua.Core.Commands
{
    public class AsyncCommand<TParam> : AquaCommandBase, IAsyncCommand<TParam>
    {
        private CancellationTokenSource _cancellationTokenSource;
        
        private readonly Func<TParam, CancellationToken, Task> _executeAsync;
        private readonly Func<TParam, bool> _canExecute;
        
        public TimeSpan? Timeout { get; set; }
        
        public Action<TParam, OperationCanceledException> CancelledHandler { get; set; }
        
        public Action<TParam, Exception> FaultedHandler { get; set; }
        
        public Func<TParam, OperationCanceledException, Task> CancelledHandlerAsync { get; set; }
        
        public Func<TParam, Exception, Task> FaultedHandlerAsync { get; set; }
        
        private bool _isCancelled;
        public bool IsCancelled
        {
            get => _isCancelled;
            private set => SetProperty(ref _isCancelled, value);
        }
        
        protected AsyncCommand() { }
        
        public AsyncCommand(Func<TParam, CancellationToken, Task> executeAsync)
        {
            _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
        }

        public AsyncCommand(Func<TParam, CancellationToken, Task> executeAsync, Func<TParam, bool> canExecute)
            : this(executeAsync)
        {
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        public async Task ExecuteAsync(TParam parameter)
        {
            if (!CanExecute(parameter))
                return;
            
            IsExecuting = true;
            IsCancelled = false;
            IsFaulted = false;

            _cancellationTokenSource = new CancellationTokenSource();
            
            if (Timeout.HasValue)
                _cancellationTokenSource.CancelAfter(Timeout.Value);

            try
            {
                await (_executeAsync ?? ExecuteAsyncInternal).Invoke(parameter, _cancellationTokenSource.Token);
            }
            catch (OperationCanceledException exception)
                when (_cancellationTokenSource.IsCancellationRequested)
            {
                IsCancelled = true;
                if (CancelledHandler != null || CancelledHandlerAsync != null)
                {
                    CancelledHandler?.Invoke(parameter, exception);
                    await (CancelledHandlerAsync?.Invoke(parameter, exception) ?? Task.CompletedTask);
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
                    FaultedHandler?.Invoke(parameter, exception);
                    await (FaultedHandlerAsync?.Invoke(parameter, exception) ?? Task.CompletedTask);
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
        
        public bool CanExecute(TParam parameter)
            => !IsExecuting && (_canExecute ?? CanExecuteInternal).Invoke(parameter);
        
        public void Cancel() => _cancellationTokenSource?.Cancel();
        
        protected virtual Task ExecuteAsyncInternal(TParam parameter, CancellationToken token) => Task.CompletedTask;
        
        protected virtual bool CanExecuteInternal(TParam parameter) => true;

        private protected sealed override async void ExecuteCore(object parameter)
        {
            if (IsValidParameter<TParam>(parameter))
                await ExecuteAsync((TParam)parameter);
        }

        private protected sealed override bool CanExecuteCore(object parameter)
            => IsValidParameter<TParam>(parameter) && CanExecute((TParam)parameter);
        
        private protected sealed override bool CanExecuteFunc(object parameter)
            => IsValidParameter<TParam>(parameter) && (_canExecute ?? CanExecuteInternal).Invoke((TParam)parameter);
    }
}
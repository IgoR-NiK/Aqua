namespace Aqua.Core.Commands
{
    public interface IAsyncCancelledCommand<in T> : IAsyncCancelledCommandBase, IAsyncCommand<T>
    {
    }
}
using System.Threading.Tasks;

namespace Aqua.Core.Commands
{
    public interface IAsyncCommand<in T> : IAquaCommandBase
    {
        Task ExecuteAsync(T parameter);

        bool CanExecute(T parameter);
    }
}
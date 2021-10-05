using System.Threading.Tasks;

namespace Aqua.Core.Commands
{
    public interface IAsyncCommand : IAquaCommandBase
    {
        Task ExecuteAsync();

        bool CanExecute();
    }
}
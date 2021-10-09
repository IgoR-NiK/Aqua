using Aqua.Core.Utils;

namespace Aqua.Core.Commands
{
    public interface IAquaCommand : IAquaCommandBase, IWithCanExecute
    {
        void Execute();
    }
}
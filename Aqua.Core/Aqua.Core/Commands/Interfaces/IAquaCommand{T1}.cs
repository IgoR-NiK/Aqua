using Aqua.Core.Utils;

namespace Aqua.Core.Commands
{
    public interface IAquaCommand<TParam> : IAquaCommandBase, IWithCanExecute<TParam>, IWithFaultedHandler<TParam>
    {
        void Execute(TParam parameter);
    }
}
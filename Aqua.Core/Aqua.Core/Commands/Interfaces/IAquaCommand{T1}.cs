using Aqua.Core.Utils;

namespace Aqua.Core.Commands
{
    public interface IAquaCommand<in TParam> : IAquaCommandBase, IWithCanExecute<TParam>
    {
        void Execute(TParam parameter);
    }
}
namespace Aqua.Core.Commands
{
    public interface IAquaCommand : IAquaCommandBase
    {
        void Execute();

        bool CanExecute();
    }
}
namespace Aqua.Core.Commands
{
    public interface IAquaCommand<in T> : IAquaCommandBase
    {
        void Execute(T parameter);

        bool CanExecute(T parameter);
    }
}
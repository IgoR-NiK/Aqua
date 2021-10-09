namespace Aqua.Core.Utils
{
    public interface IWithCanExecute<in TParam>
    {
        bool CanExecute(TParam parameter);
    }
}
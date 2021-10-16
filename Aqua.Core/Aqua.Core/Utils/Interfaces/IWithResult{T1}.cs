namespace Aqua.Core.Utils
{
    public interface IWithResult<out TResult>
    {
        TResult Result { get; }
    }
}
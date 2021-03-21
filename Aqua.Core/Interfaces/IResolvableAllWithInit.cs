namespace Aqua.Core.Interfaces
{
    public interface IResolvableAll<in TParam> : IResolvableAll, IWithInit<TParam>
    {
    }
}
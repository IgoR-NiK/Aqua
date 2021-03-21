namespace Aqua.Core.Interfaces
{
    public interface IResolvable<in TParam> : IResolvable, IWithInit<TParam>
    {
    }
}
namespace Aqua.Core.Interfaces
{
    public interface IResolvableFactory<out TService, in TParam> : IResolvable
        where TService : IResolvable<TParam>
    {
        TService Create(TParam param);
    }
}
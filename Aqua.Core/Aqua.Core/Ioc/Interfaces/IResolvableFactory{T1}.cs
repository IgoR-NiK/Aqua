namespace Aqua.Core.Ioc
{
    public interface IResolvableFactory<out TService> : IResolvable
        where TService : IResolvable
    {
        TService Create();
    }
}
namespace Aqua.Core.IoC
{
    public interface IResolvableFactory<out TService> : IResolvable
        where TService : IResolvable
    {
        TService Create();
    }
}
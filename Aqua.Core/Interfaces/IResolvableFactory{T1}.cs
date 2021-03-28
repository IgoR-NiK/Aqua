namespace Aqua.Core.Interfaces
{
    public interface IResolvableFactory<out TService> : IResolvable
        where TService : IResolvable
    {
        TService Create();
    }
}
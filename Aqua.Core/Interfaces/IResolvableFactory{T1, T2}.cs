namespace Aqua.Core.Interfaces
{
    public interface IResolvableFactory<in TFactoryParam, out TService> : IResolvable
        where TService : IResolvable
    {
        TService Create(TFactoryParam factoryParam);
    }
}
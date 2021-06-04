namespace Aqua.Core.IoC
{
    public interface IResolvableFactory<in TFactoryParam, out TService> : IResolvable
        where TService : IResolvable
    {
        TService Create(TFactoryParam factoryParam);
    }
}
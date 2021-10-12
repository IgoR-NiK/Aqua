namespace Aqua.Core.Ioc
{
    public interface IDecorator<out TService>
        where TService : IResolvable
    {
        TService Decoratee { get; }
    }
}
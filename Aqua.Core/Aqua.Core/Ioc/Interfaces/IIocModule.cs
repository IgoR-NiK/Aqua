using DryIoc;

namespace Aqua.Core.Ioc
{
    public interface IIocModule
    {
        void RegisterTypes(IRegistrator registrator);
    }
}
using Aqua.Core.Ioc;
using DryIoc;

namespace Aqua.Wpf.Core
{
    public class AquaWpfCoreIocModule : IIocModule
    {
        public void RegisterTypes(IRegistrator registrator)
        {
            registrator.Register<AquaWindow>();
        }
    }
}
using Aqua.Core.Ioc;
using Aqua.Core.Utils;
using DryIoc;

namespace Aqua.Core
{
    public sealed class AquaBootstrapper
    {
        private IContainer Container { get; } = new Container();

        public TApp GetApp<TApp>(params IIocModule[] modules) 
            where TApp : IAquaApplication
        {
            Container.RegisterModules(modules);
            return Container.Resolve<TApp>();
        }
    }
}
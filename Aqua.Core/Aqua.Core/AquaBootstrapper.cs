using Aqua.Core.Ioc;
using Aqua.Core.Utils;
using DryIoc;

namespace Aqua.Core
{
    public static class AquaBootstrapper
    {
        private static readonly IContainer Container = new Container();

        public static TApp GetApp<TApp>(params IIocModule[] modules) 
            where TApp : IAquaApplication
        {
            Container.RegisterModules(modules);
            return Container.Resolve<TApp>();
        }
    }
}
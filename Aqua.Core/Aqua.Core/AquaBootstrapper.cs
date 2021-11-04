using Aqua.Core.Ioc;
using Aqua.Core.Utils;
using DryIoc;

namespace Aqua.Core
{
    public static class AquaBootstrapper
    {
        public static TApp GetApp<TApp>(params IIocModule[] modules)
            where TApp : IAquaApplication
            => new Container()
                .RegisterModules(modules)
                .Resolve<TApp>();
    }
}
using System;
using DryIoc;

namespace Aqua.Core.Services
{
    public class CodeConfigProvider : IConfigProvider
    {
        private IResolver Resolver { get; }

        public CodeConfigProvider(IResolver resolver)
        {
            Resolver = resolver;
        }
        
        public bool IsSuitableFor<TConfig>() where TConfig : class, IConfig, new()
            => Attribute.IsDefined(typeof(TConfig), typeof(CodeConfigAttribute));

        public TConfig Get<TConfig>() where TConfig : class, IConfig, new()
            => Resolver.Resolve<TConfig>();
    }
}
using System;

using Aqua.XamarinForms.Services.Navigation.Classes;

using Autofac;

namespace Aqua.XamarinForms.Autofac.Services.Navigation.Classes
{
    public class AutofacResolver : DefaultResolver
    {
        private readonly ILifetimeScope _lifetimeScope;
        
        public AutofacResolver(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }
        
        public override T Resolve<T>() where T : class
        {
            return Resolve(typeof(T)) as T;
        }
        
        public override object Resolve(Type type)
        {
            if (type.IsValueType)
                throw new InvalidOperationException($"{type.Name} must be a class");
            
            return _lifetimeScope.Resolve(type);
        }
    }
}
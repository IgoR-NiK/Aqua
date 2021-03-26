using System;

using Aqua.XamarinForms.Services.Navigation.Classes;

namespace Aqua.XamarinForms.Autofac.Services.Navigation.Classes
{
    public class AutofacResolver : DefaultResolver
    {
        public override T Resolve<T>() where T : class
        {
            return Resolve(typeof(T)) as T;
        }
        
        public override object Resolve(Type type)
        {
            if (type.IsValueType)
                throw new InvalidOperationException($"{type.Name} must be a class");
            
            return Activator.CreateInstance(type);
        }
    }
}
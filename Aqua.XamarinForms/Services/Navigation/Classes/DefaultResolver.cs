using System;

using Aqua.XamarinForms.Services.Navigation.Interfaces;

namespace Aqua.XamarinForms.Services.Navigation.Classes
{
    public class DefaultResolver : IResolver
    {
        public virtual T Resolve<T>() where T : class
        {
            return Resolve(typeof(T)) as T;
        }

        public virtual object Resolve(Type type)
        {
            if (type.GetConstructor(Type.EmptyTypes) == null)
                throw new InvalidOperationException($"{type.Name} must have a parameterless constructor");
            
            if (type.IsValueType)
                throw new InvalidOperationException($"{type.Name} must be a class");
            
            return Activator.CreateInstance(type);
        }
    }
}
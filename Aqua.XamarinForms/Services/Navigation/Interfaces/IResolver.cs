using System;

using Aqua.Core.Interfaces;

namespace Aqua.XamarinForms.Services.Navigation.Interfaces
{
    public interface IResolver : IResolvable
    {
        T Resolve<T>() where T : class;
        
        object Resolve(Type type);
    }
}
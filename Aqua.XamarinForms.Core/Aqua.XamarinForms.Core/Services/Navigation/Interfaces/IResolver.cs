using System;

using Aqua.Core.IoC;

namespace Aqua.XamarinForms.Core.Services.Navigation
{
    [AsSingleInstance]
    public interface IResolver : IResolvable
    {
        T Resolve<T>() where T : class;
        
        object Resolve(Type type);
    }
}
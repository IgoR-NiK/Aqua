﻿namespace Aqua.Core.Interfaces
{
    public interface IResolvableFactory<in TFactoryParam, in TServiceParam, out TService> : IResolvable
        where TService : IResolvable<TServiceParam>
    {
        TService Create(TFactoryParam factoryParam, TServiceParam serviceParam);
    }
}
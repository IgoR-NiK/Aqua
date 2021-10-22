﻿namespace Aqua.Core.Ioc
{
    public class Decorator<TService> : IDecorator<TService>
        where TService : IResolvable
    {
        public TService Decoratee { get; }

        public Decorator(TService decoratee)
        {
            Decoratee = decoratee;
        }
    }
}
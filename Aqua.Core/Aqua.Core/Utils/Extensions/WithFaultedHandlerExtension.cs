using System;

namespace Aqua.Core.Utils
{
    public static class WithFaultedHandlerExtension
    {
        public static T WithFaultedHandler<T>(this T @object, Action<Exception> faultedHandler)
            where T : IWithFaultedHandler
        {
            @object.FaultedHandler = faultedHandler;
            return @object;
        }

        public static T WithFaultedHandler<T, TParam>(this T @object, Action<TParam, Exception> faultedHandler)
            where T : IWithFaultedHandler<TParam>
        {
            @object.FaultedHandler = faultedHandler;
            return @object;
        }
    }
}
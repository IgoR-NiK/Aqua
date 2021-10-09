using System;

namespace Aqua.Core.Utils
{
    public static class WithTimeoutExtension
    {
        public static T WithTimeout<T>(this T @object, TimeSpan timeout) 
            where T : IWithTimeout
        {
            @object.Timeout = timeout;
            return @object;
        }
        
        public static T WithTimeout<T>(this T @object, double timeInMilliseconds) 
            where T : IWithTimeout
            => @object.WithTimeout(TimeSpan.FromMilliseconds(timeInMilliseconds));
    }
}
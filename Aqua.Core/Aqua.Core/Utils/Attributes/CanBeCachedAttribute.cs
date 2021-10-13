using System;

namespace Aqua.Core.Utils
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class CanBeCachedAttribute : Attribute
    {
    }
}
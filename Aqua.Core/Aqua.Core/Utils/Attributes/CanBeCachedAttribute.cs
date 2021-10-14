using System;

namespace Aqua.Core.Utils
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class CanBeCachedAttribute : Attribute
    {
    }
}
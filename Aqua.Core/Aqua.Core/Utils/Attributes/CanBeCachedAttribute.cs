using System;

namespace Aqua.Core.Utils
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CanBeCachedAttribute : Attribute
    {
    }
}
using System;

namespace Aqua.Core.Services
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class JsonConfigAttribute : Attribute
    {
    }
}
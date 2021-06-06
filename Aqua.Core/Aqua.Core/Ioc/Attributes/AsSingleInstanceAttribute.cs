using System;

namespace Aqua.Core.Ioc
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class AsSingleInstanceAttribute : Attribute
    {
    }
}
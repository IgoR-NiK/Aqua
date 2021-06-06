using System;

namespace Aqua.Core.Ioc
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class ManualRegistrationAttribute : Attribute
    {
    }
}
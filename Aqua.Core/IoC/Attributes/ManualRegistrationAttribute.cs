using System;

namespace Aqua.Core.IoC
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class ManualRegistrationAttribute : Attribute
    {
    }
}
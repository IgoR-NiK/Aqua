﻿using System;

namespace Aqua.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class ManualRegistrationAttribute : Attribute
    {
    }
}
using System;

namespace Aqua.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class OrderAttribute : Attribute
    {
        public int Value { get; }
        
        public OrderAttribute(int value)
        {
            Value = value;
        }
    }
}
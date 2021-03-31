using System;

namespace Aqua.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class OrderAttribute : Attribute
    {
        public int Weight { get; }
        
        public OrderAttribute(int weight)
        {
            Weight = weight;
        }
    }
}
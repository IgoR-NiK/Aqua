using System;

namespace Aqua.Core.Utils
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
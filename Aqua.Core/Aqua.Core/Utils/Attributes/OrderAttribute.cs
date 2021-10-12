using System;

namespace Aqua.Core.Utils
{
    /// <summary>
    /// Сортировка по убыванию по Weight
    /// У первого в списке большее значение Weight
    /// </summary>
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
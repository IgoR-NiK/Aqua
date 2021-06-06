using System;

namespace Aqua.Core.Utils
{
    /// <summary>
    /// Сортировка по возрастанию по Value
    /// </summary>
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
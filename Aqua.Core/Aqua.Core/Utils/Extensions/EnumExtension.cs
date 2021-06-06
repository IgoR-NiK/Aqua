using System;
using System.ComponentModel;
using System.Linq;

namespace Aqua.Core.Utils
{
    public static class EnumExtension
    {
        public static string GetDescription(this Enum value)
        {
            var attribute = value
                .GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() as DescriptionAttribute;

            return attribute?.Description ?? value.ToString();
        }
    }
}
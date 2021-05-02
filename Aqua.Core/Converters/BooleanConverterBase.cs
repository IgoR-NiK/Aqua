using System;
using System.Collections.Generic;
using System.Globalization;

namespace Aqua.Core.Converters
{
    public abstract class BooleanConverterBase<T>
    {
        public T True { get; set; }
        
        public T False { get; set; }
        
        public BooleanConverterBase(T trueValue, T falseValue)
        {
            True = trueValue;
            False = falseValue;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                null => throw new ArgumentNullException(nameof(value)),
                bool boolean => boolean ? True : False,
                _ => throw new InvalidOperationException($"The {nameof(value)} must be a {nameof(Boolean)}")
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                null => throw new ArgumentNullException(nameof(value)),
                T tValue => EqualityComparer<T>.Default.Equals(tValue, True),
                _ => throw new InvalidOperationException($"The {nameof(value)} must be a {typeof(T).Name}")
            };
        }
    }
}
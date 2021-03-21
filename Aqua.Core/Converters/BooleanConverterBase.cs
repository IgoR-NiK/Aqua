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
            switch (value)
            {
                case null: 
                    throw new ArgumentNullException(nameof(value));
                case bool boolean:
                    return boolean ? True : False;
                default:
                    throw new InvalidOperationException($"The {nameof(value)} must be a {nameof(Boolean)}");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case null:
                    throw new ArgumentNullException(nameof(value));
                case T tValue:
                    return EqualityComparer<T>.Default.Equals(tValue, True);
                default:
                    throw new InvalidOperationException($"The {nameof(value)} must be a {typeof(T).Name}");
            }
        }
    }
}
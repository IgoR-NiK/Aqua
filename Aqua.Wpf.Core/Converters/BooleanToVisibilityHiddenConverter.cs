using System.Windows;
using System.Windows.Data;
using Aqua.Core.Converters;

namespace Aqua.Wpf.Core.Converters
{
    public class BooleanToVisibilityHiddenConverter : BooleanConverterBase<Visibility>, IValueConverter
    {
        public BooleanToVisibilityHiddenConverter()
            : base(Visibility.Visible, Visibility.Hidden) { }
    }
}
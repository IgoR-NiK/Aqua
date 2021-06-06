using System.Windows;
using System.Windows.Data;
using Aqua.Core.Converters;

namespace Aqua.Wpf.Core.Converters
{
    public class InverseBooleanToVisibilityHiddenConverter : BooleanConverterBase<Visibility>, IValueConverter
    {
        public InverseBooleanToVisibilityHiddenConverter()
            : base(Visibility.Hidden, Visibility.Visible) { }
    }
}
using System.Windows;
using System.Windows.Data;
using Aqua.Core.Converters;

namespace Aqua.Wpf.Core.Converters
{
    public class InverseBooleanToVisibilityCollapsedConverter : BooleanConverterBase<Visibility>, IValueConverter
    {
        public InverseBooleanToVisibilityCollapsedConverter()
            : base(Visibility.Collapsed, Visibility.Visible) { }
    }
}
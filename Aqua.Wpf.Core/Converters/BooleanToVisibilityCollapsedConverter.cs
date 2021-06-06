using System.Windows;
using System.Windows.Data;
using Aqua.Core.Converters;

namespace Aqua.Wpf.Core.Converters
{
    public class BooleanToVisibilityCollapsedConverter : BooleanConverterBase<Visibility>, IValueConverter
    {
        public BooleanToVisibilityCollapsedConverter()
            : base(Visibility.Visible, Visibility.Collapsed) { }
    }
}
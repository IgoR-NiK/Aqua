using System.Windows.Data;
using Aqua.Core.Converters;

namespace Aqua.Wpf.Core.Converters
{
    public sealed class InverseBooleanConverter : BooleanConverterBase<bool>, IValueConverter
    {
        public InverseBooleanConverter()
            : base(false, true) { }
    }
}
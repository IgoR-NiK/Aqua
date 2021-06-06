using Aqua.Core.Converters;
using Xamarin.Forms;

namespace Aqua.XamarinForms.Core.Converters
{
    public sealed class InverseBooleanConverter : BooleanConverterBase<bool>, IValueConverter
    {
        public InverseBooleanConverter()
            : base(false, true) { }
    }
}
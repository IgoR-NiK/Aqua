using Aqua.Core.Converters;

using Xamarin.Forms;

namespace Aqua.XamarinForms.Converters
{
    public sealed class InverseBooleanConverter : BooleanConverterBase<bool>, IValueConverter
    {
        public InverseBooleanConverter()
            : base(false, true) { }
    }
}
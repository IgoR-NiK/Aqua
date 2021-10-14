using System;

namespace Aqua.XamarinForms.Core.Services
{
    internal sealed class CanNavigateNow : IDisposable
    {
        private volatile bool _value;

        public bool Value
        {
            get => _value;
            set => _value = value;
        }

        public CanNavigateNow(bool value)
        {
            Value = value;
        }

        public void Dispose()
        {
            Value = true;
        }
    }
}
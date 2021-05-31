using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Aqua.Core.Utils
{
    public abstract class RaisableObject : INotifyPropertyChanged
    {
        protected void SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            SetProperty(ref property, value, null, propertyName);
        }

        protected void SetProperty<T>(ref T property, T value, Action<T> onValueChanged, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(property, value))
            {
                property = value;
                onValueChanged?.Invoke(property);
                RaisePropertyChanged(propertyName);
            }
        }
		
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
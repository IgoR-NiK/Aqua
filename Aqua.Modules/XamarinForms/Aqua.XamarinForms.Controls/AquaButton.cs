using System.Windows.Input;
using Aqua.Core.Commands;
using Xamarin.Forms;

namespace Aqua.XamarinForms.Controls
{
    public class AquaButton : Button
    {
        public const string IsExecutingVisualState = "IsExecuting";
        public const string IsExecutingDisabledVisualState = "IsExecutingDisabled";

        public new static readonly BindableProperty CommandProperty = BindableProperty.Create(
            nameof(Command),
            typeof(ICommand),
            typeof(AquaButton),
            null,
            propertyChanging: OnCommandChanging,
            propertyChanged: OnCommandChanged);

        public new static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
            nameof(CommandParameter),
            typeof(object),
            typeof(AquaButton),
            null,
            propertyChanged: OnCommandParameterChanged);

        public new ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }
        
        public new object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        private static void OnCommandChanging(BindableObject bindableObject, object oldValue, object newValue)
        {
            var aquaButton = (AquaButton)bindableObject;
            
            if (oldValue is IAquaCommand aquaCommand)
            {
                aquaCommand.IsExecutingChanged -= aquaButton.OnCommandIsExecutingChanged;
                aquaCommand.CheckCanExecuteFunc -= aquaButton.OnCommandCheckCanExecuteFunc;
            }
        }
        
        private static void OnCommandChanged(BindableObject bindableObject, object oldValue, object newValue)
        {
            var aquaButton = (AquaButton)bindableObject;
            
            if (newValue is ICommand newCommand)
            {
                ((Button)bindableObject).Command = newCommand;
            }

            if (newValue is IAquaCommand aquaCommand)
            {
                aquaCommand.IsExecutingChanged += aquaButton.OnCommandIsExecutingChanged;
                aquaCommand.CheckCanExecuteFunc += aquaButton.OnCommandCheckCanExecuteFunc;
            }
        }
        
        private static void OnCommandParameterChanged(BindableObject bindableObject, object oldValue, object newValue)
        {
            ((Button)bindableObject).CommandParameter = newValue;
            ((AquaButton)bindableObject).ChangeVisualState();
        }

        private void OnCommandIsExecutingChanged(bool newValue)
        {
            ChangeVisualState();
        }

        private void OnCommandCheckCanExecuteFunc()
        {
            ChangeVisualState();
        }

        protected override void ChangeVisualState()
        {
            if (!(Command is IAquaCommand aquaCommand
                  && aquaCommand.IsExecuting))
            {
                base.ChangeVisualState();
                return;
            }

            if (aquaCommand.CanExecuteFunc(CommandParameter))
            {
                var _ = VisualStateManager.GoToState(this, IsExecutingVisualState)
                        || VisualStateManager.GoToState(this, VisualStateManager.CommonStates.Normal);
                return;
            }

            var __ = VisualStateManager.GoToState(this, IsExecutingDisabledVisualState)
                    || VisualStateManager.GoToState(this, VisualStateManager.CommonStates.Disabled);
        }
    }
}
using Aqua.Core.Commands;
using Xamarin.Forms;

namespace Aqua.XamarinForms.Controls
{
    public class AquaAsyncButton : Button
    {
        public const string IsExecutingVisualState = "IsExecuting";
        
        protected override void ChangeVisualState()
        {
            if (!(Command is IAsyncCommand asyncCommand
                  && asyncCommand.IsExecuting
                  && VisualStateManager.GoToState(this, IsExecutingVisualState)))
            {
                base.ChangeVisualState();
            }
        }
    }
}
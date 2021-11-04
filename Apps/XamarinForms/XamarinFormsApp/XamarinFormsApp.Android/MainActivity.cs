using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Aqua.Core;
using Aqua.XamarinForms.Core;
using Aqua.XamarinForms.Popup;
using Rg.Plugins.Popup;

namespace XamarinFormsApp.Android
{
    [Activity(Label = "XamarinFormsApp", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            Popup.Init(this);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(AquaBootstrapper.GetApp<App>(
                new AquaCoreIocModule(), 
                new AquaXamarinFormsCoreIocModule(),
                new AquaXamarinFormsPopupIocModule(),
                new AppIocModule()));
        }
    }
}
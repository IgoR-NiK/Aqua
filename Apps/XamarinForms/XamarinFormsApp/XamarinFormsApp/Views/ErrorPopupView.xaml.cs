﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aqua.Core.Ioc;
using Aqua.Core.Mvvm;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinFormsApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ErrorPopupView : PopupPage, IView
    {
        public ErrorPopupView()
        {
            InitializeComponent();
        }
    }
}
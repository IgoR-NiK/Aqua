﻿using Aqua.XamarinForms.Core.Services;
using XamarinFormsApp.ViewModels;
using XamarinFormsApp.Views;

namespace XamarinFormsApp
{
    public class AppNavigationModule : INavigationModule
    {
        public void Map(INavigationMapper navigationMapper)
        {
            navigationMapper
                .Map<ErrorPopupViewModel, ErrorPopupView>()
                .Map<ExceptionViewModel, ExceptionView>()
                .Map<LoginViewModel, LoginView>()
                .Map<LoginTwoViewModel, LoginTwoView>()
                .Map<MainViewModel, MainView>()
                .Map<SecondViewModel, SecondView>();
        }
    }
}
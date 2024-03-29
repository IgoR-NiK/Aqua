﻿using Aqua.Core.Commands;
using Aqua.Core.Mvvm;
using Aqua.Core.Services;
using Aqua.XamarinForms.Core.Services;
using XamarinFormsApp.Configs;

namespace XamarinFormsApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _text;
        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }
        
        public AsyncCommand GoToSecondCommand { get; }
        
        public AquaCommand ReadConfig { get; }
        
        private IConfigProvider<TestConfig> ConfigProvider { get; }

        public MainViewModel(INavigationService navigationService, IConfigProvider<TestConfig> configProvider)
        {
            ConfigProvider = configProvider;
            
            GoToSecondCommand = new AsyncCommand(_ => navigationService.NavigateToAsync<SecondViewModel, string>(Text));
            ReadConfig = new AquaCommand(() =>
            {
                var testConfig = ConfigProvider.Get();
                Text = testConfig.Name;
            });
        }
    }
}
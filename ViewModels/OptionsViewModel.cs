using System;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;

namespace SIUGJ.ViewModels
{
	public class OptionsViewModel
	{
        public Command LoginCommand { get; }
        public Command RegisterCommand { get; }

        public OptionsViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            RegisterCommand = new Command(OnRegisterClicked);
        }

        private void OnLoginClicked(object obj)
        {
            Application.Current.MainPage.Navigation.PushModalAsync(new Views.LoginPage());
        }

        private void OnRegisterClicked(object obj)
        {
            Launcher.OpenAsync(new Uri(Helpers.Settings.SIUGJWebHomeUrl));
        }
    }
}

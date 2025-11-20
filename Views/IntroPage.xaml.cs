using System;
using System.Collections.Generic;
using SIUGJ.Helpers;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace SIUGJ.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IntroPage : ContentPage
    {
        public IntroPage()
        {
            InitializeComponent();
            var versionService = ServiceHelper.GetRequiredService<IAppVersionAndBuild>();
            AppVersion.Text = $"Version {versionService.GetVersionNumber()}.{versionService.GetBuildNumber()}";
        }
    }
}

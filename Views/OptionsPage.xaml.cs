using System;
using System.Collections.Generic;
using SIUGJ.ViewModels;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace SIUGJ.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OptionsPage : ContentPage
	{	
		public OptionsPage ()
		{
			InitializeComponent ();
            this.BindingContext = new OptionsViewModel();
        }
	}
}


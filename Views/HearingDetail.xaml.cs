using System;
using System.Collections.Generic;
using SIUGJ.Models;
using SIUGJ.ViewModels;
using Microsoft.Maui.Controls;

namespace SIUGJ.Views
{	
	public partial class HearingDetail : ContentPage
	{	
		
		public HearingDetail()
		{
			InitializeComponent();
			BindingContext = new HearingDetailViewModel();
		}
		
		public HearingDetail(Hearing hearing)
		{
			InitializeComponent();

			var viewModel = new HearingDetailViewModel
			{
				HearingDetail = hearing
			};
			
			BindingContext = viewModel;
        }
	}
}


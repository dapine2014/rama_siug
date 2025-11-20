using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

using SIUGJ.Models;
using SIUGJ.Views;
using SIUGJ.ViewModels;

namespace SIUGJ.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationsPage : ContentPage
	{
        NotificationsViewModel _viewModel;
        Page _page;

        public NotificationsPage ()
		{
			InitializeComponent();

            BindingContext = _viewModel = new NotificationsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _page = (Shell.Current?.CurrentItem?.CurrentItem as IShellSectionController)?.PresentedPage;

            switch (_page.Parent.ClassId)
            {
                case "0":
                    emptyLabel.Text = "Aún no tiene alertas pendientes";
                    break;
                case "1":
                    emptyLabel.Text = "No tiene alertas leidas";
                    break;
                case "2":
                    emptyLabel.Text = "Aún no tiene alertas";
                    break;
            }

            _viewModel.OnAppearing();
        }

    }
}

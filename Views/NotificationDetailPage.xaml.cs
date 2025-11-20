using SIUGJ.ViewModels;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace SIUGJ.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationDetailPage : ContentPage
    {
        public NotificationDetailPage()
        {
            InitializeComponent();
            BindingContext = new NotificationDetailViewModel();
        }
    }
}


using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using SIUGJ.ViewModels;

namespace SIUGJ.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Hearings : ContentPage
    {
        public Hearings()
        {
            InitializeComponent();
            BindingContext = new HearingViewModel();
        }
    }
}

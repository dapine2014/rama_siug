using System.ComponentModel;
using Microsoft.Maui.Controls;
using SIUGJ.ViewModels;

namespace SIUGJ.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}

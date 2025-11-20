using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using SIUGJ.Helpers;

namespace SIUGJ.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            InitDataLoad();
        }

        private void InitDataLoad()
        {
            var auth = App.dataBase.GetAuth();
            welcome.Text = "Hola, " + auth.userName;
        }

        public async void OnTapped(object sender, EventArgs args)
        {
            var stackLayoutSender = (StackLayout)sender;
            string urlToCheck = Constants.Actions;
            
            bool isAvailable = await IsWebsiteAvailable(urlToCheck);
            if (isAvailable) {
                switch (stackLayoutSender.ClassId)
                {
                    case "notifications":
                        await Shell.Current.GoToAsync($"//notifications");
                        break;
                    case "headquarters":
                        await Shell.Current.GoToAsync($"//headquarters");
                        break;
                    case "hearings":
                        await Shell.Current.GoToAsync($"//hearings");
                        break;
                    case "processes":
                        await Launcher.OpenAsync(new Uri(Helpers.Settings.SIUGJWebHomeUrl));
                        break;
                }
            } else {
                await Application.Current.MainPage.DisplayAlert(
                    "Servicio no disponible", 
                    "Servicio no disponible, intente más tarde.", 
                    "Aceptar");  
            }
        }
        
        private async Task<bool> IsWebsiteAvailable(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(5); // Establecer un timeout razonable
                    HttpResponseMessage response = await client.GetAsync(url);
                    return  response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al verificar disponibilidad del sitio: {ex.Message}");
                return false;
            }
        }
        
        
    }
}

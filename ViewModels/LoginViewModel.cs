using SIUGJ.Services;
using System;
using System.Net.Http;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using SIUGJ.Helpers;
using ServiceModels = SIUGJ.Models.ServiceSIUGJ;


namespace SIUGJ.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string email = string.Empty;
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }
        private string password = string.Empty;
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public Command LoginCommand { get; }
        public Command ForgetPasswordCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            ForgetPasswordCommand = new Command(OnForgetPasswordClicked);
            App.dataBase?.RemoveAuth();
        }

        private async void OnLoginClicked(object sender)
        {
            if (string.IsNullOrEmpty(Email) ||  string.IsNullOrEmpty(Password))
            {
                await ShowAlertAsync("Campos obligatorios", "Por favor verifique sus credenciales e intente nuevamente, gracias.");
            }
            else
            {
                string urlToCheck = Constants.Actions;
                bool isAvailable = await IsWebsiteAvailable(urlToCheck);
                if (isAvailable) {
                    var serviceSIUGJ = ServiceHelper.GetRequiredService<SIUGJ_Service>();
                    var response = serviceSIUGJ.AuthenticateUser(Email, Password);
                    var info = response.serviceStatus.ToString();

                    switch (response.serviceStatus)
                    {
                        case ServiceModels.DataBaseSIUGJ.EnServiceResults.Success:
                            Application.Current.MainPage = new AppShell();
                            break;
                        case ServiceModels.DataBaseSIUGJ.EnServiceResults.InvalidCredentials:
                            await ShowAlertAsync("Credenciales no válidas", "Usuario y/o contraseñas incorrectos");
                            break;
                        default:
                            await ShowAlertAsync("Credenciales no válidas", "Usuario y/o contraseñas incorrectos");
                            break;
                    
                        /*fue cambiado por que hay un error en el soap
                         * case ServiceModels.DataBaseSIUGJ.EnServiceResults.InvocationError:
                           await App.Current.MainPage.DisplayAlert("Credenciales no válidas", "Lo sentimos, algo salió mal. Reintente más tarde", "Ok");
                           break;
                         */
                    }
                }
                else
                {
                    await ShowAlertAsync(
                        "Servicio no disponible", 
                        "Servicio no disponible, intente más tarde.", 
                        "Aceptar");  
                }
            }
        }        

        private void OnForgetPasswordClicked(object obj)
        {
            Launcher.OpenAsync(new Uri(Helpers.Settings.SIUGJWebHomeUrl));
        }
        
        private async System.Threading.Tasks.Task<bool> IsWebsiteAvailable(string url)
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

        private static System.Threading.Tasks.Task ShowAlertAsync(string title, string message, string cancel = "Ok")
        {
            var page = Application.Current?.MainPage;
            return page != null ? page.DisplayAlert(title, message, cancel) : System.Threading.Tasks.Task.CompletedTask;
        }
    }
}

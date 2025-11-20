using System;
using System.Diagnostics;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using SIUGJ.Helpers;
using SIUGJ.Services;

namespace SIUGJ.ViewModels
{
    [QueryProperty(nameof(IdRegistro), nameof(IdRegistro))]
    public class NotificationDetailViewModel : BaseViewModel
    {
        public IDataStore<Models.Notification> NotificationService => ServiceHelper.GetRequiredService<IDataStore<Models.Notification>>();

        public string id = string.Empty;
        public string Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        public string tipoNotificacion = string.Empty;
        public string TipoNotificacion
        {
            get => tipoNotificacion;
            set => SetProperty(ref tipoNotificacion, value);
        }

        public string despacho = string.Empty;
        public string Despacho
        {
            get => despacho;
            set => SetProperty(ref despacho, value);
        }

        public string fechaAsignacion = string.Empty;
        public string FechaAsignacion
        {
            get => fechaAsignacion;
            set => SetProperty(ref fechaAsignacion, value);
        }

        public string codigoUnicoProceso = string.Empty;
        public string CodigoUnicoProceso
        {
            get => codigoUnicoProceso;
            set => SetProperty(ref codigoUnicoProceso, value);
        }

        public string demandante = string.Empty;
        public string Demandante
        {
            get => demandante;
            set => SetProperty(ref demandante, value);
        }

        public string demandado = string.Empty;
        public string Demandado
        {
            get => demandado;
            set => SetProperty(ref demandado, value);
        }

        public string contenidoMensaje = string.Empty;
        public string ContenidoMensaje
        {
            get => contenidoMensaje;
            set => SetProperty(ref contenidoMensaje, value);
        }

        public string idRegistro { get; set; } = string.Empty;
        public string IdRegistro
        {
            get
            {
                return idRegistro;
            }
            set
            {
                idRegistro = value;
                if (!string.IsNullOrEmpty(value))
                {
                    LoadItemId(value);
                }
            }
        }

        public Command ManageNotificationCommand { get; }

        public NotificationDetailViewModel()
        {
            ManageNotificationCommand= new Command(OnManageNotificationClicked);
        }

        public async void LoadItemId(string idRegistro)
        {
            if (string.IsNullOrEmpty(idRegistro))
            {
                return;
            }

            try
            {
                var notification = await NotificationService.GetItemAsync(idRegistro);
                if (notification == null)
                {
                    return;
                }

                TipoNotificacion = notification.tipoNotificacion;
                Despacho = notification.despacho;
                FechaAsignacion = notification.fechaAsignacion;
                CodigoUnicoProceso = notification.codigoUnicoProceso;
                Demandante = notification.demandante;
                Demandado = notification.demandado;
                ContenidoMensaje = notification.contenidoMensaje;
                Id = notification.idRegistro;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }

        private void OnManageNotificationClicked(object obj)
        {
            Launcher.OpenAsync(new Uri(Helpers.Settings.SIUGJWebHomeUrl));
        }
    }
}

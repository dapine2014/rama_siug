using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using SIUGJ.Helpers;
using SIUGJ.Models;

namespace SIUGJ.Services
{
	public class NotificationService : IDataStore<Notification>
    {
        public List<Notification> items;

        public NotificationService()
        {
            items = new List<Notification>();
        }

        public async Task<bool> AddItemAsync(Notification item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Notification item)
        {
            var oldItem = items.Where((Notification arg) => arg.idRegistro == item.idRegistro).FirstOrDefault();
            if (oldItem != null)
            {
                items.Remove(oldItem);
            }
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((Notification arg) => arg.idRegistro == id).FirstOrDefault();
            if (oldItem != null)
            {
                items.Remove(oldItem);
            }

            return await Task.FromResult(true);
        }

        public async Task<Notification> GetItemAsync(string id)
        {
            var notification = items.FirstOrDefault(s => s.idRegistro == id);
            if (notification == null)
            {
                throw new InvalidOperationException($"No se encontró la notificación {id}.");
            }

            return await Task.FromResult(notification);
        }

        public Notification GetItem(string id)
        {
            var notification = items.FirstOrDefault(s => s.idRegistro == id);
            if (notification == null)
            {
                throw new InvalidOperationException($"No se encontró la notificación {id}.");
            }

            return notification;
        }

        public async Task<IEnumerable<Notification>> GetItemsAsync(bool forceRefresh = false)
        {
            var serviceSIUGJ = ServiceHelper.GetRequiredService<SIUGJ_Service>();
            var response = serviceSIUGJ.UsersTask(0, forceRefresh);

            if (response.serviceStatus == Models.ServiceSIUGJ.DataBaseSIUGJ.EnServiceResults.InvocationError)
            {
                var mainPage = Application.Current?.MainPage;
                if (mainPage != null)
                {
                    await mainPage.DisplayAlert("Servicio no disponible", "Lo sentimos, algo salió mal. Reintente más tarde", "Ok");
                }
            }

            if (response.responseUserTask?.tasks == null)
            {
                items.Clear();
                return await Task.FromResult(new List<Notification>());
            }

            items = response.responseUserTask.tasks.Select(task => new Notification
            {
                idRegistro = task.idRegistro,
                fechaAsignacion = task.fechaAsignacion,
                fechaLimiteAtencion = task.fechaLimiteAtencion,
                tipoNotificacion = task.tipoNotificacion,
                usuarioRemitente = task.usuarioRemitente,
                usuarioDestinatario = task.usuarioDestinatario,
                contenidoMensaje = task.contenidoMensaje,
                fechaVisualizacion = task.fechaVisualizacion,
                codigoUnicoProceso = task.codigoUnicoProceso,
                despacho = task.despacho,
                folioProceso = task.folioProceso,
                demandante = task.demandante,
                demandado = task.demandado,
                taskstatus = task.taskstatus
            }).ToList();

            return await Task.FromResult(response.serviceStatus == Models.ServiceSIUGJ.DataBaseSIUGJ.EnServiceResults.Success ? items : new List<Notification>());
        }
    }
}

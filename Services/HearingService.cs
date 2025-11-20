using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using SIUGJ.Helpers;
using SIUGJ.Models;

namespace SIUGJ.Services
{
	public class HearingService : IDataStore<Hearing>
    {
        public List<Hearing> items;

        public HearingService()
        {
            items = new List<Hearing>();
        }

        public async Task<bool> AddItemAsync(Hearing item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Hearing item)
        {
            var oldItem = items.Where((Hearing arg) => arg.UniqueCode == item.UniqueCode).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((Hearing arg) => arg.UniqueCode == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Hearing> GetItemAsync(string id)
        {
            var items = await GetItemsAsync(true);
            return items.FirstOrDefault(item => item.IdEvento == id);
           // return await Task.FromResult(items.FirstOrDefault(s => s.UniqueCode == id));
        }

        public Hearing GetItem(string id)
        {
            return  items.FirstOrDefault(s => s.UniqueCode == id);
        }

        public async Task<IEnumerable<Hearing>> GetItemsAsync(bool forceRefresh = false)
        {
            var serviceSIUGJ = ServiceHelper.GetRequiredService<SIUGJ_Service>();
            var response = serviceSIUGJ.UsersAudiences(
                DateTime.Today.AddMonths(Helpers.Settings.DaysLowerLimitForHearings).ToString("yyyy-MM-dd"),
                DateTime.Today.AddYears(Helpers.Settings.DaysUpperLimitForHearings).ToString("yyyy-MM-dd"),
                0,
                forceRefresh);

            if (response.serviceStatus == Models.ServiceSIUGJ.DataBaseSIUGJ.EnServiceResults.InvocationError)
            {
                await App.Current.MainPage.DisplayAlert("Servicio no disponible", "Lo sentimos, algo salió mal. Reintente más tarde", "Ok");
            }


            if (response.responseUsersAudiences?.audiences == null)
            {
                items.Clear();
                return await Task.FromResult(new List<Hearing>());
            }

            items = response.responseUsersAudiences.audiences.Select(audience => new Hearing
            {
                Name = audience.tipoAudiencia,
                IdEvento = audience.idEvento,
                Description = audience.codigoUnicoProceso,
                Starting = DateTime.Parse(audience.fechaEvento),
                StartTime = DateTime.Parse(audience.horaInicial).ToString("HH:mm"),
                EndTime = DateTime.Parse(audience.horaFinal).ToString("HH:mm"),
                UniqueCode = audience.codigoUnicoProceso,
                IsVirtual = string.IsNullOrEmpty(audience.urlVideoConferencia) || string.IsNullOrEmpty(audience.esVirtual) || audience.esVirtual.Equals("0") ? false : true,
                Audience = audience            
            }).ToList();

            return await Task.FromResult(response.serviceStatus == Models.ServiceSIUGJ.DataBaseSIUGJ.EnServiceResults.Success ? items : new List<Hearing>());
        }
    }
}

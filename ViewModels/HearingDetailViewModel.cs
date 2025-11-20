using System;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using SIUGJ.Helpers;
using SIUGJ.Services;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace SIUGJ.ViewModels
{
    [QueryProperty(nameof(IdEvento), nameof(IdEvento))]
	public class HearingDetailViewModel : BaseViewModel
    {
        public IDataStore<Models.Hearing> HearingService => ServiceHelper.GetRequiredService<IDataStore<Models.Hearing>>();

        private string _idEvento;
        public string IdEvento
        {
            get => _idEvento;
            set
            {
                _idEvento = value;
                if (!string.IsNullOrEmpty(value))
                {
                    Debug.WriteLine($"IdEvento establecido a: '{value}'");
                    LoadHearing(value);
                }
            }
        }

        private Models.Hearing _hearingDetail;
        public Models.Hearing HearingDetail
        {
            get => _hearingDetail;
            set => SetProperty(ref _hearingDetail, value);
        }

        public Command ManageHearingCommand { get; }

        private CultureInfo _culture = CultureInfo.InvariantCulture;
        public CultureInfo Culture
        {
            get => _culture;
            set => SetProperty(ref _culture, value);
        }

        public string ErrorMessage { get; private set; }
        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

        public HearingDetailViewModel()
        {
            Culture = CultureInfo.CreateSpecificCulture("es");
            ManageHearingCommand = new Command(OnManageHearingClicked, () => HearingDetail?.IsVirtual == true);
        }

        public async void LoadHearing(string id)
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            try
            {
                Debug.WriteLine($"Cargando audiencia con ID: {id}");
                HearingDetail = await HearingService.GetItemAsync(id);
                
                var allHearings = await HearingService.GetItemsAsync(true);
                Debug.WriteLine($"Total de audiencias disponibles: {allHearings.Count()}");
                foreach (var hearing in allHearings)
                {
                    Debug.WriteLine($"Audiencia disponible - ID: '{hearing.IdEvento}', UniqueCode: '{hearing.UniqueCode}'");
                }
                
                HearingDetail = await HearingService.GetItemAsync(id);
                
                
                if (HearingDetail == null)
                {
                    ErrorMessage = "No se encontró la audiencia solicitada";
                    Debug.WriteLine("No se encontró la audiencia");
                }
                else
                {
                    Debug.WriteLine($"Audiencia cargada: {HearingDetail.Name}");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error al cargar los datos de la audiencia";
                Debug.WriteLine($"Failed to Load Item: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
                OnPropertyChanged(nameof(ErrorMessage));
                OnPropertyChanged(nameof(HasError));
                ManageHearingCommand.ChangeCanExecute();
            }
        }

        private void OnManageHearingClicked()
        {
            if (HearingDetail?.Audience?.urlVideoConferencia != null)
            {
                Debug.WriteLine($"Abriendo URL: {HearingDetail.Audience.urlVideoConferencia}");
                Launcher.OpenAsync(new Uri(HearingDetail.Audience.urlVideoConferencia));
            }
            else
            {
                Debug.WriteLine("No se puede abrir la URL: La audiencia no tiene URL de videoconferencia");
            }
        }
    }
}

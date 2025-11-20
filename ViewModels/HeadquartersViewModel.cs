using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Maps;
using SIUGJ.Helpers;
using SIUGJ.Models.ServiceSIUGJ;
using SIUGJ.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SIUGJ.ViewModels
{
	public class HeadquartersViewModel : BasePageViewModel
    {
        public List<Court> CurrentCourts { get; } = new List<Court>();

        private Microsoft.Maui.Controls.Maps.Map mapView;
        public Microsoft.Maui.Controls.Maps.Map MapView
        {
            get => mapView;
            set => SetProperty(ref mapView, value);
        }

        private int citySelectedIndex;
        public int CitySelectedIndex
        {
            get => citySelectedIndex;
            set => SetProperty(ref citySelectedIndex, value);
        }

        private int specialitySelectedIndex;
        public int SpecialitySelectedIndex
        {
            get => specialitySelectedIndex;
            set => SetProperty(ref specialitySelectedIndex, value);
        }

        private List<Models.PickerItem> cityItems;
        public List<Models.PickerItem> CityItems
        {
            get => cityItems ?? new List<Models.PickerItem>();
            set => SetProperty(ref cityItems, value);
        }

        private List<string> specialityItems;
        public List<string> SpecialityItems
        {
            get => specialityItems ?? new List<string>();
            set => SetProperty(ref specialityItems, value);
        }

        private CultureInfo _culture = CultureInfo.InvariantCulture;
        public CultureInfo Culture
        {
            get => _culture;
            set => SetProperty(ref _culture, value);
        }

        public HeadquartersViewModel()
        {
            Culture = CultureInfo.CreateSpecificCulture("es");

            CitySelectedIndex = -1;
            SpecialitySelectedIndex = -1;

            var serviceSIUGJ = ServiceHelper.GetRequiredService<SIUGJ_Service>();
            var response = serviceSIUGJ.GetCities(true);

            switch (response.serviceStatus)
            {
                case DataBaseSIUGJ.EnServiceResults.Success:
                    CityItems = new List<Models.PickerItem>();
                    if (response.responseGetCities.cities != null)
                    {
                        foreach (var city in response.responseGetCities.cities)
                        {
                            CityItems.Add(new Models.PickerItem { Label = string.Format(Culture, "{0}", city.nombreCiudad), Value = city.codigoCiudad });
                        }
                    }
                    break;
                case DataBaseSIUGJ.EnServiceResults.InvocationError:
                    MainThread.BeginInvokeOnMainThread(() =>
                        App.Current.MainPage.DisplayAlert("Servicio no disponible", "Lo sentimos, algo salió mal. Reintente más tarde", "Ok"));
                    break;
            }
        }

        public async System.Threading.Tasks.Task CityOnChange(object sender, EventArgs evtArg)
        {
            if (CitySelectedIndex < 0)
            {
                return;
            }

            var selectedCountry = CityItems[CitySelectedIndex];
            if (selectedCountry != null)
            {
                await LoadPins(selectedCountry.Value);
            }
        }

        public async System.Threading.Tasks.Task SpecialityOnChange(object sender, EventArgs evtArg)
        {
            if (CitySelectedIndex < 0 || SpecialitySelectedIndex < 0)
            {
                return;
            }

            var selectedCountry = CityItems[CitySelectedIndex];
            var selectedSpeciality = SpecialityItems[SpecialitySelectedIndex];

            if (selectedCountry != null && selectedSpeciality != null)
            {
                await LoadPins(selectedCountry.Value, selectedSpeciality);
            }
        }

        public async System.Threading.Tasks.Task LoadPins(string idCity, string? speciality = null)
        {
            if (string.IsNullOrEmpty(speciality))
            {
                CurrentCourts.Clear();
                ClearMapPins();

                SpecialityItems = new List<string>();

                var serviceSIUGJ = ServiceHelper.GetRequiredService<SIUGJ_Service>();
                var response = serviceSIUGJ.UsersGetCourts(idCity, 1, true);

                switch (response.serviceStatus)
                {
                    case DataBaseSIUGJ.EnServiceResults.Success:
                        if (response.responseUsersGetCourts == null || response.responseUsersGetCourts.courts == null)
                            break;

                        AttchPinInit(response.responseUsersGetCourts.courts, true);
                        break;
                    case DataBaseSIUGJ.EnServiceResults.InvocationError:
                        var mainPage = Application.Current?.MainPage;
                        if (mainPage != null)
                        {
                            await mainPage.DisplayAlert("Servicio no disponible", "Lo sentimos, algo salió mal. Reintente más tarde", "Ok");
                        }
                        break;
                }
            }
            else
            {
                AttachMarket(CurrentCourts.Where(court => speciality.Equals("Todas") || court.especialidad == speciality), true);
            }
        }

        private void AttachMarket(IEnumerable<Court> courts, bool fullLoad)
        {
            var courtsList = courts?.Where(p => !string.IsNullOrEmpty(p.latitud) && !string.IsNullOrEmpty(p.longitud)).ToList();
            if (courtsList == null || MapView == null)
            {
                return;
            }

            var specialities = new List<string>();
            var pins = new Dictionary<string, Pin>();

            foreach (var court in courtsList)
            {
                double lat = double.Parse(court.latitud.Replace(",", "."));
                double lon = double.Parse(court.longitud.Replace(",", "."));
                lon = lon > 0 ? lon * -1 : lon;

                var posKey = $"{lat:0.00000}_{lon:0.00000}";
                var address = string.IsNullOrEmpty(court.domicilio)
                    ? "No definido"
                    : $"{court.domicilio}, {court.municipio}, {court.departamento}. Tel: {(string.IsNullOrEmpty(court.telefonos) ? "No definido" : court.telefonos)}, e-mail: {(string.IsNullOrEmpty(court.correos) ? "No definido" : court.correos)}";

                if (pins.TryGetValue(posKey, out var existingPin))
                {
                    existingPin.Label += $" | {court.nombreDespacho}";
                    existingPin.Address += $"\n\n{court.nombreDespacho}: {address}";
                }
                else
                {
                    var pin = new Pin
                    {
                        Label = court.nombreDespacho,
                        Address = address,
                        Location = new Location(lat, lon),
                        Type = PinType.Place
                    };

                    pins[posKey] = pin;
                    RegisterPinInteraction(pin);
                }

                if (fullLoad && !string.IsNullOrEmpty(court.especialidad) && !specialities.Contains(court.especialidad))
                {
                    specialities.Add(court.especialidad);
                }
            }

            MainThread.BeginInvokeOnMainThread(() =>
            {
                MapView.Pins.Clear();
                foreach (var pin in pins.Values)
                {
                    MapView.Pins.Add(pin);
                }

                var span = GeoHelper.GetCenterAndRadius(MapView.Pins);
                if (span != null)
                {
                    var mapSpan = Microsoft.Maui.Maps.MapSpan.FromCenterAndRadius(
                        span.Value.Center,
                        Microsoft.Maui.Maps.Distance.FromKilometers(span.Value.RadiusKm));
                    MapView.MoveToRegion(mapSpan);
                }
            });

            if (fullLoad)
            {
                var specialityList = new List<string> { "Todas" };
                specialityList.AddRange(specialities.Except(specialityList));
                SpecialityItems = specialityList;
            }
        }

        private void AttchPinInit(IEnumerable<Court> courts, bool fullLoad)
        {
            if (courts != null)
            {
                var specialities = new List<string>();
                specialities.Add("Todas");
                var targets = courts.Where(p => !string.IsNullOrEmpty(p.latitud) && !string.IsNullOrEmpty(p.longitud));
                foreach (var pin in targets)
                {
                    // Agregar todas las cortes con coordenadas válidas
                    CurrentCourts.Add(pin);
                    
                    // Recolectar especialidades únicas por separado
                    if (fullLoad && !string.IsNullOrEmpty(pin.especialidad) && !specialities.Contains(pin.especialidad))
                    {
                        specialities.Add(pin.especialidad);
                    }
                }
                
                if (fullLoad)
                {
                    SpecialityItems = specialities;
                }
            }
        }

        private void ClearMapPins()
        {
            if (MapView == null)
            {
                return;
            }

            MainThread.BeginInvokeOnMainThread(() => MapView.Pins.Clear());
        }

        private void RegisterPinInteraction(Pin pin)
        {
            pin.MarkerClicked += async (s, e) =>
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    var title = pin.Label ?? "Detalle";
                    var message = pin.Address ?? string.Empty;
                    var mainPage = Application.Current?.MainPage;
                    if (mainPage != null)
                    {
                        await mainPage.DisplayAlert(title, message, "Cerrar");
                    }
                });
            };
        }
    }
}

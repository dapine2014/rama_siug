using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Maps;
using SIUGJ.Helpers;
using SIUGJ.Services;
using SIUGJ.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SIUGJ.Views
{
    public partial class Headquarters : ContentPage
    {
        public readonly HeadquartersViewModel _viewModel;

        public Headquarters()
        {
            InitializeComponent();
            BindingContext = _viewModel = new HeadquartersViewModel();
            _viewModel.MapView = mapView;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _ = InitializeMapAsync();
        }

        private async Task InitializeMapAsync()
        {
            var locationService = ServiceHelper.GetRequiredService<LocationService>();
            var location = await locationService.GetCurrentLocation();

            if (location != null)
            {
                await Dispatcher.DispatchAsync(() =>
                {
                    mapView.MoveToRegion(Microsoft.Maui.Maps.MapSpan.FromCenterAndRadius(location, Microsoft.Maui.Maps.Distance.FromKilometers(5)));
                });

                var cityName = await locationService.GetCityFromLongLat(location.Longitude, location.Latitude);
                var cityItem = _viewModel.CityItems.FirstOrDefault(city => city.Label == cityName);
                if (cityItem != null)
                {
                    _viewModel.CitySelectedIndex = _viewModel.CityItems.IndexOf(cityItem);
                }
            }
            else
            {
                var defaultLocation = new Location(4.570868, -74.297333);
                mapView.MoveToRegion(Microsoft.Maui.Maps.MapSpan.FromCenterAndRadius(defaultLocation, Microsoft.Maui.Maps.Distance.FromKilometers(100)));
            }
        }
        public void CityOnChange(object sender, EventArgs evtArg) =>
            Task.Run(() => _viewModel.CityOnChange(sender, evtArg));

        public void SpecialityOnChange(object sender, EventArgs evtArg) =>
            Task.Run(() => _viewModel.SpecialityOnChange(sender, evtArg));
    }
}

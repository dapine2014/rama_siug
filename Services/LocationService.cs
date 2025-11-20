using System;
using System.Linq;
using System.Threading;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices.Sensors;

namespace SIUGJ.Services
{
    public class LocationService
    {

        CancellationTokenSource cts;

        public async System.Threading.Tasks.Task<Location?> GetCurrentLocation()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                }

                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null && location.IsFromMockProvider)
                {
                    return null;
                }
                return location;
            }
            catch (FeatureNotSupportedException)
            {
                return null;
            }
            catch (FeatureNotEnabledException)
            {
                return null;
            }
            catch (PermissionException)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (cts != null && !cts.IsCancellationRequested)
                    cts.Cancel();
            }
        }

        public async System.Threading.Tasks.Task<string> GetCityFromLongLat(double Longitude, double Latitude)
        {
            var addrs = (await Geocoding.GetPlacemarksAsync(new Location(Latitude, Longitude))).FirstOrDefault();
            return addrs != null ? string.IsNullOrEmpty(addrs.Locality) ? addrs.AdminArea : addrs.Locality : null;
        }
    }
}

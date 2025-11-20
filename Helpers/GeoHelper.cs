using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Devices.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIUGJ.Helpers
{
	public static class GeoHelper
	{
        public static Location GetCentralGeoCoordinate(IList<Pin> geoCoordinates)
        {
            if (geoCoordinates.Count == 1)
            {
                return geoCoordinates.Single().Location;
            }

            double x = 0;
            double y = 0;
            double z = 0;

            foreach (var geoCoordinate in geoCoordinates)
            {
                var latitude = geoCoordinate.Location.Latitude * Math.PI / 180;
                var longitude = geoCoordinate.Location.Longitude * Math.PI / 180;

                x += Math.Cos(latitude) * Math.Cos(longitude);
                y += Math.Cos(latitude) * Math.Sin(longitude);
                z += Math.Sin(latitude);
            }

            var total = geoCoordinates.Count;

            x = x / total;
            y = y / total;
            z = z / total;

            var centralLongitude = Math.Atan2(y, x) * 180 / Math.PI;
            var centralSquareRoot = Math.Sqrt(x * x + y * y);
            var centralLatitude = Math.Atan2(z, centralSquareRoot) * 180 / Math.PI;

            return new Location(centralLatitude, centralLongitude);
        }

        public static (Location Center, double RadiusKm)? GetCenterAndRadius(this IList<Pin> pins)
        {
            if (pins == null || pins.Count == 0)
                return null;

            var minLat = pins.Min(p => p.Location.Latitude);
            var maxLat = pins.Max(p => p.Location.Latitude);
            var minLon = pins.Min(p => p.Location.Longitude);
            var maxLon = pins.Max(p => p.Location.Longitude);

            var center = new Location((minLat + maxLat) / 2, (minLon + maxLon) / 2);

            // Rough conversion of degrees to kilometers
            var latDistance = Math.Max((maxLat - minLat) * 111, 1);
            var lonDistance = Math.Max((maxLon - minLon) * 111, 1);
            var distance = Math.Max(latDistance, lonDistance);

            return (center, distance / 2);
        }
    }
}

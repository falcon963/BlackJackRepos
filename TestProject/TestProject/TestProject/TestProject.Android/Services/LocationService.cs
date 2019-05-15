using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TestProject.Droid.Services.Interfaces;

namespace TestProject.Droid.Services
{
    public class LocationService
        : ILocationService
    {
        public void GetRandomLocation(LatLng point, int radius, GoogleMap map)
        {
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(10);
                Random random = new Random();

                double x0 = point.Latitude;
                double y0 = point.Longitude;

                double radiusInDegrees = radius / 111000f;

                double u = random.NextDouble();
                double v = random.NextDouble();
                double w = radiusInDegrees * Math.Sqrt(u);
                double t = 2 * Math.PI * v;
                double x = w * Math.Cos(t);
                double y = w * Math.Sin(t);

                double new_x = x / Math.Cos(y0);

                double foundLatitude = new_x + x0;
                double foundLongitude = y + y0;

                LatLng randomLatLng = new LatLng(foundLatitude, foundLongitude);

                MarkerOptions markerRandomOptions = new MarkerOptions();
                markerRandomOptions.SetPosition(randomLatLng);

                map.AddMarker(markerRandomOptions);
            }
        }
    }
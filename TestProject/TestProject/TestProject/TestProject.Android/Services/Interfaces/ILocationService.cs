﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TestProject.Droid.Services.Interfaces
{
    public interface ILocationService
    {
        void GetRandomLocation(LatLng latLng, int radius, GoogleMap map);
    }
}
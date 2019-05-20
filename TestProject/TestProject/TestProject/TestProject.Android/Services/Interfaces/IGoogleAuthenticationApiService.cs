using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using static Android.Gms.Common.Apis.GoogleApiClient;

namespace TestProject.Droid.Services.Interfaces
{
    public interface IGoogleAuthenticationApiService
        : IOnConnectionFailedListener, IConnectionCallbacks
    {
    }
}
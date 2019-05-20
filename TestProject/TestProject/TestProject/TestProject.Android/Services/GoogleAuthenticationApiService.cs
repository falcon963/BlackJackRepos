using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Common;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross;
using MvvmCross.Logging;
using TestProject.Droid.Services.Interfaces;

namespace TestProject.Droid.Services
{
    public class GoogleAuthenticationApiService
        : Java.Lang.Object, IGoogleAuthenticationApiService
    {
        private IMvxLog _mvxLog;

        public GoogleAuthenticationApiService()
        {
            _mvxLog = Mvx.IoCProvider.Resolve<IMvxLog>();
        }

        public void OnConnected(Bundle connectionHint)
        {
            ;
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            _mvxLog.Trace(result.ErrorMessage);
        }

        public void OnConnectionSuspended(int cause)
        {
            ;
        }
    }
}
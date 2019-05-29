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
using Xamarin.Auth;

namespace TestProject.Droid.Services.Interfaces
{
    public interface IAuthenticationGoogleService
    {
        event EventHandler OnAuthenticationCanceled;
        event EventHandler OnAuthenticationCompleted;
        event EventHandler OnAuthenticationFailed;

        void InitializeGoogleAuth();
        OAuth2Authenticator GetAuthenticator();
    }
}
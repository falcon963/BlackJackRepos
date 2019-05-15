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
using TestProject.Core.Authentication.Interfaces;
using Xamarin.Auth;

namespace TestProject.Droid.Services.Interfaces
{
    public interface IFacebookAuthenticationService
    {
        event EventHandler OnAuthenticationCanceled;
        event EventHandler OnAuthenticationCompleted;
        event EventHandler OnAuthenticationFailed;

        void InitializeFacebookAuth();
        OAuth2Authenticator GetAuthenticator();
    }
}
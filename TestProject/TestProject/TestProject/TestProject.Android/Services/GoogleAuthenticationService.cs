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
using TestProject.Core.Authentication;
using TestProject.Core.Authentication.Interfaces;
using TestProject.Core.Constants;
using TestProject.Droid.Services.Interfaces;
using Xamarin.Auth;

namespace TestProject.Droid.Services
{
    public class GoogleAuthenticationService
        : IGoogleAuthenticationDelegate, IAuthenticationGoogleService
    {
        private GoogleAuthenticator _authGoogle = null;

        public event EventHandler OnAuthenticationCanceled;
        public event EventHandler OnAuthenticationCompleted;
        public event EventHandler OnAuthenticationFailed;

        public GoogleAuthenticationService()
        {
            InitializeFacebookAuth();
        }

        public void InitializeFacebookAuth()
        {
            InitializeGoogleAuth();
        }

        void IGoogleAuthenticationDelegate.OnAuthenticationCanceled()
        {
            OnAuthenticationCanceled?.Invoke(this, EventArgs.Empty);
        }

        void IGoogleAuthenticationDelegate.OnAuthenticationCompleted(string token)
        {
            OnAuthenticationCompleted?.Invoke(this, EventArgs.Empty);
        }

        void IGoogleAuthenticationDelegate.OnAuthenticationFailed(string message, Exception exception)
        {
            OnAuthenticationFailed?.Invoke(this, EventArgs.Empty);
        }

        public OAuth2Authenticator GetAuthenticator()
        {
            return _authGoogle.GetAuthenticator();
        }

        public void InitializeGoogleAuth()
        {
            _authGoogle = new GoogleAuthenticator(SocialConstants.ClientIdAndroidGoogle, SocialConstants.Scope,
                new Uri("com.googleusercontent.apps.70862177039-u5gele75q6ca0f76r9o82muh4arha4sa:/oauth2redirect"), this);
        }
    }
}
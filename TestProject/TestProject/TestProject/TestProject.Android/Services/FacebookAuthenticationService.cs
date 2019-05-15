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
using Java.Lang;
using TestProject.Core.Authentication;
using TestProject.Core.Authentication.Interfaces;
using TestProject.Core.Constants;
using TestProject.Droid.Services.Interfaces;
using Xamarin.Auth;

namespace TestProject.Droid.Services
{
    public class FacebookAuthenticationService
        : IFacebookAuthenticationService, IFacebookAuthentication
    {
        public event EventHandler OnAuthenticationCanceled;
        public event EventHandler OnAuthenticationCompleted;
        public event EventHandler OnAuthenticationFailed;

        private FacebookAuthenticator _authFacebook;

        public FacebookAuthenticationService()
        {
            InitializeFacebookAuth();
        }

        public void InitializeFacebookAuth()
        {
            _authFacebook = new FacebookAuthenticator(SocialConstants.ClientIdFacebook, SocialConstants.Scope, this);
        }

        void IFacebookAuthentication.OnAuthenticationCompleted(string token)
        {
            OnAuthenticationCompleted?.Invoke(this, EventArgs.Empty);
        }

        void IFacebookAuthentication.OnAuthenticationFailed(string message, System.Exception exception)
        {
            OnAuthenticationFailed?.Invoke(this, EventArgs.Empty);
        }

        void IFacebookAuthentication.OnAuthenticationCanceled()
        {
            OnAuthenticationCanceled?.Invoke(this, EventArgs.Empty);
        }

        public OAuth2Authenticator GetAuthenticator()
        {
            return _authFacebook.GetAuthenticator();
        }
    }
}
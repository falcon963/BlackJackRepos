using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using TestProject.Core.Authentication;
using TestProject.Core.Authentication.Interfaces;
using TestProject.Core.Constants;
using TestProject.iOS.Services.Interfaces;
using UIKit;
using Xamarin.Auth;

namespace TestProject.iOS.Services
{
    public class AuthenticationGoogleService
        : IGoogleAuthenticationDelegate,  IAuthenticationGoogleService
    {

        private GoogleAuthenticator _authGoogle = null;
        private const string scope = "email";

        private object _objectLock;

        private event EventHandler GoogleCanceled;
        private event EventHandler GoogleCompleted;
        private event EventHandler GoogleFailed;


        public AuthenticationGoogleService()
        {
            _objectLock = new object();
            InitializeGoogleAuth();
        }


        #region IAuthenticationGoogleService events

        event EventHandler IAuthenticationGoogleService.OnAuthenticationCanceled
        {
            add
            {
                lock (_objectLock)
                {
                    GoogleCanceled += value;
                }
            }

            remove
            {
                lock (_objectLock)
                {
                    GoogleCanceled -= value;
                }
            }
        }

        event EventHandler IAuthenticationGoogleService.OnAuthenticationCompleted
        {
            add
            {
                lock (_objectLock)
                {
                    GoogleCompleted += value;
                }
            }

            remove
            {
                lock (_objectLock)
                {
                    GoogleCompleted -= value;
                }
            }
        }

        event EventHandler IAuthenticationGoogleService.OnAuthenticationFailed
        {
            add
            {
                lock (_objectLock)
                {
                    GoogleFailed += value;
                }
            }

            remove
            {
                lock (_objectLock)
                {
                    GoogleFailed -= value;
                }
            }
        }

        #endregion

        void IGoogleAuthenticationDelegate.OnAuthenticationCompleted(string token)
        {
            GoogleCompleted?.Invoke(this, EventArgs.Empty);
        }

        void IGoogleAuthenticationDelegate.OnAuthenticationFailed(string message, Exception exception)
        {
            GoogleFailed?.Invoke(this, EventArgs.Empty);
        }

        void IGoogleAuthenticationDelegate.OnAuthenticationCanceled()
        {
            GoogleCanceled?.Invoke(this, EventArgs.Empty);
        }

        public void InitializeGoogleAuth()
        {
            var googleDictionary = NSDictionary.FromFile("credentials.plist");
            string clienId = googleDictionary["CLIENT_ID"].ToString();
            string reversedClientId = googleDictionary["REVERSED_CLIENT_ID"].ToString();

            _authGoogle = new GoogleAuthenticator(clienId, scope,
                new Uri($"{reversedClientId}:/oauth2redirect"), this);
            //_authGoogle = new GoogleAuthenticator("70862177039-jm46ae5e77822hk8qllegch1fqler0a4.apps.googleusercontent.com", "email",
            //    new Uri("com.googleusercontent.apps.70862177039-jm46ae5e77822hk8qllegch1fqler0a4:/oauth2redirect"), this);
        }

        public OAuth2Authenticator GetAuthenticator()
        {
            return _authGoogle.GetAuthenticator();
        }
    }
}
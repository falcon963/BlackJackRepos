using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Constant;
using TestProject.Core.Interfaces.SocialService.Google;
using TestProject.Core.Models;
using Xamarin.Auth;

namespace TestProject.Core.Authentication
{
    public class GoogleAuthenticator
    {
        public static Uri AuthorizeUrl = new Uri("https://accounts.google.com/o/oauth2/v2/auth");
        private const string TokenUrl = "https://www.googleapis.com/oauth2/v4/token";
        private const bool IsUsingNativeUI = true;


        private OAuth2Authenticator _auth;
        private IGoogleAuthenticationDelegate _authenticationDelegate;

        public GoogleAuthenticator(string clientId, string scope, Uri redirectUrl, IGoogleAuthenticationDelegate googleAuthenticationDelegate)
        {
            _authenticationDelegate = googleAuthenticationDelegate;
            //AccessTokenUrl = new Uri(TokenUrl);
            _auth = new OAuth2Authenticator(clientId, string.Empty, scope,
                                            AuthorizeUrl,
                                            redirectUrl,
                                            new Uri(TokenUrl),
                                            null, IsUsingNativeUI);
            _auth.Completed += OnAuthenticationCompleted;
            _auth.Error += OnAuthenticationFailed;
        }

        public void OnPageLoading(Uri uri)
        {
            _auth.OnPageLoading(uri);
        }

        public OAuth2Authenticator GetAuthenticator()
        {
            return _auth;
        }

        //public void OnRetrievedAccountProperties(IDictionary<string, string> accountProperties)
        //{
        //    _auth.OnRetrievedAccountProperties(accountProperties);
        //}
        //protected void OnCreatingInitialUrl(IDictionary<string, string> query) => _auth.OnCreatingInitialUrl(query);

        private void OnAuthenticationCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
                var token = e.Account.Properties["access_token"];
                CrossSecureStorage.Current.SetValue(SecureConstant.AccessToken, token);
                _authenticationDelegate.OnAuthenticationCompleted(token);
            }
            else
            {
                _authenticationDelegate.OnAuthenticationCanceled();
            }
        }

        private void OnAuthenticationFailed(object sender, AuthenticatorErrorEventArgs e)
        {
            _authenticationDelegate.OnAuthenticationFailed(e.Message, e.Exception);
        }

    }
}

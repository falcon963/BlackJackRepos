using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Authentication.Interfacies;
using TestProject.Core.Constants;
using TestProject.Core.Models;
using TestProject.Core.Servicies.Interfacies.SocialService.Google;
using Xamarin.Auth;

namespace TestProject.Core.Authentication
{
    public class GoogleAuthenticator
    {
        public static Uri AuthorizeUrl = new Uri(SocialConstant.AuthorizeUrlGoogle);
        private const bool IsUsingNativeUI = true;


        private readonly OAuth2Authenticator _auth;
        private readonly IGoogleAuthenticationDelegate _authenticationDelegate;

        public GoogleAuthenticator(string clientId, string scope, Uri redirectUrl, IGoogleAuthenticationDelegate googleAuthenticationDelegate)
        {
            _authenticationDelegate = googleAuthenticationDelegate;
            _auth = new OAuth2Authenticator(clientId, string.Empty, scope,
                                            AuthorizeUrl,
                                            redirectUrl,
                                            new Uri(SocialConstant.TokenUrlGoogle),
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

        private void OnAuthenticationCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
                var token = e.Account.Properties[SocialConstant.CompletedProperties];
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

using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Authentication.Interfacies;
using TestProject.Core.Constants;
using TestProject.Core.Models;
using TestProject.Core.Servicies.Interfacies.SocialService.Facebook;
using Xamarin.Auth;

namespace TestProject.Core.Authentication
{
    public class FacebookAuthenticator
    {
        private const bool IsUsingNativeUI = false;

        private readonly OAuth2Authenticator _auth;
        private readonly IFacebookAuthentication _authentication;

        public FacebookAuthenticator(string clientId, string scope, IFacebookAuthentication authentication, bool isUsingNativeUi = IsUsingNativeUI)
        {
            _authentication = authentication;

            _auth = new OAuth2Authenticator(clientId, scope,
                                            new Uri(SocialConstant.AuthorizeUrlFacebook),
                                            new Uri(SocialConstant.RedirectUrlFacebook),
                                            null, IsUsingNativeUI);

            _auth.Completed += OnAuthenticationCompleted;
            _auth.Error += OnAuthenticationFailed;
        }

        public OAuth2Authenticator GetAuthenticator()
        {
            return _auth;
        }

        public void OnPageLoading(Uri uri)
        {
            _auth.OnPageLoading(uri);
        }

        private void OnAuthenticationCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
                var token = e.Account.Properties[SocialConstant.CompletedProperties];
                CrossSecureStorage.Current.SetValue(SecureConstant.AccessToken, token);
                _authentication.OnAuthenticationCompleted(token);
            }
            else
            {
                _authentication.OnAuthenticationCanceled();
            }
        }

        private void OnAuthenticationFailed(object sender, AuthenticatorErrorEventArgs e)
        {
            _authentication.OnAuthenticationFailed(e.Message, e.Exception);
        }
    }
}

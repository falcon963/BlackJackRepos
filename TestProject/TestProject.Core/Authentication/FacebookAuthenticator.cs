using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Constant;
using TestProject.Core.Interfaces.SocialService.Facebook;
using TestProject.Core.Models;
using Xamarin.Auth;

namespace TestProject.Core.Authentication
{
    public class FacebookAuthenticator
    {
        private const String AuthorizeUrl = "https://m.facebook.com/dialog/oauth/";
        private const String RedirectUrl = "https://www.facebook.com/connect/login_success.html";
        private const Boolean IsUsingNativeUI = false;

        private OAuth2Authenticator _auth;
        private IFacebookAuthentication _authentication;

        public FacebookAuthenticator(String clientId, String scope, IFacebookAuthentication authentication, Boolean isUsingNativeUi = IsUsingNativeUI)
        {
            _authentication = authentication;

            _auth = new OAuth2Authenticator(clientId, scope,
                                            new Uri(AuthorizeUrl),
                                            new Uri(RedirectUrl),
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

        private void OnAuthenticationCompleted(Object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
                var token = e.Account.Properties["access_token"];
                CrossSecureStorage.Current.SetValue(SecureConstant.AccessToken, token);
                _authentication.OnAuthenticationCompleted(token);
            }
            else
            {
                _authentication.OnAuthenticationCanceled();
            }
        }

        private void OnAuthenticationFailed(Object sender, AuthenticatorErrorEventArgs e)
        {
            _authentication.OnAuthenticationFailed(e.Message, e.Exception);
        }
    }
}

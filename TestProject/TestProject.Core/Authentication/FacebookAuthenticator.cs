using MvvmCross;
using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Authentication.Interfaces;
using TestProject.Core.Constants;
using TestProject.Core.Helpers.Interfaces;
using TestProject.Core.Models;
using TestProject.Core.Services.Interfaces.SocialService.Facebook;
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
                                            new Uri(SocialConstants.AuthorizeUrlFacebook),
                                            new Uri(SocialConstants.RedirectUrlFacebook),
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
                var token = e.Account.Properties[SocialConstants.CompletedProperties];

                var userHelper = Mvx.IoCProvider.Resolve<IUserHelper>();

                userHelper.UserAccessToken = token;

                _authentication.OnAuthenticationCompleted(token);
            }

            if(!e.IsAuthenticated)
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

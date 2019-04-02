using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Interfaces;
using Xamarin.Auth;

namespace TestProject.Core.Services
{
    public class OAuthService
        : IOAuthService
    {
        public OAuth2Authenticator AndroidLoginFacebook()
        {
            throw new NotImplementedException();
        }

        public OAuth2Authenticator AndroidLoginGoogle()
        {
            throw new NotImplementedException();
        }

        public OAuth2Authenticator IOSLoginFacebook()
        {
            var auth = new OAuth2Authenticator(
                clientId: "578268892583841",
                scope: "",
                authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
                redirectUrl: new Uri("http://www.facebook.com/connect/login_success.html")
                );
            return auth;
        }

        public OAuth2Authenticator IOSLoginGoogle()
        {
            var auth = new OAuth2Authenticator(
                clientId: "70862177039-jm46ae5e77822hk8qllegch1fqler0a4.apps.googleusercontent.com",
                scope: "",
                authorizeUrl: new Uri("https://accounts.google.com/o/oauth2/v2/auth"),
                redirectUrl: new Uri("https://www.googleapis.com/oauth2/v4/token")
                );
            return auth;
        }
    }
}

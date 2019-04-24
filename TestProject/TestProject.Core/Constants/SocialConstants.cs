using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject.Core.Constants
{
    public class SocialConstants
    {
        public const string ClientIdiOSFacebook = "578268892583841";
        public const string ClientIdiOSGoogle = "70862177039-jm46ae5e77822hk8qllegch1fqler0a4.apps.googleusercontent.com";
        public const string ClientSecret = "DcP6ERfnO1qRQqWyIu29w6ol";
        public const string RedirectUrlGoogle = "com.mvvmcross.testproject:/oauth2redirect";
        public const string Scope = "email";
        public const string TokenUrlGoogle = "https://www.googleapis.com/oauth2/v4/token";
        public const string RedirectUrlFacebook = "https://www.facebook.com/connect/login_success.html";
        public const string AuthorizeUrlGoogle = "https://accounts.google.com/o/oauth2/v2/auth";
        public const string AuthorizeUrlFacebook = "https://m.facebook.com/dialog/oauth/";
        public const string TokenUrlFacebook = "https://graph.facebook.com/oauth/access_token";
        public const string CompletedProperties = "access_token";
    }
}

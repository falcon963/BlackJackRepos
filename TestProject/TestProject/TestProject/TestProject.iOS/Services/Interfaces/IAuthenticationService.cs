using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Auth;

namespace TestProject.iOS.Services.Interfaces
{
    public interface IAuthenticationGoogleService
    {
        event EventHandler OnAuthenticationCanceled;
        event EventHandler OnAuthenticationCompleted;
        event EventHandler OnAuthenticationFailed;

        void InitializeGoogleAuth();
        OAuth2Authenticator GetAuthenticator();
    }

    public interface IAuthenticationFacebookService
    {
        event EventHandler OnAuthenticationCanceled;
        event EventHandler OnAuthenticationCompleted;
        event EventHandler OnAuthenticationFailed;

        void InitializeFacebookAuth();
        OAuth2Authenticator GetAuthenticator();
    }
}
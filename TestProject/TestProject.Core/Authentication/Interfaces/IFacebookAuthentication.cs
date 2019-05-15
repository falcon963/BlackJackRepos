using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Models;
using Xamarin.Auth;

namespace TestProject.Core.Authentication.Interfaces
{
    public interface IFacebookAuthentication
    {
        void OnAuthenticationCompleted(string token);
        void OnAuthenticationFailed(string message, Exception exception);
        void OnAuthenticationCanceled();
    }
}

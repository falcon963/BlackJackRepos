using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using TestProject.Core.Authentication.Interfaces;
using TestProject.iOS.Models;
using TestProject.iOS.Views;
using UIKit;

namespace TestProject.iOS.Services
{
    public class AuthenticationService
        : IFacebookAuthentication, IGoogleAuthenticationDelegate
    {

        private AuthenticationModel _authenticationModel;

        public AuthenticationService(AuthenticationModel authenticationModel)
        {
            _authenticationModel = authenticationModel;
        }

       void IFacebookAuthentication.OnAuthenticationCanceled()
        {
            _authenticationModel?.FacebookOnAuthenticationCanceled?.Invoke();
            _authenticationModel?.FacebookInitialize?.Invoke();
        }

        void IFacebookAuthentication.OnAuthenticationCompleted(string token)
        {
            _authenticationModel?.FacebookOnAuthenticationCompleted?.Invoke();
            _authenticationModel?.DissmisController.Invoke();
        }

        void IFacebookAuthentication.OnAuthenticationFailed(string message, Exception exception)
        {
            _authenticationModel?.GoogleOnAuthenticationFailed?.Invoke();
            _authenticationModel?.DissmisController?.Invoke();
        }

        void IGoogleAuthenticationDelegate.OnAuthenticationCompleted(string token)
        {
            _authenticationModel?.FacebookOnAuthenticationCompleted?.Invoke();
            _authenticationModel?.DissmisController?.Invoke();
        }

        void IGoogleAuthenticationDelegate.OnAuthenticationFailed(string message, Exception exception)
        {
            _authenticationModel?.FacebookOnAuthenticationFailed?.Invoke();
            _authenticationModel?.DissmisController?.Invoke();
        }

        void IGoogleAuthenticationDelegate.OnAuthenticationCanceled()
        {
            _authenticationModel?.FacebookOnAuthenticationCanceled?.Invoke();
        }
    }
}
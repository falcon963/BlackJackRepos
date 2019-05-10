using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TestProject.Core.Authentication.Interfaces;

namespace TestProject.Droid.Services
{
    public class GoogleAuthenticationService
        : IGoogleAuthenticationDelegate
    {
        private Action SignInCommand { get; set; }
        public GoogleAuthenticationService(Action signInCommand)
        {
            SignInCommand = signInCommand;
        }

        void IGoogleAuthenticationDelegate.OnAuthenticationCanceled()
        {
            ;
        }

        void IGoogleAuthenticationDelegate.OnAuthenticationCompleted(string token)
        {
            SignInCommand?.Invoke();
        }

        void IGoogleAuthenticationDelegate.OnAuthenticationFailed(string message, Exception exception)
        {
            ;
        }
    }
}
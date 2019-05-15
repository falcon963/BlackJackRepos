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
        public event EventHandler GoogleAuthenticationCanceled;
        public event EventHandler GoogleAuthenticationCompleted;
        public event EventHandler GoogleAuthenticationFailed;

        void IGoogleAuthenticationDelegate.OnAuthenticationCanceled()
        {
            GoogleAuthenticationCanceled?.Invoke(this, EventArgs.Empty);
        }

        void IGoogleAuthenticationDelegate.OnAuthenticationCompleted(string token)
        {
            GoogleAuthenticationCompleted?.Invoke(this, EventArgs.Empty);
        }

        void IGoogleAuthenticationDelegate.OnAuthenticationFailed(string message, Exception exception)
        {
            GoogleAuthenticationFailed?.Invoke(this, EventArgs.Empty);
        }
    }
}
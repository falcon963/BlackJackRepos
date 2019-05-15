using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using TestProject.Core.Authentication;
using TestProject.Core.Authentication.Interfaces;
using TestProject.Core.Constants;
using TestProject.iOS.Services.Interfaces;
using UIKit;

namespace TestProject.iOS.Services
{
    public class AuthenticationFacebookService
        : IAuthenticationFacebookService, IFacebookAuthentication
    {

        private event EventHandler FacebookCanceled;
        private event EventHandler FacebookCompleted;
        private event EventHandler FacebookFailed;

        private FacebookAuthenticator _authFacebook;

        private object _objectLock;

        public AuthenticationFacebookService()
        {
            _objectLock = new object();
            InitializeFacebookAuth();
        }

        #region IAuthenticationFacebookService events
        event EventHandler IAuthenticationFacebookService.OnAuthenticationCanceled
        {
            add
            {
                lock (_objectLock)
                {
                    FacebookCanceled += value;
                }
            }

            remove
            {
                lock (_objectLock)
                {
                    FacebookCanceled -= value;
                }
            }
        }

        event EventHandler IAuthenticationFacebookService.OnAuthenticationCompleted
        {
            add
            {
                lock (_objectLock)
                {
                    FacebookCompleted += value;
                }
            }

            remove
            {
                lock (_objectLock)
                {
                    FacebookCompleted -= value;
                }
            }
        }

        event EventHandler IAuthenticationFacebookService.OnAuthenticationFailed
        {
            add
            {
                lock (_objectLock)
                {
                    FacebookFailed += value;
                }
            }

            remove
            {
                lock (_objectLock)
                {
                    FacebookFailed -= value;
                }
            }
        }

        #endregion

        void IFacebookAuthentication.OnAuthenticationCanceled()
        {
            FacebookCanceled?.Invoke(this, EventArgs.Empty);
            InitializeFacebookAuth();
        }

        void IFacebookAuthentication.OnAuthenticationCompleted(string token)
        {
            FacebookCompleted?.Invoke(this, EventArgs.Empty);
        }

        void IFacebookAuthentication.OnAuthenticationFailed(string message, Exception exception)
        {
            FacebookFailed?.Invoke(this, EventArgs.Empty);
        }

        public void InitializeFacebookAuth()
        {
            _authFacebook = new FacebookAuthenticator(SocialConstants.ClientIdFacebook, SocialConstants.Scope, this);
        }
    }
}
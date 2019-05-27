using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Constants;
using TestProject.Core.Helpers.Interfaces;

namespace TestProject.Core.Helpers
{
    public class UserHelper
        : IUserHelper
    {
        public string UserLogin
        {
            get
            {
                return CrossSecureStorage.Current.GetValue(nameof(SecureConstants.Login));
            }
            set
            {
                CrossSecureStorage.Current.SetValue(nameof(SecureConstants.Login), value);
            }
        }

        public string UserPassword
        {
            get
            {
                return CrossSecureStorage.Current.GetValue(nameof(SecureConstants.Password));
            }
            set
            {
                CrossSecureStorage.Current.SetValue(nameof(SecureConstants.Password), value);
            }
        }

        public int UserId
        {
            get
            {
                int userId;

                string stringUserId = CrossSecureStorage.Current.GetValue(nameof(SecureConstants.UserId));

                int.TryParse(stringUserId, out userId);

                return userId;
            }
            set
            {
                CrossSecureStorage.Current.SetValue(nameof(SecureConstants.UserId), value.ToString());
            }
        }

        public string UserAccessToken
        {
            get
            {
                return CrossSecureStorage.Current.GetValue(nameof(SecureConstants.AccessToken));
            }
            set
            {
                CrossSecureStorage.Current.SetValue(nameof(SecureConstants.AccessToken), value);
            }
        }

        public bool IsUserLogin
        {
            get
            {
                bool userStatus;
                string stringUserStatus = CrossSecureStorage.Current.GetValue(nameof(SecureConstants.Status));

                bool.TryParse(stringUserStatus, out userStatus);

                return userStatus;
            }
            set
            {
                CrossSecureStorage.Current.SetValue(nameof(SecureConstants.Status), value.ToString());
            }
        }

        public void DeleteUserStatus()
        {
            CrossSecureStorage.Current.DeleteKey(nameof(SecureConstants.Status));
        }

        public void DeleteUserId()
        {
            CrossSecureStorage.Current.DeleteKey(nameof(SecureConstants.UserId));
        }

        public void DeleteUserAccessToken()
        {
            CrossSecureStorage.Current.DeleteKey(nameof(SecureConstants.AccessToken));
        }
    }
}

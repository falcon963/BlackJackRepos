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
                return CrossSecureStorage.Current.GetValue(SecureConstants.Login);
            }
            set
            {
                CrossSecureStorage.Current.SetValue(SecureConstants.Login, value);
            }
        }

        public string UserPassword
        {
            get
            {
                return CrossSecureStorage.Current.GetValue(SecureConstants.Password);
            }
            set
            {
                CrossSecureStorage.Current.SetValue(SecureConstants.Password, value);
            }
        }

        public int UserId
        {
            get
            {
                int userId;
                string stringUserId = CrossSecureStorage.Current.GetValue(SecureConstants.UserId);

                Int32.TryParse(stringUserId, out userId);

                return userId;
            }
            set
            {
                CrossSecureStorage.Current.SetValue(SecureConstants.UserId, value.ToString());
            }
        }

        public string UserAccessToken
        {
            get
            {
                return CrossSecureStorage.Current.GetValue(SecureConstants.AccessToken);
            }
            set
            {
                CrossSecureStorage.Current.SetValue(SecureConstants.AccessToken, value);
            }
        }

        public bool IsUserLogin
        {
            get
            {
                bool userStatus;
                string stringUserStatus = CrossSecureStorage.Current.GetValue(SecureConstants.Status);

                Boolean.TryParse(stringUserStatus, out userStatus);

                return userStatus;
            }
            set
            {
                CrossSecureStorage.Current.SetValue(SecureConstants.Status, value.ToString());
            }
        }

        public void DeleteUserStatus()
        {
            CrossSecureStorage.Current.DeleteKey(SecureConstants.Status);
        }

        public void DeleteUserAccessToken()
        {
            CrossSecureStorage.Current.DeleteKey(SecureConstants.AccessToken);
        }
    }
}

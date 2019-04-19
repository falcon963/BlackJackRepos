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
                return CrossSecureStorage.Current.GetValue(SecureConstant.Login);
            }
            set
            {
                CrossSecureStorage.Current.SetValue(SecureConstant.Login, value.ToString());
            }
        }

        public string UserPassword
        {
            get
            {
                return CrossSecureStorage.Current.GetValue(SecureConstant.Password);
            }
            set
            {
                CrossSecureStorage.Current.SetValue(SecureConstant.Password, value.ToString());
            }
        }

        public int UserId
        {
            get
            {
                int userId;
                string stringUserId = CrossSecureStorage.Current.GetValue(SecureConstant.UserId);
                Int32.TryParse(stringUserId, out userId);
                return userId;
            }
            set
            {
                CrossSecureStorage.Current.SetValue(SecureConstant.UserId, value.ToString());
            }
        }

        public string UserAccessToken
        {
            get
            {
                return CrossSecureStorage.Current.GetValue(SecureConstant.AccessToken);
            }
            set
            {
                CrossSecureStorage.Current.SetValue(SecureConstant.AccessToken, value.ToString());
            }
        }

        public bool UserStatus
        {
            get
            {
                bool userStatus;
                string stringUserStatus = CrossSecureStorage.Current.GetValue(SecureConstant.Status);
                Boolean.TryParse(stringUserStatus, out userStatus);
                return userStatus;
            }
            set
            {
                CrossSecureStorage.Current.SetValue(SecureConstant.Status, value.ToString());
            }
        }

        public void DeleteUserStatus()
        {
            CrossSecureStorage.Current.DeleteKey(SecureConstant.Status);
        }

        public void DeleteUserAccessToken()
        {
            CrossSecureStorage.Current.DeleteKey(SecureConstant.AccessToken);
        }
    }
}

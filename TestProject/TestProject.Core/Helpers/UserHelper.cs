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
        public void DeleteUserStatus()
        {
            CrossSecureStorage.Current.DeleteKey(SecureConstant.Status);
        }

        public string GetUserAccessToken()
        {
            return CrossSecureStorage.Current.GetValue(SecureConstant.AccessToken);
        }

        public int GetUserId()
        {
            return Int32.Parse(CrossSecureStorage.Current.GetValue(SecureConstant.UserId));
        }

        public bool GetUserStatus()
        {
            if(CrossSecureStorage.Current.GetValue(SecureConstant.Status) == "True")
            {
                return true;
            }
            return false;
        }

        public void SetUserStatus(bool status)
        {
            if (status)
            {
                CrossSecureStorage.Current.SetValue(SecureConstant.Status, "True");
            }
            if (!status)
            {
                CrossSecureStorage.Current.SetValue(SecureConstant.Status, "False");
            }
        }

        public void SetUserId(int userId)
        {
            CrossSecureStorage.Current.SetValue(SecureConstant.UserId, userId.ToString());
        }

        public void SetUserLogin(string login)
        {
            CrossSecureStorage.Current.SetValue(SecureConstant.Login, login.ToString());
        }

        public void SetUserPassword(string password)
        {
            CrossSecureStorage.Current.SetValue(SecureConstant.Password, password.ToString());
        }

        public string GetUserLogin()
        {
            return CrossSecureStorage.Current.GetValue(SecureConstant.Login);
        }

        public string GetUserPassword()
        {
            return CrossSecureStorage.Current.GetValue(SecureConstant.Password);
        }

        public void DeleteUserAccessToken()
        {
            CrossSecureStorage.Current.DeleteKey(SecureConstant.AccessToken);
        }
    }
}

using MvvmCross;
using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Constants;
using TestProject.Core.Helpers.Interfaces;
using TestProject.Core.Repositorys.Interfaces;

namespace TestProject.Core.Helpers
{
    public class StartPageCheckHelper
        : IStartPageCheckHelper
    {


        public StartPageCheckHelper()
        {

        }

        public bool AccountStatus()
        {
            var statusValue = CrossSecureStorage.Current.GetValue(SecureConstant.Status);
            bool boolValue;
            Boolean.TryParse(statusValue, out boolValue);
            return boolValue;
        }

        public bool CheckAccountAccess()
        {
            var _loginService = Mvx.IoCProvider.Resolve<ILoginRepository>();
            var login = CrossSecureStorage.Current.GetValue(SecureConstant.Login);
            var password = CrossSecureStorage.Current.GetValue(SecureConstant.Password);
            var user = _loginService.CheckAccountAccess(login, password);
            if(user == null)
            {
                return false;
            }
            return true;
        }

        public int SetUserId()
        {
            var _loginService = Mvx.IoCProvider.Resolve<ILoginRepository>();
            var login = CrossSecureStorage.Current.GetValue(SecureConstant.Login);
            var password = CrossSecureStorage.Current.GetValue(SecureConstant.Password);
            var user = _loginService.CheckAccountAccess(login, password);
            return user.Id;
        }

        public bool SocialNetworkLogin()
        {
            if (String.IsNullOrWhiteSpace(CrossSecureStorage.Current.GetValue(SecureConstant.AccessToken)))
            {
                return false;
            }
            if (String.IsNullOrEmpty(CrossSecureStorage.Current.GetValue(SecureConstant.AccessToken)))
            {
                return false;
            }

            return true;
        }
    }
}

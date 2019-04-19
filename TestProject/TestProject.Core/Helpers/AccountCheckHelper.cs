using MvvmCross;
using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Constants;
using TestProject.Core.Helpers.Interfaces;
using TestProject.Core.Repositories.Interfacies;

namespace TestProject.Core.Helpers
{
    public class AccountCheckHelper
        : IAccountCheckHelper
    {
        public readonly ILoginRepository _loginRepository;

        public AccountCheckHelper(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        public bool IsAccountStatus()
        {
            var statusValue = CrossSecureStorage.Current.GetValue(SecureConstant.Status);
            bool isAccountStatus;
            Boolean.TryParse(statusValue, out isAccountStatus);
            return isAccountStatus;
        }

        public bool IsCheckAccountAccess()
        {
            var login = CrossSecureStorage.Current.GetValue(SecureConstant.Login);
            var password = CrossSecureStorage.Current.GetValue(SecureConstant.Password);
            var user = _loginRepository.GetAppRegistrateUserAccount(login, password);
            return user == null;
        }

        public int GetUserId()
        {
            var login = CrossSecureStorage.Current.GetValue(SecureConstant.Login);
            var password = CrossSecureStorage.Current.GetValue(SecureConstant.Password);
            var user = _loginRepository.GetAppRegistrateUserAccount(login, password);
            return user.Id;
        }

        public bool IsSocialNetworkLogin()
        {
            var isSocialNetworkLogin = String.IsNullOrWhiteSpace(CrossSecureStorage.Current.GetValue(SecureConstant.AccessToken));
            return isSocialNetworkLogin;
        }
    }
}

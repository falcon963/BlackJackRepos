using MvvmCross;
using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Constants;
using TestProject.Core.Helpers.Interfaces;
using TestProject.Core.Repositories.Interfaces;

namespace TestProject.Core.Helpers
{
    public class AccountCheckHelper
        : IAccountCheckHelper
    {
        private readonly ILoginRepository _loginRepository;

        private readonly IUserHelper _userHelper;

        public AccountCheckHelper(ILoginRepository loginRepository, IUserHelper userHelper)
        {
            _loginRepository = loginRepository;
            _userHelper = userHelper;
        }

        public bool IsAccountStatus()
        {
            var statusValue = _userHelper.IsUserLogin;

            return statusValue;
        }

        public bool IsCheckAccountAccess()
        {
            var login = _userHelper.UserLogin;
            var password = _userHelper.UserPassword;
            var user = _loginRepository.GetAppRegistrateUserAccount(login, password);

            return user != null;
        }

        public int GetUserId()
        {
            var login = _userHelper.UserLogin;
            var password = _userHelper.UserPassword;
            var user = _loginRepository.GetAppRegistrateUserAccount(login, password);

            return user.Id;
        }

        public bool IsSocialNetworkLogin()
        {
            var isSocialNetworkLogin = !string.IsNullOrEmpty(_userHelper.UserAccessToken);

            return isSocialNetworkLogin;
        }
    }
}

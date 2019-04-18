using MvvmCross.Commands;
using MvvmCross.Navigation;
using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Constants;
using TestProject.Core.Interfaces;
using TestProject.Core.Models;
using TestProject.Core.Repositorys.Interfaces;

namespace TestProject.Core.ViewModels
{
    public class MainRegistrationViewModel
        : BaseViewModel
    {
        #region Fields

        private readonly ILoginRepository _loginService;

        #endregion

        public MainRegistrationViewModel(IMvxNavigationService navigationService, ILoginRepository loginService)
        {
            NavigationService = navigationService;
            _loginService = loginService;
        }

        #region Commands

        public IMvxCommand ShowLoginPageCommand
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    NavigationService.Navigate<LoginViewModel>();
                });
            }
        }

        #endregion
    }
}

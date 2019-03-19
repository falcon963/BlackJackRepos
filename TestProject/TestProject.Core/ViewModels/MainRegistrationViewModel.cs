using MvvmCross.Commands;
using MvvmCross.Navigation;
using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Constant;
using TestProject.Core.Interfaces;
using TestProject.Core.Models;

namespace TestProject.Core.ViewModels
{
    public class MainRegistrationViewModel
        : BaseViewModel
    {
        #region Fields

        private readonly IMvxNavigationService _navigationService;

        private readonly ILoginService _loginService;

        #endregion

        public MainRegistrationViewModel(IMvxNavigationService navigationService, ILoginService loginService)
        {
            _navigationService = navigationService;
            _loginService = loginService;
        }

        #region Commands

        public IMvxCommand ShowLoginPageCommand
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    _navigationService.Navigate<LoginViewModel>();
                });
            }
        }

        #endregion
    }
}

using MvvmCross.Commands;
using MvvmCross.Navigation;
using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Constants;
using TestProject.Core.Models;
using TestProject.Core.Repositories.Interfaces;

namespace TestProject.Core.ViewModels
{
    public class MainRegistrationViewModel
        : BaseViewModel
    {

        public MainRegistrationViewModel(IMvxNavigationService navigationService) : base(navigationService)
        {
            
        }

        #region Commands

        public IMvxCommand ShowLoginPageCommand
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    await NavigationService.Navigate<LoginViewModel>();
                });
            }
        }

        #endregion
    }
}

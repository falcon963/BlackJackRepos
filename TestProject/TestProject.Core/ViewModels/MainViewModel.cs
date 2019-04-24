using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using TestProject.Core.Models;
using System.Threading.Tasks;
using Plugin.SecureStorage;
using TestProject.Core.Constants;
using TestProject.Core.Repositories.Interfaces;

namespace TestProject.Core.ViewModels
{
    public class MainViewModel
        : BaseViewModel
    {
        #region Fields

        private readonly ILoginRepository _loginService;

        #endregion

        public MainViewModel(IMvxNavigationService navigationService, ILoginRepository loginService) :base(navigationService)
        {
            _loginService = loginService;
        }

        #region Commands

        public IMvxAsyncCommand ShowMenuCommand
        {
            get
            {
                    return new MvxAsyncCommand(async () =>
                    {
                        await NavigationService.Navigate<TaskListViewModel>();
                        await NavigationService.Navigate<UserProfileViewModel>();
                        await NavigationService.Navigate<UserLocationViewModel>();
                    });
            }
        }

        #endregion
    }
}

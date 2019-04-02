using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using TestProject.Core.Models;
using TestProject.Core.Interfaces;
using System.Threading.Tasks;
using Plugin.SecureStorage;
using TestProject.Core.Constant;

namespace TestProject.Core.ViewModels
{
    public class MainViewModel
        : BaseViewModel
    {
        #region Fields

        private readonly IMvxNavigationService _navigationService;

        private readonly ILoginService _loginService;

        #endregion

        public MainViewModel(IMvxNavigationService navigationService, ILoginService loginService)
        {
            _navigationService = navigationService;
            _loginService = loginService;
        }

        #region Commands

        public IMvxAsyncCommand ShowMenuCommand
        {
            get
            {
                    return new MvxAsyncCommand(async () =>
                    {
                        await _navigationService.Navigate<TaskListViewModel>();
                        await _navigationService.Navigate<UserProfileViewModel>();
                        await _navigationService.Navigate<UserLocationViewModel>();
                    });
            }
        }


        public IMvxAsyncCommand CloseMain
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    await _navigationService.Close(this);
                });
            }
        }

        #endregion
    }
}

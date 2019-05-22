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
                        try
                        {
                            await NavigationService.Navigate<TasksListViewModel>();
                            await NavigationService.Navigate<MenuViewModel>();
                        }catch(Exception ex)
                        {
                            var e = ex.InnerException;
                        }
                    });
            }
        }

        public IMvxAsyncCommand ShowAppPagesCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    try
                    {
                        await NavigationService.Navigate<TasksListViewModel>();
                        await NavigationService.Navigate<ProfileViewModel>();
                        await NavigationService.Navigate<LocationViewModel>();
                    }
                    catch (Exception ex)
                    {
                        var e = ex.InnerException;
                    }
                });
            }
        }

        #endregion
    }
}

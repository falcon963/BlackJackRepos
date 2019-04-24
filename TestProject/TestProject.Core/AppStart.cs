﻿using System;
using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Plugin.SecureStorage;
using TestProject.Core.Constants;
using TestProject.Core.Helpers.Interfaces;
using TestProject.Core.Models;
using TestProject.Core.Repositories.Interfaces;
using TestProject.Core.ViewModels;

namespace TestProject.Core
{
    public class AppStart : MvxAppStart
    {
        public AppStart(IMvxApplication app, IMvxNavigationService mvxNavigationService)
            : base(app, mvxNavigationService)
        {
            
        }

        protected override Task NavigateToFirstViewModel(object hint = null)
        {
            IAccountCheckHelper accountCheckHelper = Mvx.IoCProvider.Resolve<IAccountCheckHelper>();
            IUserHelper userHelper = Mvx.IoCProvider.Resolve<IUserHelper>();

            if (accountCheckHelper.IsSocialNetworkLogin())
            {
                return NavigationService.Navigate<MainViewModel>();
            }
            if (userHelper.IsUserLogin)
            {
                if (!accountCheckHelper.IsCheckAccountAccess())
                {
                    userHelper.IsUserLogin = false;

                    return NavigationService.Navigate<MainRegistrationViewModel>();
                }

                userHelper.UserId = accountCheckHelper.GetUserId();

                return NavigationService.Navigate<MainViewModel>();
            }
            return NavigationService.Navigate<MainRegistrationViewModel>();

        }
    }
}
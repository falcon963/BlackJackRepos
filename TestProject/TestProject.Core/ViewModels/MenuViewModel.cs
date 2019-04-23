using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Acr.UserDialogs;
using Plugin.SecureStorage;
using TestProject.Core.Constants;
using TestProject.Core.Models;
using MvvmCross.ViewModels;
using System.Threading.Tasks;
using MvvmCross.UI;
using TestProject.Core.Repositories.Interfacies;
using TestProject.Core.Helpers.Interfaces;
using TestProject.Resources;
using TestProject.Core.Colors;

namespace TestProject.Core.ViewModels
{
    public class MenuViewModel
        : BaseViewModel
    {
        #region Fields

        private readonly IUserDialogs _userDialogs;

        private readonly IUserHelper _userHelper;

        #endregion

        #region Propertys

        public ResultModel UserTask { get; set; }

        public User Profile { get; set; }

        public MvxColor MainTheme { get; set; }

        public MvxColor MenuColor
        {
            get
            {
                return AppColors.MenuColor;
            }
        }

        public MvxObservableCollection<MenuItem> MenuItems { get; set; }

        #endregion


        public MenuViewModel(IMvxNavigationService navigationService, IUserDialogs userDialogs, ILoginRepository loginService, IUserHelper userHelper) : base(navigationService)
        {
            _userDialogs = userDialogs;
            _userHelper = userHelper;
            int userId = _userHelper.UserId;
            Profile = loginService.Get(userId);
            MenuItems = new MvxObservableCollection<MenuItem>()
            {
                new MenuItem
                {
                    ItemAction = () => {
                        LogOutCommand.Execute();
                    },
                    ItemTitle = "Logout"
                },
                new MenuItem
                {
                    ItemAction = () => {
                        GetLocationCommand.Execute();
                    },
                    ItemTitle = "Map"
                },
                new MenuItem
                {
                    ItemAction = () => {
                        OpenProfileCommand.Execute();
                    },
                    ItemTitle = "Profile"
                }
            };
        }

        #region Commands

        public IMvxCommand LogOutCommand
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    var logOut = await _userDialogs.ConfirmAsync(new ConfirmConfig
                    {
                        Title = Strings.AlertMessege,
                        Message = Strings.Logout,
                        OkText = Strings.YesText,
                        CancelText = Strings.NoText
                    });
                    if (logOut)
                    {
                        _userHelper.DeleteUserStatus();
                        await NavigationService.Navigate<MainRegistrationViewModel>();
                    }
                    if (!logOut)
                    {
                        return;
                    }
                });
            }
        }

        public IMvxCommand<MenuItem> ItemSelectCommand
        {
            get
            {
                return new MvxCommand<MenuItem>((item) =>
                {
                    item.ItemAction();
                });
            }
        }

        public IMvxAsyncCommand GetLocationCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    await NavigationService.Navigate<UserLocationViewModel>();
                });
            }
        }


        public IMvxAsyncCommand OpenProfileCommand
        {
            get
            {
                return new MvxAsyncCommand(async() =>
                {
                    await NavigationService.Navigate<UserProfileViewModel>();
                });
            }
        }

        #endregion

    }
}

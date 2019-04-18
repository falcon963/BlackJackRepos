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
using TestProject.Core.Interfaces;
using MvvmCross.UI;
using TestProject.Core.Repositorys.Interfaces;
using TestProject.Core.Helpers.Interfaces;
using TestProject.Resources;

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
                return new MvxColor(0, 105, 92);
            }
        }

        public MvxObservableCollection<MenuItem> MenuItems { get; set; }

        #endregion


        public MenuViewModel(IMvxNavigationService navigationService, IUserDialogs userDialogs, ILoginRepository loginService, IUserHelper userHelper)
        {
            NavigationService = navigationService;
            _userDialogs = userDialogs;
            _userHelper = userHelper;
            int userId = _userHelper.GetUserId();
            Profile = loginService.GetDate(userId);
            MenuItems = new MvxObservableCollection<MenuItem>()
            {
                new MenuItem
                {
                    ItemAction = Enum.MenuItemAction.Logout,
                    ItemTitle = "Logout"
                },
                new MenuItem
                {
                    ItemAction = Enum.MenuItemAction.Location,
                    ItemTitle = "Map"
                },
                new MenuItem
                {
                    ItemAction = Enum.MenuItemAction.Profile,
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

        public IMvxAsyncCommand<MenuItem> ItemSelectCommand
        {
            get
            {
                return new MvxAsyncCommand<MenuItem>(async(item) =>
                {
                    var action = item.ItemAction;
                    if(action == Enum.MenuItemAction.Logout)
                    {
                        LogOutCommand.Execute();
                    }
                    if(action == Enum.MenuItemAction.Location)
                    {
                        GetLocationCommand.Execute();
                    }
                    if (action == Enum.MenuItemAction.Profile)
                    {
                        OpenProfileCommand.Execute();
                    }
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

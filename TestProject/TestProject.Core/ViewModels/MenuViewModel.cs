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
using TestProject.Core.Repositories.Interfaces;
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

        private readonly ILoginRepository _loginRepository; 

        #endregion

        #region Propertys

        public ResultModel UserTask { get; set; }

        public User Profile { get; set; }

        public MvxColor MainTheme { get; set; }

        public MvxColor MenuColor
        {
            get
            {
                return AppColors.MenuBackgroundColor;
            }
        }

        public MvxObservableCollection<MenuItem> MenuItems { get; set; }

        #endregion


        public MenuViewModel(IMvxNavigationService navigationService, IUserDialogs userDialogs, ILoginRepository loginRepository, IUserHelper userHelper) : base(navigationService)
        {
            _userDialogs = userDialogs;
            _userHelper = userHelper;
            _loginRepository = loginRepository;
        }

        public override void Prepare()
        {
            base.Prepare();

            int userId = _userHelper.UserId;
            Profile = _loginRepository.Get(userId);

            MenuItems = new MvxObservableCollection<MenuItem>()
            {
                new MenuItem
                {
                    ItemAction = () => {
                        LogOutCommand?.Execute();
                    },
                    ItemTitle = "Logout"
                },
                new MenuItem
                {
                    ItemAction = async () => {
                        await NavigationService.Navigate<UserLocationViewModel>();
                    },
                    ItemTitle = "Map"
                },
                new MenuItem
                {
                    ItemAction = async () => {
                        await NavigationService.Navigate<UserProfileViewModel>();
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
                        Message = Strings.DoYouWantLogout,
                        OkText = Strings.Yes,
                        CancelText = Strings.No
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
                    item?.ItemAction();
                });
            }
        }

        #endregion

    }
}

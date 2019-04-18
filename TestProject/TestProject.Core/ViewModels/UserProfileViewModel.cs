using Acr.UserDialogs;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.UI;
using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Constants;
using TestProject.Core.Helpers.Interfaces;
using TestProject.Core.Interfaces;
using TestProject.Core.Models;
using TestProject.Core.Repositorys.Interfaces;
using TestProject.Resources;

namespace TestProject.Core.ViewModels
{
    public class UserProfileViewModel
        :BaseViewModel
    {

        #region Fields

        private readonly ILoginRepository _loginService;
        private readonly IUserDialogs _userDialogs;
        private readonly IUserHelper _userHelper;
        private readonly ICheckNullOrWhiteSpaceHelper _checkHelper;
        private string _oldPassword;
        private string _newPassword;
        private string _confirmPassword;

        #endregion

        public UserProfileViewModel(IMvxNavigationService navigationService, ILoginRepository loginService,
            IUserDialogs userDialogs, IUserHelper userHelper, ICheckNullOrWhiteSpaceHelper checkHelper)
        {
            NavigationService = navigationService;
            _loginService = loginService;
            _userDialogs = userDialogs;
            _checkHelper = checkHelper;
            _userHelper = userHelper;
            int userId = _userHelper.GetUserId();
            Profile = _loginService.GetDate(userId);
            Background = new MvxColor(251, 192, 45);
            ConfirmColor = new MvxColor(241, 241, 241);
            OldPasswordFieldColor = new MvxColor(241, 241, 241);
        }

        #region Propertys

        public User Profile { get; set; }

        public string OldPassword
        {
            get
            {
                return _oldPassword;
            }
            set
            {
                SetProperty(ref _oldPassword, value);
                CheckEnableStatus();
                if (OldPassword != Profile.Password)
                {
                    OldPasswordFieldColor = new MvxColor(241, 241, 241);
                }
                if(OldPassword == Profile.Password && !String.IsNullOrEmpty(OldPassword) && !String.IsNullOrWhiteSpace(OldPassword))
                {
                    OldPasswordFieldColor = new MvxColor(54, 255, 47);
                }
                if (String.IsNullOrEmpty(OldPassword))
                {
                    OldPasswordFieldColor = new MvxColor(241, 241, 241);
                }
                if (String.IsNullOrWhiteSpace(OldPassword))
                {
                    OldPasswordFieldColor = new MvxColor(241, 241, 241);
                }
                if (String.IsNullOrWhiteSpace(NewPassword))
                {
                    OldPasswordFieldColor = new MvxColor(241, 241, 241);
                }
            }
        }

        public string NewPassword
        {
            get
            {
                return _newPassword;
            }
            set
            {
                SetProperty(ref _newPassword, value);
                CheckEnableStatus();
                if (NewPassword == ConfirmPassword
                    && !_checkHelper.Check2FieldsConfirm(NewPassword, ConfirmPassword))
                {
                    ConfirmColor = new MvxColor(54, 255, 47);
                }
                if(NewPassword != ConfirmPassword)
                {
                    ConfirmColor = new MvxColor(241, 241, 241);
                }
                if (!_checkHelper.Check2FieldsConfirm(NewPassword, ConfirmPassword))
                {
                    ConfirmColor = new MvxColor(241, 241, 241);
                }
            }
        }

        public string ConfirmPassword
        {
            get
            {
                return _confirmPassword;
            }
            set
            {
                SetProperty(ref _confirmPassword, value);
                CheckEnableStatus();
                if (NewPassword == ConfirmPassword
                    && !_checkHelper.Check2FieldsConfirm(NewPassword, ConfirmPassword))
                {
                    ConfirmColor = new MvxColor(54, 255, 47);
                }
                if (NewPassword != ConfirmPassword)
                {
                    ConfirmColor = new MvxColor(241, 241, 241);
                }
                if (!_checkHelper.Check2FieldsConfirm(NewPassword, ConfirmPassword))
                {
                    ConfirmColor = new MvxColor(241, 241, 241);
                }
            }
        }

        public MvxColor Background { get; set; }

        public bool PassChangeEneble { get; set; }

        public MvxColor OldPasswordFieldColor { get; set; }

        public MvxColor ConfirmColor { get; set; }

        #endregion
        #region Commands


        public IMvxAsyncCommand CloseProfileCommand
        {
            get
            {
                return new MvxAsyncCommand(async() =>
                {
                    await NavigationService.Close(this);
                });
            }
        }


        public IMvxCommand SavePasswordChangeCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    if (PassChangeEneble == true)
                    {
                        _loginService.ChangePassword(Profile.Id, NewPassword);
                        _userHelper.SetUserPassword(NewPassword);
                    }
                    if ((_checkHelper.Check3Strings(ConfirmPassword, NewPassword, OldPassword)) && OldPassword != Profile.Password)
                    {
                        var alertPass = UserDialogs.Instance.Alert(
                            new AlertConfig
                            {
                                Message = Strings.UserPassword,
                                OkText = Strings.OkText,
                            });
                        return;
                    }
                    if ((_checkHelper.Check3Strings(ConfirmPassword, NewPassword, OldPassword)) && NewPassword != ConfirmPassword)
                    {
                        var alertPass = UserDialogs.Instance.Alert(
                            new AlertConfig
                            {
                                Message = Strings.WrongPassword,
                                OkText = Strings.OkText,
                            });
                        return;
                    }
                    var alert = UserDialogs.Instance.Alert(
                            new AlertConfig
                            {
                                Message = Strings.Success,
                                OkText = Strings.OkText,
                                Title = Strings.Success
                            });
                    _loginService.ChangeImage(Profile.Id, Profile.ImagePath);
                });
            }
        }

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
                        _userHelper.DeleteUserAccessToken();
                        await NavigationService.Navigate<MainRegistrationViewModel>();
                    }
                    if (!logOut)
                    {
                        return;
                    }
                });
            }
        }


        public IMvxCommand SaveImageChangeCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    var alert = UserDialogs.Instance.Alert(
                            new AlertConfig
                            {
                                Message = Strings.Success,
                                OkText = Strings.OkText,
                                Title = Strings.Success
                            });
                    _loginService.ChangeImage(Profile.Id, Profile.ImagePath);
                });
            }
        }



        #endregion

        public void CheckEnableStatus()
        {
            if (OldPassword == Profile.Password
                    && NewPassword == ConfirmPassword
                    && _checkHelper.Check3FieldsConfirm(NewPassword, OldPassword, ConfirmPassword))
            {
                PassChangeEneble = true;
            }
        }
    }
}

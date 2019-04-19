using Acr.UserDialogs;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.UI;
using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Resources;
using System.Text;
using TestProject.Core.Constants;
using TestProject.Core.Helpers.Interfaces;
using TestProject.Core.Models;
using TestProject.Core.Repositories.Interfacies;
using TestProject.Core.Servicies.Interfacies;
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
        private readonly IValidationService _validationService;
        private string _oldPassword;
        private string _newPassword;
        private string _confirmPassword;

        #endregion

        public UserProfileViewModel(IMvxNavigationService navigationService, ILoginRepository loginService,
            IUserDialogs userDialogs, IUserHelper userHelper, IValidationService validationService)
        {
            NavigationService = navigationService;
            _loginService = loginService;
            _userDialogs = userDialogs;
            _userHelper = userHelper;
            _validationService = validationService;
            int userId = _userHelper.UserId;
            Profile = _loginService.Get(userId);
            Background = new MvxColor(251, 192, 45);
            ConfirmColor = new MvxColor(241, 241, 241);
            OldPasswordFieldColor = new MvxColor(241, 241, 241);
        }

        #region Propertys

        public User Profile { get; set; }

        [Required(ErrorMessageResourceName = "Strings.OldPasswordFieldIsEmpty", ErrorMessageResourceType = typeof(ResourceManager))]
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
            }
        }

        [Required(ErrorMessageResourceName = "Strings.PasswordFieldIsEmpty", ErrorMessageResourceType = typeof(ResourceManager))]
        [RegularExpression(@"[0-9A-Z]+.{8,}", ErrorMessageResourceName = "Strings.RegularError", ErrorMessageResourceType = typeof(ResourceManager))]
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
                var passwordValidationModel = new PasswordValidationModel { Password = NewPassword, PasswordConfirm = ConfirmPassword };
                var validationModel = _validationService.GetViewModelValidation(passwordValidationModel);
                if (validationModel.IsValid)
                {
                    ConfirmColor = new MvxColor(54, 255, 47);
                }
                if(!validationModel.IsValid)
                {
                    ConfirmColor = new MvxColor(241, 241, 241);
                }
            }
        }

        [Required(ErrorMessageResourceName = "Strings.ConfirmPasswordFieldIsEmpty", ErrorMessageResourceType = typeof(ResourceManager))]
        [Compare("UserProfileViewModel.Password", ErrorMessageResourceName = "Strings.ConfirmPasswordNotComparePassword", ErrorMessageResourceType = typeof(ResourceManager))]
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
                var passwordValidationModel = new PasswordValidationModel { Password = NewPassword, PasswordConfirm = ConfirmPassword };
                var validationModel = _validationService.GetViewModelValidation(passwordValidationModel);
                if (validationModel.IsValid)
                {
                    ConfirmColor = new MvxColor(54, 255, 47);
                }
                if (!validationModel.IsValid)
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
                    var validationModel = _validationService.GetViewModelValidation(this);
                    if (PassChangeEneble == true)
                    {
                        _loginService.ChangePassword(Profile.Id, NewPassword);
                        _userHelper.UserPassword = NewPassword;
                    }
                    if (!validationModel.IsValid && OldPassword != Profile.Password)
                    {
                        foreach (string errorMessage in validationModel.Errors) {
                            var alertPass = UserDialogs.Instance.Alert(
                                new AlertConfig
                                {
                                    Message = errorMessage,
                                    OkText = Strings.OkText,
                                });
                        }
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
                    && _validationService.GetViewModelValidation(this))
            {
                PassChangeEneble = true;
            }
        }
    }
}

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
using System.Text.RegularExpressions;
using TestProject.Core.Constants;
using TestProject.Core.Helpers.Interfaces;
using TestProject.Core.Models;
using TestProject.Core.Repositories.Interfacies;
using TestProject.Core.Servicies.Interfacies;
using TestProject.Resources;

namespace TestProject.Core.ViewModels
{
    public class RegistrationViewModel
        : BaseViewModel
    {
        #region Fields

        private readonly ILoginRepository _loginService;

        private readonly IDialogsService _dialogsService;

        private readonly IValidationService _validationService;

        private string _password;
        private string _passwordRevise;

        private MvxColor _backgroundColor;
        private MvxColor _passwordFieldColor;
        private MvxColor _loginEnebleColor;

        private readonly IUserDialogs _userDialogs;

        #endregion

        public RegistrationViewModel(IMvxNavigationService navigationService, ILoginRepository loginService,
            IUserDialogs userDialogs, IDialogsService dialogsService, IValidationService validationService)
        {
            LoginColor = new MvxColor(251, 192, 45);
            ValidateColor = new MvxColor(241, 241, 241);
            _userDialogs = userDialogs;
            NavigationService = navigationService;
            _loginService = loginService;
            _dialogsService = dialogsService;
            _validationService = validationService;
        }

        #region MvxColor

        public MvxColor LoginColor { get; set; }

        public MvxColor LoginEnebleColor { get; set; }

        public MvxColor ValidateColor { get; set; }

        #endregion

        #region Propertys

        [Required(ErrorMessageResourceName = "Strings.LoginFieldIsEmpty", ErrorMessageResourceType = typeof(ResourceManager))]
        public string Login { get; set; }

        [Required(ErrorMessageResourceName = "Strings.PasswordFieldIsEmpty", ErrorMessageResourceType = typeof(ResourceManager))]
        [RegularExpression(@"[0-9A-Z]+.{8,}", ErrorMessageResourceName = "Strings.RegularError", ErrorMessageResourceType = typeof(ResourceManager))]
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                SetProperty(ref _password, value);
                var passwordValidationModel = new PasswordValidationModel { Password = Password, PasswordConfirm = PasswordRevise };
                var validationModel = _validationService.GetViewModelValidation(passwordValidationModel);
                if (validationModel.IsValid)
                {
                    ValidateColor = new MvxColor(54, 255, 47);
                }
                if (!validationModel.IsValid)
                {
                    ValidateColor = new MvxColor(241, 241, 241);
                }
            }
        }

        [Required(ErrorMessageResourceName = "Strings.ConfirmPasswordFieldIsEmpty", ErrorMessageResourceType =typeof(ResourceManager))]
        [Compare("RegistrationViewModel.Password", ErrorMessageResourceName = "Strings.ConfirmPasswordNotComparePassword", ErrorMessageResourceType = typeof(ResourceManager))]
        public string PasswordRevise
        {
            get
            {
                return _passwordRevise;
            }
            set
            {
                SetProperty(ref _passwordRevise, value);
                var passwordValidationModel = new PasswordValidationModel { Password = Password, PasswordConfirm = PasswordRevise };
                var validationModel = _validationService.GetViewModelValidation(passwordValidationModel);
                if (validationModel.IsValid)
                {
                    ValidateColor = new MvxColor(54, 255, 47);
                }
                if (!validationModel.IsValid)
                {
                    ValidateColor = new MvxColor(241, 241, 241);
                }
            }
        }

        public bool CheckLogin
        {
            get
            {
                return _loginService.CheckValidLogin(Login);
            }
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand RegistrationCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    var valid = _loginService.CheckValidLogin(Login);
                    var validationModel = _validationService.GetViewModelValidation(this);
                    if (validationModel.IsValid)
                    {
                        foreach (string error in validationModel.Errors)
                        {
                            _dialogsService.UserDialogAlert(error);
                        }
                        return;
                    }
                    if (!valid)
                    {
                        var alert = UserDialogs.Instance.Alert(new AlertConfig
                        {
                            Message = Strings.Login,
                            OkText = Strings.OkText,
                            Title = Strings.LoginUse
                        });
                        return;
                    }
                    if (valid)
                    {
                        var user = new User()
                        {
                            Login = Login,
                            Password = Password
                        };
                        _loginService.Save(user);
                        var alert = await _userDialogs.ConfirmAsync(new ConfirmConfig
                        {
                            Message = Strings.Registrate,
                            OkText = Strings.OkText,
                            Title = Strings.Success,
                            CancelText = Strings.NoText
                        });
                        if (alert)
                        {
                            await NavigationService.Close(this);
                        }
                        if (!alert)
                        {
                            return;
                        }
                    }
                });
            }
        }

        public IMvxAsyncCommand GoOnLoginCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    await NavigationService.Close(this);
                });
            }

            #endregion
        }
    }
}

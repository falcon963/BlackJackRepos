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
using TestProject.Core.Colors;
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
            IUserDialogs userDialogs, IDialogsService dialogsService, IValidationService validationService) : base(navigationService)
        {
            _userDialogs = userDialogs;
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

        [Required(ErrorMessageResourceName = "LoginFieldIsEmpty", ErrorMessageResourceType = typeof(Strings))]
        public string Login { get; set; }

        [Required(ErrorMessageResourceName = "PasswordFieldIsEmpty", ErrorMessageResourceType = typeof(Strings))]
        [RegularExpression(@"[0-9A-Z]+.{8,}", ErrorMessageResourceName = "RegularError", ErrorMessageResourceType = typeof(Strings))]
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
                var validationModel = _validationService.Validate(passwordValidationModel);
                if (validationModel.IsValid)
                {
                    ValidateColor = AppColors.NotValidateColor;
                }
                if (!validationModel.IsValid)
                {
                    ValidateColor = AppColors.ValidateColor;
                }
            }
        }

        [Required(ErrorMessageResourceName = "ConfirmPasswordFieldIsEmpty", ErrorMessageResourceType =typeof(Strings))]
        [Compare(nameof(Password), ErrorMessageResourceName = "ConfirmPasswordNotComparePassword", ErrorMessageResourceType = typeof(Strings))]
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
                var validationModel = _validationService.Validate(passwordValidationModel);
                if (validationModel.IsValid)
                {
                    ValidateColor = AppColors.NotValidateColor;
                }
                if (!validationModel.IsValid)
                {
                    ValidateColor = AppColors.ValidateColor;
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

        public override void Prepare()
        {
            base.Prepare();
            LoginColor = AppColors.LoginColor;
            ValidateColor = AppColors.ValidateColor;
        }

        #region Commands

        public IMvxAsyncCommand RegistrationCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    var valid = _loginService.CheckValidLogin(Login);
                    var validationModel = _validationService.Validate(this);
                    if (!validationModel.IsValid)
                    {
                        foreach (string error in validationModel.Errors)
                        {
                            _dialogsService.ShowAlert(error);
                        }
                        return;
                    }
                    if (!valid)
                    {
                        _dialogsService.ShowAlert(Strings.Login);
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
                        var userChose = await _dialogsService.ShowConfirmDialogAsync(Strings.Registrate, Strings.Success);
                        if (!userChose)
                        {
                            return;
                        }

                        await NavigationService.Close(this);
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

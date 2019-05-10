using Acr.UserDialogs;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.UI;
using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using TestProject.Core.Colors;
using TestProject.Core.Constants;
using TestProject.Core.Helpers.Interfaces;
using TestProject.Core.Models;
using TestProject.Core.Repositories.Interfaces;
using TestProject.Core.Services.Interfaces;
using TestProject.LanguageResources;

namespace TestProject.Core.ViewModels
{
    public class RegistrationViewModel
        : BaseViewModel
    {
        #region Fields

        private readonly ILoginRepository _loginService;
        private readonly IDialogsService _dialogsService;
        private readonly IValidationService _validationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IUserService _userService;

        private string _password;
        private string _passwordRevise;

        #endregion

        public RegistrationViewModel(IMvxNavigationService navigationService, ILoginRepository loginService,
            IUserDialogs userDialogs, IDialogsService dialogsService, IValidationService validationService, IUserService userService) : base(navigationService)
        {
            _userDialogs = userDialogs;
            _loginService = loginService;
            _dialogsService = dialogsService;
            _validationService = validationService;
            _userService = userService;

            LoginColor = AppColors.LoginBackgroundColor;
            ValidateColor = AppColors.ValidColor;
        }

        #region MvxColor

        public MvxColor LoginColor { get; set; }

        public MvxColor LoginEnebleColor { get; set; }

        public MvxColor ValidateColor { get; set; }

        #endregion

        #region Propertys

        [Required(ErrorMessageResourceName = "LoginMustBeRequired", ErrorMessageResourceType = typeof(Strings))]
        public string Login { get; set; }

        [Required(ErrorMessageResourceName = "PasswordMustBeRequired", ErrorMessageResourceType = typeof(Strings))]
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

                var passwordValidationModel = new PasswordValidationModel
                {
                    Password = Password,
                    PasswordConfirmation = PasswordConfirmation
                };

                var validationModel = _validationService.Validate(passwordValidationModel);

                if (validationModel.IsValid)
                {
                    ValidateColor = AppColors.InvalidColor;
                }

                if (!validationModel.IsValid)
                {
                    ValidateColor = AppColors.ValidColor;
                }
            }
        }

        [Required(ErrorMessageResourceName = "ConfirmPasswordFieldIsEmpty", ErrorMessageResourceType =typeof(Strings))]
        [Compare(nameof(Password), ErrorMessageResourceName = "ConfirmPasswordMustBeComparePassword", ErrorMessageResourceType = typeof(Strings))]
        public string PasswordConfirmation
        {
            get
            {
                return _passwordRevise;
            }
            set
            {
                SetProperty(ref _passwordRevise, value);

                var passwordValidationModel = new PasswordValidationModel
                {
                    Password = Password,
                    PasswordConfirmation = PasswordConfirmation
                };

                var validationModel = _validationService.Validate(passwordValidationModel);

                if (validationModel.IsValid)
                {
                    ValidateColor = AppColors.InvalidColor;
                }

                if (!validationModel.IsValid)
                {
                    ValidateColor = AppColors.ValidColor;
                }
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
                    var isLoginFree = _userService.IsValidLogin(Login);

                    if (!isLoginFree)
                    {
                        _dialogsService.ShowAlert(Strings.LoginAlreadyUse);

                        return;
                    }

                    var validationModel = _validationService.Validate(this);

                    if (!validationModel.IsValid)
                    {
                        _dialogsService.ShowAlert(validationModel.Errors.FirstOrDefault());

                        return;
                    }

                    var user = new User()
                    {
                        Login = Login,
                        Password = Password
                    };

                    _loginService.Save(user);

                    var userConfirm = await _dialogsService.ShowConfirmDialogAsync(Strings.RegistrateSuccessfulReturnOnLoginPage, Strings.Success);

                    if (!userConfirm)
                    {
                        return;
                    }

                    await NavigationService.Close(this);

                });
            }
        }

            #endregion
    }
}

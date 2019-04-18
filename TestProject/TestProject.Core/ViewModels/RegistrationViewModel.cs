using Acr.UserDialogs;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.UI;
using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TestProject.Core.Constants;
using TestProject.Core.Helpers.Interfaces;
using TestProject.Core.Interfaces;
using TestProject.Core.Models;
using TestProject.Core.Repositorys.Interfaces;
using TestProject.Resources;

namespace TestProject.Core.ViewModels
{
    public class RegistrationViewModel
        : BaseViewModel
    {
        #region Fields

        private readonly ILoginRepository _loginService;

        private readonly IDialogsService _dialogsService;

        private readonly ICheckNullOrWhiteSpaceHelper _checkHelper;

        private string _login;
        private string _password;
        private string _passwordRevise;

        private MvxColor _backgroundColor;
        private MvxColor _passwordFieldColor;
        private MvxColor _loginEnebleColor;

        Regex _hasNumber = new Regex(@"[0-9]+");
        Regex _hasUpperChar = new Regex(@"[A-Z]+");
        Regex _hasMinimum8Chars = new Regex(@".{8,}");

        private readonly IUserDialogs _userDialogs;

        #endregion

        public RegistrationViewModel(IMvxNavigationService navigationService, ILoginRepository loginService,
            IUserDialogs userDialogs, IDialogsService dialogsService, ICheckNullOrWhiteSpaceHelper checkHelper)
        {
            LoginColor = new MvxColor(251, 192, 45);
            ValidateColor = new MvxColor(241, 241, 241);
            _userDialogs = userDialogs;
            NavigationService = navigationService;
            _loginService = loginService;
            _dialogsService = dialogsService;
            _checkHelper = checkHelper;
        }

        #region MvxColor

        public MvxColor LoginColor { get; set; }

        public MvxColor LoginEnebleColor { get; set; }

        public MvxColor ValidateColor { get; set; }

        #endregion

        #region Propertys

        public string Login
        {
            get
            {
                return _login;
            }
            set
            {
                SetProperty(ref _login, value);
                if (_loginService.CheckValidLogin(Login))
                {
                    LoginEnebleColor = new MvxColor(54, 255, 47);
                }
                if (String.IsNullOrEmpty(Login))
                {
                    LoginEnebleColor = new MvxColor(241, 241, 241);
                }
                if (!_loginService.CheckValidLogin(Login))
                {
                   LoginEnebleColor = new MvxColor(176, 14, 14);
                }
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                SetProperty(ref _password, value);
                if (Password == PasswordRevise)
                {
                    ValidateColor = new MvxColor(54, 255, 47);
                }
                if (_checkHelper.Check2Strings(Password, PasswordRevise) || Password != PasswordRevise)
                {
                    ValidateColor = new MvxColor(241, 241, 241);
                }
            }
        }

        public string PasswordRevise
        {
            get
            {
                return _passwordRevise;
            }
            set
            {
                SetProperty(ref _passwordRevise, value);
                if(Password == PasswordRevise)
                {
                    ValidateColor = new MvxColor(54, 255, 47);
                }
                if(_checkHelper.Check2Strings(Password, PasswordRevise) || Password != PasswordRevise)
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
                    if(!_hasNumber.IsMatch(Password)|| !_hasNumber.IsMatch(PasswordRevise)){
                        _dialogsService.UserDialogAlert(Strings.PasswordMustContentNumber);
                    }
                    if (!_hasUpperChar.IsMatch(Password) || !_hasUpperChar.IsMatch(PasswordRevise)){
                        _dialogsService.UserDialogAlert(Strings.PasswordMustContentUpperChar);
                    }
                    if (!_hasMinimum8Chars.IsMatch(Password) || !_hasMinimum8Chars.IsMatch(PasswordRevise)){
                        _dialogsService.UserDialogAlert(Strings.PasswordMustContent8Char);
                    }
                    if (_checkHelper.Check2Strings(Login, Password) && Password == PasswordRevise)
                    {
                        _dialogsService.UserDialogAlert(Strings.EmptyFieldRegistrateMessege);
                    }
                    if(Password != PasswordRevise)
                    {
                        _dialogsService.UserDialogAlert(Strings.WrongPassword);
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

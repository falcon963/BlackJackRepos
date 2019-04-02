using Acr.UserDialogs;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.UI;
using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TestProject.Core.Constant;
using TestProject.Core.Interfaces;
using TestProject.Core.Models;

namespace TestProject.Core.ViewModels
{
    public class RegistrationViewModel
        : BaseViewModel
    {
        #region Fields

        private readonly IMvxNavigationService _navigationService;

        private readonly ILoginService _loginService;

        private String _login;
        private String _password;
        private String _passwordRevise;
        private Boolean _checkBoxStatus;
        private User _user;

        private MvxColor _backgroundColor;
        private MvxColor _passwordFieldColor;
        private MvxColor _loginEnebleColor;

        Regex _hasNumber = new Regex(@"[0-9]+");
        Regex _hasUpperChar = new Regex(@"[A-Z]+");
        Regex _hasMinimum8Chars = new Regex(@".{8,}");

        private readonly IUserDialogs _userDialogs;

        #endregion

        public RegistrationViewModel(IMvxNavigationService navigationService, ILoginService loginService, IUserDialogs userDialogs)
        {
            LoginColor = new MvxColor(251, 192, 45);
            ValidateColor = new MvxColor(241, 241, 241);
            _userDialogs = userDialogs;
            _navigationService = navigationService;
            _loginService = loginService;
            _user = new User();
        }

        #region MvxColor

        public MvxColor LoginColor
        {
            get
            {
                return _backgroundColor;
            }
            set
            {
                SetProperty(ref _backgroundColor, value);
            }
        }

        public MvxColor LoginEnebleColor
        {
            get
            {
                return _loginEnebleColor;
            }
            set
            {
                _loginEnebleColor = value;
            }
        }

        public MvxColor ValidateColor
        {
            get
            {
                return _passwordFieldColor;
            }
            set
            {
                SetProperty(ref _passwordFieldColor, value);
            }
        }

        #endregion

        #region Propertys

        public User User
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
            }
        }

        public String Login
        {
            get
            {
                return _login;
            }
            set
            {
                SetProperty(ref _login, value);
                User.Login = _login;
                if (_loginService.CheckValidLogin(Login))
                {
                    LoginEnebleColor = new MvxColor(54, 255, 47);
                }
                if (String.IsNullOrEmpty(Login))
                {
                    LoginEnebleColor = new MvxColor(241, 241, 241);
                }
                if (!_loginService.CheckValidLogin(User.Login))
                {
                   LoginEnebleColor = new MvxColor(176, 14, 14);
                }
                CheckEnableStatus();
            }
        }

        public String Password
        {
            get
            {
                return _password;
            }
            set
            {
                SetProperty(ref _password, value);
                User.Password = _password;
                CheckEnableStatus();
                if (Password == PasswordRevise)
                {
                    ValidateColor = new MvxColor(54, 255, 47);
                }
                if (String.IsNullOrEmpty(Password) && String.IsNullOrEmpty(PasswordRevise))
                {
                    ValidateColor = new MvxColor(241, 241, 241);
                }
            }
        }

        public String PasswordRevise
        {
            get
            {
                return _passwordRevise;
            }
            set
            {
                SetProperty(ref _passwordRevise, value);
                CheckEnableStatus();
                if(Password == PasswordRevise)
                {
                    ValidateColor = new MvxColor(54, 255, 47);
                }
                if(String.IsNullOrEmpty(Password) && String.IsNullOrEmpty(PasswordRevise) || String.IsNullOrWhiteSpace(Password) && String.IsNullOrWhiteSpace(PasswordRevise) || Password != PasswordRevise)
                {
                    ValidateColor = new MvxColor(241, 241, 241);
                }
            }
        }

        public Boolean EnableStatus
        {
            get
            {
                return _checkBoxStatus;
            }
            set
            {
                SetProperty(ref _checkBoxStatus, value);
            }
        }

        public Boolean CheckLogin
        {
            get
            {
                return _loginService.CheckValidLogin(User.Login);
            }
        }

        #endregion

        public void CheckEnableStatus()
        {
            if (String.IsNullOrEmpty(User.Login)
                && String.IsNullOrEmpty(User.Password))
            {
                EnableStatus = false;
            }
            if (!String.IsNullOrEmpty(User.Login)
                && !String.IsNullOrEmpty(User.Password)
                && !String.IsNullOrWhiteSpace(User.Password))
            {
                EnableStatus = true;
            }
            if (Password != PasswordRevise)
            {
                EnableStatus = false;
            }
            if (Password == PasswordRevise 
                && !String.IsNullOrEmpty(User.Password) 
                && !String.IsNullOrWhiteSpace(User.Password))
            {
                EnableStatus = true;
            }
            if (String.IsNullOrWhiteSpace(User.Login))
            {
                EnableStatus = false;
            }
        }

        #region Commands

        public IMvxAsyncCommand RegistrationCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    var valid = _loginService.CheckValidLogin(User.Login);
                    if(!_hasNumber.IsMatch(Password)|| !_hasNumber.IsMatch(PasswordRevise)){
                        var alert = UserDialogs.Instance.Alert(
                            new AlertConfig
                            {
                                Message = MessengeFields.PasswordMustContentNumber,
                                OkText = MessengeFields.OkText,
                                Title = MessengeFields.AlertMessege
                            });
                        return;
                    }
                    if (!_hasUpperChar.IsMatch(Password) || !_hasUpperChar.IsMatch(PasswordRevise)){
                        var alert = UserDialogs.Instance.Alert(
                            new AlertConfig
                            {
                                Message = MessengeFields.PasswordMustContentUpperChar,
                                OkText = MessengeFields.OkText,
                                Title = MessengeFields.AlertMessege
                            });
                        return;
                    }
                    if (!_hasMinimum8Chars.IsMatch(Password) || !_hasMinimum8Chars.IsMatch(PasswordRevise)){
                        var alert = UserDialogs.Instance.Alert(
                            new AlertConfig
                            {
                                Message = MessengeFields.PasswordMustContent8Char,
                                OkText = MessengeFields.OkText,
                                Title = MessengeFields.AlertMessege
                            });
                        return;
                    }
                    if (String.IsNullOrEmpty(User.Login)
                        || String.IsNullOrEmpty(User.Password)
                        || String.IsNullOrWhiteSpace(User.Password)
                        || String.IsNullOrWhiteSpace(User.Login)
                        && Password == PasswordRevise)
                    {
                        var alert = UserDialogs.Instance.Alert(
                            new AlertConfig
                            {
                                Message = MessengeFields.EmptyFieldRegistrateMessege,
                                OkText = MessengeFields.OkText,
                                Title = MessengeFields.AlertMessege
                            });
                        return;
                    }
                    if(Password != PasswordRevise)
                    {
                        var alert = UserDialogs.Instance.Alert(
                            new AlertConfig
                            {
                                Message = MessengeFields.WrongPassword,
                                OkText = MessengeFields.OkText,
                                Title = MessengeFields.AlertMessege
                            });
                        return;
                    }
                    if (!valid)
                    {
                        var alert = UserDialogs.Instance.Alert(new AlertConfig
                        {
                            Message = MessengeFields.Login,
                            OkText = MessengeFields.OkText,
                            Title = MessengeFields.LoginUse
                        });
                        return;
                    }
                    if (valid)
                    {
                        _loginService.CreateUser(User);
                        var alert = await _userDialogs.ConfirmAsync(new ConfirmConfig
                        {
                            Message = MessengeFields.Registrate,
                            OkText = MessengeFields.OkText,
                            Title = MessengeFields.Success,
                            CancelText = MessengeFields.NoText
                        });
                        if (alert)
                        {
                            await _navigationService.Close(this);
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
                    await _navigationService.Close(this);
                });
            }

            #endregion
        }
    }
}

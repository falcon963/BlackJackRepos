using Acr.UserDialogs;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.UI;
using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Constant;
using TestProject.Core.Interfaces;
using TestProject.Core.Models;

namespace TestProject.Core.ViewModels
{
    public class RegistrationViewModel
        : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        private readonly ILoginRepository _loginService;

        private String _login;
        private String _password;
        private String _passwordRevise;
        private Boolean _checkBoxStatus;
        private User _user;

        private MvxColor _backgroundColor;
        private MvxColor _passwordFieldColor;

        

        public RegistrationViewModel(IMvxNavigationService navigationService, ILoginRepository loginService)
        {
            LoginColor = new MvxColor(251, 192, 45);
            ValidateColor = new MvxColor(241, 241, 241);
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
                if (Password != PasswordRevise)
                {
                    ValidateColor = new MvxColor(201, 18, 18);
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
                if(Password != PasswordRevise)
                {
                    ValidateColor = new MvxColor(201, 18, 18);
                }
                if(String.IsNullOrEmpty(Password) && String.IsNullOrEmpty(PasswordRevise))
                {
                    ValidateColor = new MvxColor(241, 241, 241);
                }
            }
        }

        public void CheckEnableStatus()
        {
            if (String.IsNullOrEmpty(User.Login)
                && String.IsNullOrEmpty(User.Password))
            {
                EnableStatus = false;
            }
            if (!String.IsNullOrEmpty(User.Login)
                && !String.IsNullOrEmpty(User.Password))
            {
                EnableStatus = true;
            }
            if (Password != PasswordRevise)
            {
                EnableStatus = false;
            }
            if (Password == PasswordRevise)
            {
                EnableStatus = true;
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

        #region Commands

        public IMvxAsyncCommand RegistrationCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    var valid = _loginService.CheckValidLogin(User.Login);
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
                        var alert = UserDialogs.Instance.Alert(new AlertConfig
                        {
                            Message = MessengeFields.Registrate,
                            OkText = MessengeFields.OkText,
                            Title = MessengeFields.Success
                        });
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

using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Interfaces;
using TestProject.Core.Models;
using MvvmCross;
using Acr.UserDialogs;
using System.Threading.Tasks;
using Plugin.SecureStorage;
using TestProject.Core.Constant;
using MvvmCross.UI;

namespace TestProject.Core.ViewModels
{
    public class LoginViewModel 
        : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        private readonly ILoginRepository _loginService;

        private readonly ITasksRepository _taskService;

        private Boolean _rememberMe;

        private Boolean _checkBoxStatus;

        private String _login;

        private String _password;

        private MvxColor _color;

        public MvxColor LoginColor
        {
            get
            {
                return _color;
            }
            set
            {
                SetProperty(ref _color, value);
            }
        }

        User _user;

        public LoginViewModel(IMvxNavigationService navigationService, ILoginRepository loginService, ITasksRepository taskService)
        {
            _loginService = loginService;
            _navigationService = navigationService;
            _taskService = taskService;
            LoginColor = new MvxColor(251, 192, 45);
            _user = new User();
            if (CrossSecureStorage.Current.GetValue(SecureConstant.Status) == "True")
            {
                Login = CrossSecureStorage.Current.GetValue(SecureConstant.Login);
                Password = CrossSecureStorage.Current.GetValue(SecureConstant.Password);
                _rememberMe = Boolean.Parse(CrossSecureStorage.Current.GetValue(SecureConstant.Status));
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
                LoginColor = new MvxColor(21, 206, 234);
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
                LoginColor = new MvxColor(28, 21, 234);
            }
        }

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

        public Boolean RememberMeStatus
        {
            get
            {
                return _rememberMe;
            }
            set
            {
                _rememberMe = value;
                if (_rememberMe == true)
                {
                    CrossSecureStorage.Current.SetValue(SecureConstant.Status, "True");
                    _loginService.SetLoginAndPassword(Login, Password);
                }
                if (_rememberMe == false)
                {
                    CrossSecureStorage.Current.DeleteKey(SecureConstant.Status);
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
        }

        #region Commands

        public IMvxAsyncCommand LoginCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    var account = _loginService.CheckAccountAccess(User.Login, User.Password);

                    if (account != null)
                    {
                        CrossSecureStorage.Current.SetValue(SecureConstant.UserId, account.Id.ToString());
                        await _navigationService.Navigate<TaskListViewModel>();
                        await _navigationService.Close(this);
                    }
                    if((account == null))
                    {
                        UserDialogs.Instance.Alert(new AlertConfig
                        {
                            Message = MessengeFields.WrongData,
                            OkText = MessengeFields.OkText,
                            Title = MessengeFields.AccountNotFound
                        });
                        return;
                    }
                });
            }
        }

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

        #endregion

    }
}

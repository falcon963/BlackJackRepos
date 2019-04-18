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
using TestProject.Core.Constants;
using MvvmCross.UI;
using Xamarin.Auth;
using TestProject.Core.Interfaces.SocialService.Google;
using TestProject.Core.Services;
using TestProject.Core.Interfaces.SocialService.Facebook;
using TestProject.Core.Repositorys.Interfaces;
using TestProject.Core.Helpers.Interfaces;
using TestProject.Resources;

namespace TestProject.Core.ViewModels
{
    public class LoginViewModel 
        : BaseViewModel
    {
        #region Fields

        private readonly ILoginRepository _loginService;
        private readonly ITasksRepository _taskService;
        private readonly IGoogleService _googleService;
        private readonly IFacebookService _facebookService;
        private readonly ICheckNullOrWhiteSpaceHelper _checkHelper;
        private readonly IUserHelper _userHelper;
        private bool _rememberMe;

        #endregion

        public LoginViewModel(IMvxNavigationService navigationService,
            ILoginRepository loginService, ITasksRepository taskService, IGoogleService googleService, IFacebookService facebookService, ICheckNullOrWhiteSpaceHelper checkHelper, IUserHelper userHelper)
        {
                _facebookService = facebookService;
                _loginService = loginService;
                NavigationService = navigationService;
                _taskService = taskService;
                _googleService = googleService;
                _checkHelper = checkHelper;
                _userHelper = userHelper;

                LoginColor = new MvxColor(251, 192, 45);

                if (_userHelper.GetUserStatus())
                {
                    Login = _userHelper.GetUserLogin();
                    Password = _userHelper.GetUserPassword();
                    _rememberMe = _userHelper.GetUserStatus();
                }      
        }

        #region Propertys

        public MvxColor LoginColor { get; set; }

        public OAuth2Authenticator Auth { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public bool IsRememberMeStatus
        {
            get
            {
                return _rememberMe;
            }
            set
            {
                _rememberMe = value;
                if (_rememberMe)
                {
                    _userHelper.SetUserStatus(true);
                    _loginService.SetLoginAndPassword(Login, Password);
                }
                if (!_rememberMe)
                {
                    _userHelper.DeleteUserStatus();
                }
            }
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand LoginCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    var account = _loginService.CheckAccountAccess(Login, Password);

                    if (account != null)
                    {
                        _userHelper.SetUserId(account.Id);
                        await NavigationService.Navigate<MainViewModel>();
                        await NavigationService.Close(this);
                    }
                    if((account == null))
                    {
                        UserDialogs.Instance.Alert(new AlertConfig
                        {
                            Message = Strings.WrongData,
                            OkText = Strings.OkText,
                            Title = Strings.AccountNotFound
                        });
                        return;
                    }
                });
            }
        }

        public IMvxAsyncCommand GoRegistrationPageCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                   await NavigationService.Navigate<RegistrationViewModel>();
                });
            }
        }

        public IMvxAsyncCommand SaveFacebookUserCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    var token = _userHelper.GetUserAccessToken();
                    if(token == null)
                    {
                        return;
                    }
                    if (token != null)
                    {
                        User user = await _facebookService.GetSocialNetworkAsync(token);
                        var id = _loginService.GetSocialAccount(user);
                        _userHelper.SetUserId(id);
                        await NavigationService.Navigate<MainViewModel>();
                        await NavigationService.Close(this);
                    }
                });
            }
        }

        public IMvxAsyncCommand SaveGoogleUserCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    var token = _userHelper.GetUserAccessToken();
                    if (token == null)
                    {
                        return;
                    }
                    if (token != null)
                    {
                        User user = await _googleService.GetSocialNetworkAsync(token);
                        var id = _loginService.GetSocialAccount(user);
                        _userHelper.SetUserId(id);
                        await NavigationService.Navigate<MainViewModel>();
                        await NavigationService.Close(this);
                    }
                });
            }
        }

        #endregion


    }
}

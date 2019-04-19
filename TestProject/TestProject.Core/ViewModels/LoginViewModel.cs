using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Interfacies;
using TestProject.Core.Models;
using MvvmCross;
using Acr.UserDialogs;
using System.Threading.Tasks;
using Plugin.SecureStorage;
using TestProject.Core.Constants;
using MvvmCross.UI;
using Xamarin.Auth;
using TestProject.Core.Interfacies.SocialService.Google;
using TestProject.Core.Servicies;
using TestProject.Core.Interfacies.SocialService.Facebook;
using TestProject.Core.Repositories.Interfacies;
using TestProject.Core.Helpers.Interfaces;
using TestProject.Resources;
using System.ComponentModel.DataAnnotations;
using System.Resources;

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
        private readonly IValidationService<LoginViewModel> _validationService;
        private readonly IDialogsService _dialogsService;
        private bool _rememberMe;

        #endregion

        public LoginViewModel(IMvxNavigationService navigationService,
            ILoginRepository loginService, ITasksRepository taskService, IGoogleService googleService, IFacebookService facebookService,
            ICheckNullOrWhiteSpaceHelper checkHelper, IUserHelper userHelper, IValidationService<LoginViewModel> validationService, IDialogsService dialogsService)
        {
                _facebookService = facebookService;
                _loginService = loginService;
                NavigationService = navigationService;
                _taskService = taskService;
                _googleService = googleService;
                _checkHelper = checkHelper;
                _userHelper = userHelper;
                _validationService = validationService;
                _dialogsService = dialogsService;

                LoginColor = new MvxColor(251, 192, 45);

                if (_userHelper.UserStatus)
                {
                    Login = _userHelper.UserLogin;
                    Password = _userHelper.UserPassword;
                    _rememberMe = _userHelper.UserStatus;
                }      
        }

        #region Properties

        public MvxColor LoginColor { get; set; }

        public OAuth2Authenticator Auth { get; set; }

        [Required(ErrorMessageResourceName = "Strings.LoginFieldIsEmpty", ErrorMessageResourceType = typeof(ResourceManager))]
        public string Login { get; set; }

        [Required(ErrorMessageResourceName = "Strings.PasswordFieldIsEmpty", ErrorMessageResourceType = typeof(ResourceManager))]
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
                    _userHelper.UserStatus = true;
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
                    if (_validationService.GetViewModelValidation(this))
                    {
                        var errorsList = _validationService.GetValidationError();
                        foreach (string error in errorsList)
                        {
                            _dialogsService.UserDialogAlert(error);
                        }
                        return;
                    }
                    var account = _loginService.CheckAccountAccess(Login, Password);
                    if (account != null)
                    {
                        _userHelper.UserId = account.Id;
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
                    var token = _userHelper.UserAccessToken;
                    if(token == null)
                    {
                        return;
                    }
                    if (token != null)
                    {
                        User user = await _facebookService.GetSocialNetworkAsync(token);
                        var id = _loginService.GetSocialAccount(user);
                        _userHelper.UserId = id;
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
                    var token = _userHelper.UserAccessToken;
                    if (token == null)
                    {
                        return;
                    }
                    if (token != null)
                    {
                        User user = await _googleService.GetSocialNetworkAsync(token);
                        var id = _loginService.GetSocialAccount(user);
                        _userHelper.UserId = id;
                        await NavigationService.Navigate<MainViewModel>();
                        await NavigationService.Close(this);
                    }
                });
            }
        }

        #endregion


    }
}

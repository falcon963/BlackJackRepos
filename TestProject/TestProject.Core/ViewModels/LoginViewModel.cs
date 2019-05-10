using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Models;
using MvvmCross;
using Acr.UserDialogs;
using System.Threading.Tasks;
using Plugin.SecureStorage;
using TestProject.Core.Constants;
using MvvmCross.UI;
using Xamarin.Auth;
using TestProject.Core.Services;
using TestProject.Core.Repositories.Interfaces;
using TestProject.Core.Helpers.Interfaces;
using TestProject.Resources;
using System.ComponentModel.DataAnnotations;
using System.Resources;
using TestProject.Core.Services.Interfaces.SocialService.Google;
using TestProject.Core.Services.Interfaces.SocialService.Facebook;
using TestProject.Core.Services.Interfaces;
using TestProject.Core.Colors;
using System.Linq;
using TestProject.LanguageResources;

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
        private readonly IUserHelper _userHelper;
        private readonly IValidationService _validationService;
        private readonly IDialogsService _dialogsService;
        private readonly IUserService _userService;
        private bool _rememberMe;

        #endregion

        public LoginViewModel(IMvxNavigationService navigationService,ILoginRepository loginService, ITasksRepository taskService, IGoogleService googleService, 
            IFacebookService facebookService, IUserHelper userHelper, IValidationService validationService, IDialogsService dialogsService, IUserService userService) : base(navigationService)
        {
                _facebookService = facebookService;
                _loginService = loginService;
                _taskService = taskService;
                _googleService = googleService;
                _userHelper = userHelper;
                _validationService = validationService;
                _dialogsService = dialogsService;
                _userService = userService;

            LoginColor = AppColors.LoginBackgroundColor;
        }

        #region Properties

        public MvxColor LoginColor { get; set; }

        public OAuth2Authenticator Auth { get; set; }

        [Required(ErrorMessageResourceName = "LoginMustBeRequired", ErrorMessageResourceType = typeof(Strings))]
        public string Login { get; set; }

        [Required(ErrorMessageResourceName = "PasswordMustBeRequired", ErrorMessageResourceType = typeof(Strings))]
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
                    _userHelper.IsUserLogin = true;
                    _userHelper.UserLogin = Login;
                    _userHelper.UserPassword = Password;
                }
            }
        }

        #endregion

        public override void Prepare()
        {
            base.Prepare();

            if (_userHelper.IsUserLogin)
            {
                Login = _userHelper.UserLogin;
                Password = _userHelper.UserPassword;
                _rememberMe = _userHelper.IsUserLogin;
            }
        }

        #region Commands

        public IMvxAsyncCommand LoginCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    var validationModel = _validationService.Validate(this);

                    if (!validationModel.IsValid)
                    {
                        _dialogsService.ShowAlert(validationModel.Errors.FirstOrDefault());

                        return;
                    }

                    var account = _loginService.GetAppRegistrateUserAccount(Login, Password);

                    if ((account == null))
                    {
                        _dialogsService.ShowAlert(Strings.WrongDataAccountNotFound);

                        return;
                    }

                    _userHelper.UserId = account.Id;

                    await NavigationService.Navigate<MainViewModel>();
                    await NavigationService.Close(this); 
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

        public IMvxAsyncCommand SignInWithFacebookCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    await SingIn(async () => await _facebookService.GetUserAsync(_userHelper.UserAccessToken));
                });
            }
        }

        public IMvxAsyncCommand SignInWithGoogleCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {                     
                    await SingIn(async () => await _googleService.GetUserAsync(_userHelper.UserAccessToken));
                });
            }
        }

        async Task SingIn(Func<Task<User>> getUser){
            var token = _userHelper.UserAccessToken;

            if (string.IsNullOrEmpty(token))
            {
                return;
            }

            User user = await getUser();

            var id = _userService.LoginInSocialAccount(user);

            await NavigationService.Navigate<MainViewModel>();
            await NavigationService.Close(this);
        }

        #endregion


    }
}

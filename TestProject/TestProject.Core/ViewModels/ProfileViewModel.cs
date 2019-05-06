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
using TestProject.Core.Colors;
using TestProject.Core.Constants;
using TestProject.Core.Helpers.Interfaces;
using TestProject.Core.Models;
using TestProject.Core.Repositories.Interfaces;
using TestProject.Core.Services.Interfaces;
using TestProject.Resources;

namespace TestProject.Core.ViewModels
{
    public class ProfileViewModel
        :BaseViewModel
    {

        #region Fields

        private readonly ILoginRepository _loginService;
        private readonly IUserDialogs _userDialogs;
        private readonly IUserHelper _userHelper;
        private readonly IValidationService _validationService;
        private readonly IDialogsService _dialogsService;
        private readonly IUserService _userService;

        private string _oldPassword;
        private string _newPassword;
        private string _confirmPassword;

        #endregion

        public ProfileViewModel(IMvxNavigationService navigationService, ILoginRepository loginService,
            IUserDialogs userDialogs, IUserHelper userHelper, IValidationService validationService, IDialogsService dialogsService, IUserService userService) : base(navigationService)
        {
            _loginService = loginService;
            _userDialogs = userDialogs;
            _userHelper = userHelper;
            _validationService = validationService;
            _dialogsService = dialogsService;
            _userService = userService;

            Background = AppColors.LoginBackgroundColor;
            ConfirmColor = AppColors.ValidColor;
            OldPasswordFieldColor = AppColors.ValidColor;
        }

        #region Propertys

        public User Profile { get; set; }

        [Required(ErrorMessageResourceName = "OldPasswordFieldIsEmpty", ErrorMessageResourceType = typeof(Strings))]
        public string OldPassword
        {
            get
            {
                return _oldPassword;
            }
            set
            {
                SetProperty(ref _oldPassword, value);
            }
        }

        [Required(ErrorMessageResourceName = "PasswordFieldIsEmpty", ErrorMessageResourceType = typeof(Strings))]
        [RegularExpression(@"[0-9A-Z]+.{8,}", ErrorMessageResourceName = "RegularError", ErrorMessageResourceType = typeof(Strings))]
        public string NewPassword
        {
            get
            {
                return _newPassword;
            }
            set
            {
                SetProperty(ref _newPassword, value);
                var passwordValidationModel = new PasswordValidationModel { Password = NewPassword, PasswordConfirmation = ConfirmPassword };
                var validationModel = _validationService.Validate(passwordValidationModel);
                if (validationModel.IsValid)
                {
                    ConfirmColor = AppColors.InvalidColor;
                }
                if(!validationModel.IsValid)
                {
                    ConfirmColor = AppColors.ValidColor;
                }
            }
        }

        [Required(ErrorMessageResourceName = "ConfirmPasswordFieldIsEmpty", ErrorMessageResourceType = typeof(Strings))]
        [Compare("UserProfileViewModel.Password", ErrorMessageResourceName = "ConfirmPasswordNotComparePassword", ErrorMessageResourceType = typeof(Strings))]
        public string ConfirmPassword
        {
            get
            {
                return _confirmPassword;
            }
            set
            {
                SetProperty(ref _confirmPassword, value);
                var passwordValidationModel = new PasswordValidationModel { Password = NewPassword, PasswordConfirmation = ConfirmPassword };
                var validationModel = _validationService.Validate(passwordValidationModel);
                if (!validationModel.IsValid)
                {
                    ConfirmColor = AppColors.InvalidColor;
                }
                if (validationModel.IsValid)
                {
                    ConfirmColor = AppColors.ValidColor;
                }
            }
        }

        public MvxColor Background { get; set; }

        public bool IsPasswordChangeConfirmed { get; set; }

        public MvxColor OldPasswordFieldColor { get; set; }

        public MvxColor ConfirmColor { get; set; }

        #endregion
        #region Commands


        public IMvxAsyncCommand CloseProfileCommand
        {
            get
            {
                return new MvxAsyncCommand(async() =>
                {
                    await NavigationService.Close(this);
                });
            }
        }


        public IMvxCommand UpdateProfileCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    var validationModel = _validationService.Validate(this);

                    if (IsPasswordChangeConfirmed)
                    {
                        _userService.ChangePassword(Profile.Id, NewPassword);

                        _userHelper.UserPassword = NewPassword;
                    }

                    if (!validationModel.IsValid && OldPassword != Profile.Password)
                    {
                         _dialogsService.ShowAlert(message: validationModel.Errors.FirstOrDefault());

                        return;
                    }

                    _dialogsService.ShowSuccessMessage(message: Strings.ChangesAccepted);
                    _userService.ChangeImage(Profile.Id, Profile.ImagePath);
                });
            }
        }

        public IMvxAsyncCommand LogOutCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    var isLoginRedirectConfirmed = await _dialogsService.ShowConfirmDialogAsync(message: Strings.DoYouWantLogout, title: Strings.AlertMessege);

                    if (isLoginRedirectConfirmed)
                    {
                        _userHelper.DeleteUserStatus();
                        _userHelper.DeleteUserAccessToken();

                        await NavigationService.Navigate<MainRegistrationViewModel>();
                    }
                });
            }
        }



        #endregion

        public override void Prepare()
        {
            base.Prepare();

            int userId = _userHelper.UserId;

            Profile = _loginService.Get(userId);
        }
    }
}

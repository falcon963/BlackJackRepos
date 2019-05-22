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
using TestProject.LanguageResources;

namespace TestProject.Core.ViewModels
{
    public class ProfileViewModel
        :BaseViewModel
    {

        #region Fields

        private readonly ILoginRepository _loginRepository;
        private readonly IUserHelper _userHelper;
        private readonly IValidationService _validationService;
        private readonly IDialogsService _dialogsService;
        private readonly IUserService _userService;
        private readonly IImagePickerService _imagePickerService;

        private User _user;
        private string _profileImage;
        private string _oldPassword;
        private string _newPassword;
        private string _confirmPassword;

        #endregion

        public ProfileViewModel(IMvxNavigationService navigationService, ILoginRepository loginRepository,
            IUserHelper userHelper, IValidationService validationService, IDialogsService dialogsService, IUserService userService, IImagePickerService imagePickerService) : base(navigationService)
        {
            _loginRepository = loginRepository;
            _userHelper = userHelper;
            _validationService = validationService;
            _dialogsService = dialogsService;
            _userService = userService;
            _imagePickerService = imagePickerService;

            Background = AppColors.LoginBackgroundColor;
            ConfirmColor = AppColors.ValidColor;
            OldPasswordFieldColor = AppColors.ValidColor;
        }

        #region Propertys

        public User Profile
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

        public string ProfileImage
        {
            get
            {
                return _profileImage;
            }
            set
            {
                _profileImage = value;
                RaisePropertyChanged(() => ProfileImage);
                Profile.ImagePath = value;
            }
        }

        [Required(ErrorMessageResourceName = nameof(Strings.OldPasswordFieldMustBeRequired), ErrorMessageResourceType = typeof(Strings))]
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

        [Required(ErrorMessageResourceName = nameof(Strings.PasswordMustBeRequired), ErrorMessageResourceType = typeof(Strings))]
        [RegularExpression(@"[0-9A-Z]+.{8,}", ErrorMessageResourceName = nameof(Strings.RegularError), ErrorMessageResourceType = typeof(Strings))]
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

        [Required(ErrorMessageResourceName = nameof(Strings.ConfirmPasswordMustBeRequired), ErrorMessageResourceType = typeof(Strings))]
        [Compare("UserProfileViewModel.Password", ErrorMessageResourceName = nameof(Strings.ConfirmPasswordMustBeComparePassword), ErrorMessageResourceType = typeof(Strings))]
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
                        _userService.Logout();

                        await NavigationService.Navigate<MainRegistrationViewModel>();
                    }
                });
            }
        }

        public IMvxAsyncCommand PickPhotoCommand
        {
            get
            {
                return new MvxAsyncCommand(async () => {

                    var imageString = await _imagePickerService.GetImageBase64();

                    if (string.IsNullOrEmpty(imageString))
                    {
                        return;
                    }

                    ProfileImage = imageString;
                });
            }
        }



        #endregion

        public override void Prepare()
        {
            base.Prepare();

            int userId = _userHelper.UserId;

            Profile = _loginRepository.Get(userId);

            ProfileImage = Profile.ImagePath;
        }
    }
}

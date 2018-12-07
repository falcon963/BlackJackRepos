using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.UI;
using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Constant;
using TestProject.Core.Interfaces;
using TestProject.Core.Models;

namespace TestProject.Core.ViewModels
{
    public class UserProfileViewModel
        :BaseViewModel
    {
        private User _profile;
        private readonly IMvxNavigationService _navigationService;
        private readonly ILoginRepository _loginService;
        private String _oldPassword;
        private String _newPassword;
        private String _confirmPassword;
        private MvxColor _background;
        private bool _eneble;
        private MvxColor _oldPasswordFieldColor;
        private MvxColor _confirmColor;

        public UserProfileViewModel(IMvxNavigationService navigationService, ILoginRepository loginService)
        {
            _navigationService = navigationService;
            _loginService = loginService;
            Int32 userId = Int32.Parse(CrossSecureStorage.Current.GetValue(SecureConstant.UserId));
            Profile = _loginService.TakeProfile(userId);
            Background = new MvxColor(251, 192, 45);
            ConfirmColor = new MvxColor(241, 241, 241);
            OldPasswordFieldColor = new MvxColor(241, 241, 241);
        }

        #region Property
        public User Profile
        {
            get
            {
                return _profile;
            }
            set
            {
                SetProperty(ref _profile, value);
            }
        }

        public String OldPassword
        {
            get
            {
                return _oldPassword;
            }
            set
            {
                SetProperty(ref _oldPassword, value);
                CheckEnableStatus();
                if (OldPassword != Profile.Password)
                {
                    OldPasswordFieldColor = new MvxColor(201, 18, 18);
                }
                if(OldPassword == Profile.Password)
                {
                    OldPasswordFieldColor = new MvxColor(54, 255, 47);
                }
                if (String.IsNullOrEmpty(OldPassword))
                {
                    OldPasswordFieldColor = new MvxColor(241, 241, 241);
                }
            }
        }

        public String NewPassword
        {
            get
            {
                return _newPassword;
            }
            set
            {
                SetProperty(ref _newPassword, value);
                CheckEnableStatus();
                if (NewPassword == ConfirmPassword
                    && !String.IsNullOrEmpty(NewPassword)
                    && !String.IsNullOrEmpty(ConfirmPassword))
                {
                    ConfirmColor = new MvxColor(54, 255, 47);
                }
                if(NewPassword != ConfirmPassword)
                {
                    ConfirmColor = new MvxColor(201, 18, 18);
                }
                if (String.IsNullOrEmpty(NewPassword) && String.IsNullOrEmpty(ConfirmPassword))
                {
                    ConfirmColor = new MvxColor(241, 241, 241);
                }
            }
        }

        public String ConfirmPassword
        {
            get
            {
                return _confirmPassword;
            }
            set
            {
                SetProperty(ref _confirmPassword, value);
                CheckEnableStatus();
                if (NewPassword == ConfirmPassword
                    && !String.IsNullOrEmpty(NewPassword)
                    && !String.IsNullOrEmpty(ConfirmPassword))
                {
                    ConfirmColor = new MvxColor(54, 255, 47);
                }
                if (NewPassword != ConfirmPassword)
                {
                    ConfirmColor = new MvxColor(201, 18, 18);
                }
                if (String.IsNullOrEmpty(NewPassword) && String.IsNullOrEmpty(ConfirmPassword))
                {
                    ConfirmColor = new MvxColor(241, 241, 241);
                }
            }
        }

        public MvxColor Background
        {
            get
            {
                return _background;
            }
            set
            {
                SetProperty(ref _background, value);
            }
        }

        public Boolean PassChangeEneble
        {
            get
            {
                return _eneble;
            }
            set
            {
                SetProperty(ref _eneble, value);
            }
        }

        public MvxColor OldPasswordFieldColor
        {
            get
            {
                return _oldPasswordFieldColor;
            }
            set
            {
                SetProperty(ref _oldPasswordFieldColor, value);
            }
        }

        public MvxColor ConfirmColor
        {
            get
            {
                return _confirmColor;
            }
            set
            {
                SetProperty(ref _confirmColor, value);
            }
        }
        #endregion
        #region Command


        public IMvxAsyncCommand CloseProfileCommand
        {
            get
            {
                return new MvxAsyncCommand(async() =>
                {
                    await _navigationService.Close(this);
                });
            }
        }


        public IMvxCommand SavePasswordChangeCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    _loginService.ChangePassword(Profile.Id, NewPassword);
                    CrossSecureStorage.Current.SetValue(SecureConstant.Password, NewPassword);
                });
            }
        }


        public IMvxCommand SaveImageChangeCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    _loginService.ChangeImage(Profile.Id, Profile.ImagePath);
                });
            }
        }



        #endregion

        public void CheckEnableStatus()
        {
            if (OldPassword == Profile.Password
                    && NewPassword == ConfirmPassword
                    && !String.IsNullOrEmpty(NewPassword)
                    && !String.IsNullOrEmpty(ConfirmPassword))
            {
                PassChangeEneble = true;
            }
        }
    }
}

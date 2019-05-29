using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Acr.UserDialogs;
using Plugin.SecureStorage;
using TestProject.Core.Constants;
using TestProject.Core.Models;
using MvvmCross.ViewModels;
using System.Threading.Tasks;
using MvvmCross.UI;
using TestProject.Core.Repositories.Interfaces;
using TestProject.Core.Helpers.Interfaces;
using TestProject.Core.Colors;
using TestProject.Core.Services.Interfaces;
using TestProject.LanguageResources;
using MvvmCross.Plugin.Messenger;
using TestProject.Core.Messengers;

namespace TestProject.Core.ViewModels
{
    public class MenuViewModel
        : BaseViewModel
    {
        #region Fields

        private readonly IUserHelper _userHelper;

        private readonly ILoginRepository _loginRepository;

        private readonly IDialogsService _dialogsService;

        private readonly MvxSubscriptionToken _token;

        private string _profileImage;

        #endregion

        #region Propertys

        public string UserName { get; set; }

        public MvxColor MainTheme { get; set; }

        public MvxColor MenuColor
        {
            get
            {
                return AppColors.MenuBackgroundColor;
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
            }
        }

        public MvxObservableCollection<MenuItem> MenuItems { get; set; }

        #endregion


        public MenuViewModel(IMvxNavigationService navigationService, ILoginRepository loginRepository, IUserHelper userHelper, IDialogsService dialogsService, IMvxMessenger mvxMessenger) : base(navigationService)
        {
            _userHelper = userHelper;
            _loginRepository = loginRepository;
            _dialogsService = dialogsService;
            _token = mvxMessenger.Subscribe<ImageMessage>(OnImageMessage);



            #region InitObservableCollection
            MenuItems = new MvxObservableCollection<MenuItem>()
            {
                new MenuItem
                {
                    ItemAction = async () => {

                        bool isLoginRedirectConfirmed = await _dialogsService.ShowConfirmDialogAsync(message: Strings.DoYouWantLogout, title: Strings.AlertMessege);

                        if (isLoginRedirectConfirmed)
                            {
                                _userHelper.DeleteUserStatus();

                                _userHelper.DeleteUserAccessToken();

                                await NavigationService.Navigate<MainRegistrationViewModel>();
                            }
                    },
                    ItemTitle = Strings.Logout
                },
                new MenuItem
                {
                    ItemAction = async () => {
                        await NavigationService.Navigate<LocationViewModel>();
                    },
                    ItemTitle = Strings.Map
                },
                new MenuItem
                {
                    ItemAction = async () => {
                        await NavigationService.Navigate<ProfileViewModel>();
                    },
                    ItemTitle = Strings.Profile
                }
            };
            #endregion
        }

        private void OnImageMessage(ImageMessage imageMessage)
        {
            ProfileImage = imageMessage.ImageBase64;
        }

        public override void Prepare()
        {
            base.Prepare();

            int userId = _userHelper.UserId;
            User profile = _loginRepository.Get(userId);
            UserName = profile.Login;
            ProfileImage = profile.ImagePath;
        }

        #region Commands

        public IMvxCommand<MenuItem> ItemSelectCommand
        {
            get
            {
                return new MvxCommand<MenuItem>((item) =>
                {
                    item?.ItemAction?.Invoke();
                });
            }
        }

        #endregion

    }
}

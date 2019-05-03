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
using TestProject.Resources;
using TestProject.Core.Colors;
using TestProject.Core.Servicies.Interfaces;

namespace TestProject.Core.ViewModels
{
    public class MenuViewModel
        : BaseViewModel
    {
        #region Fields

        private readonly IUserDialogs _userDialogs;

        private readonly IUserHelper _userHelper;

        private readonly ILoginRepository _loginRepository;

        private readonly IDialogsService _dialogsService;

        #endregion

        #region Propertys

        public ResultModel UserTask { get; set; }

        public User Profile { get; set; }

        public MvxColor MainTheme { get; set; }

        public MvxColor MenuColor
        {
            get
            {
                return AppColors.MenuBackgroundColor;
            }
        }

        public MvxObservableCollection<MenuItem> MenuItems { get; set; }

        #endregion


        public MenuViewModel(IMvxNavigationService navigationService, IUserDialogs userDialogs, ILoginRepository loginRepository, IUserHelper userHelper, IDialogsService dialogsService) : base(navigationService)
        {
            _userDialogs = userDialogs;
            _userHelper = userHelper;
            _loginRepository = loginRepository;
            _dialogsService = dialogsService;

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

        public override void Prepare()
        {
            base.Prepare();

            int userId = _userHelper.UserId;
            Profile = _loginRepository.Get(userId);
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

using System;
using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Plugin.SecureStorage;
using TestProject.Core.Constants;
using TestProject.Core.Helpers.Interfaces;
using TestProject.Core.Models;
using TestProject.Core.Repositories.Interfacies;
using TestProject.Core.ViewModels;

namespace TestProject.Core
{
    public class AppStart : MvxAppStart
    {
        private readonly IAccountCheckHelper _accountCheckHelper;
        private readonly IUserHelper _userHelper;

        public AppStart(IMvxApplication app, IMvxNavigationService mvxNavigationService)
            : base(app, mvxNavigationService)
        {
            _accountCheckHelper = Mvx.IoCProvider.Resolve<IAccountCheckHelper>();
            _userHelper = Mvx.IoCProvider.Resolve<IUserHelper>();
        }

        protected override Task NavigateToFirstViewModel(object hint = null)
        {

            if(_accountCheckHelper.IsSocialNetworkLogin())
            {
                return NavigationService.Navigate<MainViewModel>();
            }
            if (_userHelper.IsUserLogin)
            {
                if (!_accountCheckHelper.IsCheckAccountAccess())
                {
                    _userHelper.IsUserLogin = false;
                    return NavigationService.Navigate<MainRegistrationViewModel>();
                }

                _userHelper.UserId = _accountCheckHelper.GetUserId();
                return NavigationService.Navigate<MainViewModel>();
            }
            return NavigationService.Navigate<MainRegistrationViewModel>();

        }
    }
}
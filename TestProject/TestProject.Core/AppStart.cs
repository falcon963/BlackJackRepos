using System;
using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Plugin.SecureStorage;
using TestProject.Core.Constants;
using TestProject.Core.Helpers.Interfaces;
using TestProject.Core.Interfaces;
using TestProject.Core.Models;
using TestProject.Core.Repositorys.Interfaces;
using TestProject.Core.ViewModels;

namespace TestProject.Core
{
    public class AppStart : MvxAppStart
    {
        public AppStart(IMvxApplication app, IMvxNavigationService mvxNavigationService)
            : base(app, mvxNavigationService)
        {
        }

        protected override Task NavigateToFirstViewModel(object hint = null)
        {
            var _pageCheck = Mvx.IoCProvider.Resolve<IStartPageCheckHelper>();

            if(_pageCheck.SocialNetworkLogin())
            {
                return NavigationService.Navigate<MainViewModel>();
            }
            if (_pageCheck.AccountStatus())
            {
                if (_pageCheck.CheckAccountAccess())
                {
                    CrossSecureStorage.Current.SetValue(SecureConstant.Status, false.ToString());
                    return NavigationService.Navigate<MainRegistrationViewModel>();
                }

                CrossSecureStorage.Current.SetValue(SecureConstant.UserId, _pageCheck.SetUserId().ToString());
                return NavigationService.Navigate<MainViewModel>();
            }
            return NavigationService.Navigate<MainRegistrationViewModel>();

        }
    }
}
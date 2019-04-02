using System;
using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Plugin.SecureStorage;
using TestProject.Core.Constant;
using TestProject.Core.Interfaces;
using TestProject.Core.Models;
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
            var _loginService = Mvx.IoCProvider.Resolve<ILoginService>();

            var value = CrossSecureStorage.Current.GetValue(SecureConstant.Status);
            if(CrossSecureStorage.Current.GetValue(SecureConstant.AccessToken) != null)
            {
                return NavigationService.Navigate<MainViewModel>();
            }
            if (Convert.ToBoolean(value) == true)
            {
                User user = new User();
                var login = CrossSecureStorage.Current.GetValue(SecureConstant.Login);
                var password = CrossSecureStorage.Current.GetValue(SecureConstant.Password);
                user = _loginService.CheckAccountAccess(login, password);
                if (user == null)
                {
                    CrossSecureStorage.Current.SetValue(SecureConstant.Status, false.ToString());
                    return NavigationService.Navigate<MainRegistrationViewModel>();
                }
                if (user != null)
                {
                    CrossSecureStorage.Current.SetValue(SecureConstant.UserId, user.Id.ToString());
                    return NavigationService.Navigate<MainViewModel>();
                }
            }
            return NavigationService.Navigate<MainRegistrationViewModel>();

        }
    }
}
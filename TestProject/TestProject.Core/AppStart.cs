using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System.Threading.Tasks;
using TestProject.Core.Helpers.Interfaces;
using TestProject.Core.ViewModels;

namespace TestProject.Core
{
    public class AppStart : MvxAppStart
    {
        IAccountCheckHelper _accountCheckHelper;
        IUserHelper _userHelper;

        public AppStart(IMvxApplication app, IMvxNavigationService mvxNavigationService, IAccountCheckHelper accountCheckHelper, IUserHelper userHelper)
            : base(app, mvxNavigationService)
        {
            _accountCheckHelper = accountCheckHelper;
            _userHelper = userHelper;
        }

        protected override Task NavigateToFirstViewModel(object hint = null)
        {

            if (_accountCheckHelper.IsSocialNetworkLogin())
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
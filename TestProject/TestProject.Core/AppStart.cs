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
            var _taskService = Mvx.IoCProvider.Resolve<ITaskService>();

            if (CrossSecureStorage.Current.GetValue(SecureConstant.status) == "True")
            {
                User user = new User();
                Task.Factory.StartNew(async() =>
                {
                    user = await _taskService.CheckAccountAccess(CrossSecureStorage.Current.GetValue(SecureConstant.login), CrossSecureStorage.Current.GetValue(SecureConstant.password));
                }).ContinueWith(task =>
                {
                    return NavigationService.Navigate<MainViewModel, int>(user.Id);
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            return  NavigationService.Navigate<LoginViewModel>();

        }
    }
}
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using TestProject.Core.Models;
using TestProject.Core.Interfaces;
using System.Threading.Tasks;
using Plugin.SecureStorage;
using TestProject.Core.Constant;

namespace TestProject.Core.ViewModels
{
    public class MainViewModel
        : BaseViewModel
    {
        #region Fields

        private readonly IMvxNavigationService _navigationService;

        private readonly ILoginRepository _loginService;

        #endregion

        public MainViewModel(IMvxNavigationService navigationService, ILoginRepository loginService)
        {
            _navigationService = navigationService;
            _loginService = loginService;
        }

        #region Commands

        public IMvxAsyncCommand ShowMenuCommand
        {
            get
            {
                    return new MvxAsyncCommand(async () =>
                    {
                        var value = CrossSecureStorage.Current.GetValue(SecureConstant.Status);
                        if (value == "True")
                        {
                            User user = new User();
                            var login = CrossSecureStorage.Current.GetValue(SecureConstant.Login);
                            var password = CrossSecureStorage.Current.GetValue(SecureConstant.Password);
                            user = _loginService.CheckAccountAccess(login, password);
                            if(user == null)
                            {
                                CrossSecureStorage.Current.SetValue(SecureConstant.Status, false.ToString());
                                await _navigationService.Navigate<LoginViewModel>();
                            }
                            if (user != null)
                            {
                                CrossSecureStorage.Current.SetValue(SecureConstant.UserId, user.Id.ToString());
                                await _navigationService.Navigate<TaskListViewModel>();
                            }
                        }
                        if (value != "True")
                        {
                                await _navigationService.Navigate<LoginViewModel>();
                        }
                    });
            }
        }

        public IMvxAsyncCommand CloseMain
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    await _navigationService.Close(this);
                });
            }
        }

        #endregion
    }
}

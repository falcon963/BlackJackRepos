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
    public class MainViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        private readonly ITaskService _taskService;


        public MainViewModel(IMvxNavigationService navigationService, ITaskService taskService)
        {
            _navigationService = navigationService;
            _taskService = taskService;
        }


        #region Commands

        public IMvxAsyncCommand ShowMenuCommand
        {
            get
            {
                    return new MvxAsyncCommand(async () =>
                    {
                        var value = CrossSecureStorage.Current.GetValue(SecureConstant.status);
                        if (value == "True")
                        {
                            User user = new User();
                            var login = CrossSecureStorage.Current.GetValue(SecureConstant.login);
                            var password = CrossSecureStorage.Current.GetValue(SecureConstant.password);
                            user = await _taskService.CheckAccountAccess(login, password);
                            if(user == null)
                            {
                                CrossSecureStorage.Current.SetValue(SecureConstant.status, false.ToString());
                                await _navigationService.Navigate<LoginViewModel>();
                            }
                            if (user != null)
                            {
                                var taskToNavigate = new ResultModel { Changes = new UserTask { UserId = user.Id } };
                                await _navigationService.Navigate<TaskListViewModel, ResultModel>(taskToNavigate);
                            }
                        }
                        if (value != "True")
                        {
                            await _navigationService.Navigate<LoginViewModel>();
                        }
                    });
            }
        }

        #endregion
    }
}

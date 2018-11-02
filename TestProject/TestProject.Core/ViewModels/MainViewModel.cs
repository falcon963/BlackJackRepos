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
                        if (CrossSecureStorage.Current.GetValue(SecureConstant.status) == "True")
                        {
                            User user = new User();
                            user = await _taskService.CheckAccountAccess(CrossSecureStorage.Current.GetValue(SecureConstant.login), CrossSecureStorage.Current.GetValue(SecureConstant.password));
                            var taskToNavigate = new ResultModel { Changes = new UserTask { UserId = user.Id } };
                            await _navigationService.Navigate<TaskListViewModel, ResultModel>(taskToNavigate);
                        }
                        if (CrossSecureStorage.Current.GetValue(SecureConstant.status) != "True")
                        {
                            await _navigationService.Navigate<LoginViewModel>();
                        }
                    });
            }
        }

        #endregion
    }
}

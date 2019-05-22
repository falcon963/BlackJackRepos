using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using TestProject.Core.Models;
using System.Threading.Tasks;
using SQLitePCL;
using System.Linq;
using Plugin.SecureStorage;
using TestProject.Core.Constants;
using Acr.UserDialogs;
using System.Reflection;
using MvvmCross.UI;
using TestProject.Core.Repositories.Interfaces;
using TestProject.Core.Helpers.Interfaces;
using TestProject.Core.Enums;
using TestProject.Core.Colors;

namespace TestProject.Core.ViewModels
{
    public class TasksListViewModel
        : BaseViewModel
        {

        #region Fields

        private readonly ITasksRepository _taskService;
        private readonly IUserDialogs _userDialogs;
        private readonly IUserHelper _userHelper;

        #endregion

        public TasksListViewModel(IMvxNavigationService navigationService, ITasksRepository taskService, IUserDialogs userDialogs, IUserHelper userHelper) : base(navigationService)
        {
            Tasks = new MvxObservableCollection<UserTask>();

            _taskService = taskService;
            _userDialogs = userDialogs;
            _userHelper = userHelper;
        }

        #region Properties

        public ResultModel UserTask { get; set; }

        public MvxObservableCollection<UserTask> Tasks { get; set; }

        public bool IsRefreshing { get; set; }

        #endregion

        public override Task Initialize()
        {
            UserTaskInitialize();
            return base.Initialize();
        }

        public MvxColor TasksListColor
        {
            get
            {
                return AppColors.AppThemeColor;
            }
        }

        public async Task UserTaskInitialize()
        {
            var userId = _userHelper.UserId;

            var list = _taskService.GetTasksList(userId);

            Tasks.AddRange(list);
        }


        #region Commands

        public IMvxCommand<UserTask> DeleteTaskCommand
        {
            get
            {
                return new MvxCommand<UserTask>((task) =>
                {
                    _taskService.Delete(task);
                });
            }
        }

        public IMvxAsyncCommand CreateTaskCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    ResultModel result = await NavigationService.Navigate<TaskViewModel, int, ResultModel>(0);

                    if (result == null)
                    {
                        return;
                    }

                    if (result.Result == UserTaskResult.Saved)
                    {
                        Tasks.Add(result.Changes);
                    }
                });

            }
        }

        public IMvxCommand RefreshTaskCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    IsRefreshing = true;

                    var userId = _userHelper.UserId;

                    List<UserTask> list = _taskService.GetTasksList(userId).ToList();

                    Tasks.ReplaceWith(list);

                    IsRefreshing = false;
                    RaisePropertyChanged(() => IsRefreshing);
                });

            }
        }

        public IMvxCommand<UserTask> ItemSelectedCommand
        {
            get
            {
                return new MvxCommand<UserTask>(async (item) =>
                {
                    var result = await NavigationService.Navigate<TaskViewModel, int, ResultModel>(item.Id);

                    if (result == null)
                    {
                        return;
                    }

                    if(result.Result == UserTaskResult.Deleted)
                    {
                        var delete = Tasks.FirstOrDefault(p => p.Id == result.Changes.Id);

                        if(delete == null)
                        {
                            return;
                        }

                        Tasks.Remove(delete);

                        return;
                    }

                    if (result.Result == UserTaskResult.Saved)
                    {
                        var modelToUpdate = Tasks.FirstOrDefault(p => p.Id == result.Changes.Id);
                        var index = Tasks.IndexOf(modelToUpdate);

                        Tasks[index] = result.Changes;
                    }
                });
            }
        }

        #endregion

    }
}

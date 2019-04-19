﻿using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using TestProject.Core.Models;
using TestProject.Core.Interfacies;
using System.Threading.Tasks;
using SQLitePCL;
using System.Linq;
using Plugin.SecureStorage;
using TestProject.Core.Constants;
using Acr.UserDialogs;
using System.Reflection;
using MvvmCross.UI;
using TestProject.Core.Repositories.Interfacies;
using TestProject.Core.Helpers.Interfaces;
using TestProject.Core.Enums;

namespace TestProject.Core.ViewModels
{
    public class TaskListViewModel
        : BaseViewModel
        {

        #region Fields

        private readonly ITasksRepository _taskService;
        private readonly IUserDialogs _userDialogs;
        private readonly IUserHelper _userHelper;

        #endregion

        public TaskListViewModel(IMvxNavigationService navigationService, ITasksRepository taskService, IUserDialogs userDialogs, IUserHelper userHelper)
        {
            ListOfTasks = new MvxObservableCollection<UserTask>();
            NavigationService = navigationService;
            _taskService = taskService;
            _userDialogs = userDialogs;
            _userHelper = userHelper;
        }

        #region Properties

        public ResultModel UserTask { get; set; }

        public MvxObservableCollection<UserTask> ListOfTasks { get; set; }

        public virtual bool IsRefreshing { get; set; }

        #endregion

        public override Task Initialize()
        {
            Task.Run(async () =>
            {
                await UserTaskInitialize();
            });
            return base.Initialize();
        }

        public MvxColor TasksListColor
        {
            get
            {
                return new MvxColor(251, 192, 45);
            }
        }

        public async Task UserTaskInitialize()
        {
            var userId = _userHelper.UserId;
            List<UserTask> list = _taskService.GetUserTasks(userId);
            foreach (var item in list)
            {
                ListOfTasks.Add(new UserTask
                {
                    Id = item.Id,
                    UserId = item.UserId,
                    ImagePath = item.ImagePath,
                    Title = item.Title,
                    Status = item.Status
                });
            }
        }

        

        
        public void UserTaskProcess(List<UserTask> tasks)
        {
            ListOfTasks = new MvxObservableCollection<UserTask>(tasks);
        }




        #region Commands

        public IMvxCommand<UserTask> DeleteTaskCommand
        {
            get
            {
                return new MvxCommand<UserTask>((task) =>
                {
                    _taskService.SwipeTaskDelete(task);
                });
            }
        }

        public IMvxAsyncCommand ShowTaskCommand
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
                        ListOfTasks.Add(result.Changes);
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
                    List<UserTask> list = _taskService.GetUserTasks(userId);
                    var listTasks = new List<UserTask>();
                    foreach (var item in list)
                    {
                        listTasks.Add(new UserTask
                        {
                            Id = item.Id,
                            UserId = item.UserId,
                            ImagePath = item.ImagePath,
                            Title = item.Title,
                            Status = item.Status
                        });
                    }
                    ListOfTasks.ReplaceWith(listTasks);

                    IsRefreshing = false;
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
                        var delete = ListOfTasks.FirstOrDefault(p => p.Id == result.Changes.Id);
                        if(delete == null)
                        {
                            return;
                        }
                        ListOfTasks.Remove(delete);
                        return;
                    }
                    if(result.Result == UserTaskResult.NotChanged)
                    {
                        return;
                    }
                    if (result.Result == UserTaskResult.Saved)
                    {
                        var modelToUpdate = ListOfTasks.FirstOrDefault(p => p.Id == result.Changes.Id);
                        var index = ListOfTasks.IndexOf(modelToUpdate);
                        ListOfTasks[index] = result.Changes;
                    }
                });
            }
        }

        #endregion

    }
}

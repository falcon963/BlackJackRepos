using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using TestProject.Core.Models;
using TestProject.Core.Interfaces;
using System.Threading.Tasks;
using SQLitePCL;
using System.Linq;
using Plugin.SecureStorage;
using TestProject.Core.Constant;
using Acr.UserDialogs;

namespace TestProject.Core.ViewModels
{
    public class TaskListViewModel: BaseViewModel<ResultModel>
        {
        private readonly IMvxNavigationService _navigationService;
        private ITaskService _taskService;
        private bool _isRefreshing;
        private Int32 _userId;
        private readonly IUserDialogs _userDialogs;

        private MvxObservableCollection<UserTask> _listOfTasks;

        public TaskListViewModel(IMvxNavigationService navigationService, ITaskService taskService, IUserDialogs userDialogs)
        {
            _taskService = taskService;
            _navigationService = navigationService;
            _userDialogs = userDialogs;
            _listOfTasks = new MvxObservableCollection<UserTask>();
        }

        public override  Task Initialize()
        {
            MvxObservableCollection<UserTask> result = new MvxObservableCollection<UserTask>();
            Task.Factory.StartNew(async () =>
            {
                result = await UserTaskInitialize();
                ListOfTasks = result;
            });
            //).ContinueWith(task =>
            //{
            //    UserTaskProcess(result);
            //}, TaskScheduler.FromCurrentSynchronizationContext());
            return base.Initialize();
        }

        public async Task<MvxObservableCollection<UserTask>> UserTaskInitialize()
        {
            var list = await _taskService.GetTasksAsync(_userId);
            foreach (var item in list)
            {
                _listOfTasks.Add(item);
            }
            return _listOfTasks;
        }

        public virtual bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                RaisePropertyChanged(() => IsRefreshing);
            }
        }

        
        public void UserTaskProcess(List<UserTask> tasks)
        {
            ListOfTasks = new MvxObservableCollection<UserTask>(tasks);
        }


        public MvxObservableCollection<UserTask> ListOfTasks
        {
            get
            {
                return _listOfTasks;
            }
            set
            {
                SetProperty(ref _listOfTasks, value);
            }
        }

        public MvxNotifyTask FetchTasksTask { get; private set; }

        #region Commands

        public IMvxAsyncCommand TakeTasksCommand
        {
            get
            {
                return new MvxAsyncCommand(async() =>
                {
                    var result = await _taskService.GetTasksAsync(_userId);
                });
            }
        }

        public IMvxAsyncCommand<ResultModel> ShowTaskCommand
        {
            get
            {
                return new MvxAsyncCommand<ResultModel>(async(task) =>
                {
                    task = new ResultModel
                    {
                        Changes = new UserTask {UserId = _userId},
                        Result = Enum.UserTaskResult.Save
                    };
                    var result = await _navigationService.Navigate<TaskViewModel, ResultModel, ResultModel>(task);
                    if (result == null)
                    {
                        return;
                    }
                    if (result.Result == Enum.UserTaskResult.Save)
                    {
                        ListOfTasks.Add(task.Changes);
                    }
                });

            }
        }

        public IMvxAsyncCommand RefreshTaskCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    IsRefreshing = true;

                    var result = await _taskService.RefreshUserTasks(_userId);

                    ListOfTasks = new MvxObservableCollection<UserTask>(result);

                    IsRefreshing = false;
                });

            }
        }

        public IMvxCommand<UserTask> ItemSelectedCommand
        {
            get
            {
                return new MvxCommand<UserTask>(async (UserTask task) =>
                {
                    var taskToNavigate = new ResultModel { Result = Enum.UserTaskResult.Update, Changes = new UserTask {Id=task.Id, Note=task.Note,Title=task.Title, Status=task.Status, UserId = _userId, ImagePath = task.ImagePath} };

                    var result = await _navigationService.Navigate<TaskViewModel, ResultModel, ResultModel>(taskToNavigate);
                    if (result == null)
                    {
                        return;
                    }
                    if(result.Result == Enum.UserTaskResult.Delete)
                    {
                        var delete = ListOfTasks.Select(p => p).Where(p => p.Id == result.Changes.Id).FirstOrDefault();
                        ListOfTasks.Remove(delete);
                    }
                    if(result.Result == Enum.UserTaskResult.UnChangeunchanged)
                    {
                        return;
                    }
                    if (result.Result == Enum.UserTaskResult.Update)
                    {
                        var modelToUpdate = ListOfTasks.Select(p => p).Where(p => p.Id == result.Changes.Id).FirstOrDefault();
                        ListOfTasks[ListOfTasks.IndexOf(modelToUpdate)] = result.Changes;
                    }
                });
            }
        }

        public IMvxCommand LogOutCommand
        {
            get
            {
                return new MvxCommand(async() =>
                {
                    var logOut = await _userDialogs.ConfirmAsync(new ConfirmConfig
                    {
                        Title = "Alert Messege",
                        Message = "Do you want logout?",
                        OkText = "Yes",
                        CancelText = "No"
                    });
                    if (logOut)
                    {
                        CrossSecureStorage.Current.DeleteKey(SecureConstant.status);
                        await _navigationService.Navigate<LoginViewModel>();
                    }
                    if (!logOut)
                    {
                        return;
                    }
                });
            }
        }
        

        #endregion

        public override void Prepare(ResultModel model)
        {
            _userId = model.Changes.UserId;
        }
    }
}

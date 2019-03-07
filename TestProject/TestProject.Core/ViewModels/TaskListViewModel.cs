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
using System.Reflection;
using MvvmCross.UI;

namespace TestProject.Core.ViewModels
{
    public class TaskListViewModel
        : BaseViewModel
        {

        #region Fields

        private readonly IMvxNavigationService _navigationService;
        private readonly ITasksRepository _taskService;
        private bool _isRefreshing;
        private ResultModel _userTask;
        private readonly IUserDialogs _userDialogs;

        private MvxObservableCollection<UserTask> _listOfTasks;

        #endregion

        public TaskListViewModel(IMvxNavigationService navigationService, ITasksRepository taskService, IUserDialogs userDialogs)
        {
            _listOfTasks = new MvxObservableCollection<UserTask>();
            ListOfTasks = new MvxObservableCollection<UserTask>();
            _navigationService = navigationService;
            _taskService = taskService;
            _userDialogs = userDialogs;
        }

        #region Propertys

        public ResultModel UserTask
        {
            get
            {
                return _userTask;
            }
            set
            {
                SetProperty(ref _userTask, value);
            }
        }

        public virtual bool IsRefreshing
        {
            get
            {
                return _isRefreshing;
            }
            set
            {
                _isRefreshing = value;
                RaisePropertyChanged(() => IsRefreshing);
            }
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

        #endregion

        public override  Task Initialize()
        {
            MvxObservableCollection<UserTask> result = new MvxObservableCollection<UserTask>();
            Task.Factory.StartNew(async () =>
            {
                result = await UserTaskInitialize();
                ListOfTasks = result;
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

        public async Task<MvxObservableCollection<UserTask>> UserTaskInitialize()
        {
            Int32 userId = Int32.Parse(CrossSecureStorage.Current.GetValue(SecureConstant.UserId));
            List<Int32> list = _taskService.GetUserTasksIdAsync(userId);
            foreach (var item in list)
            {
                var task = _taskService.GetUserTaskAsync(item);
                ListOfTasks.Add(new UserTask
                {
                    Id = task.Id,
                    UserId = task.UserId,
                    ImagePath = task.ImagePath,
                    Title = task.Title,
                    Status = task.Status
                });
            }
            return ListOfTasks;
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

        public IMvxAsyncCommand GetLocationCommand
        {
            get
            {
                return new MvxAsyncCommand(async() =>
                {
                   await _navigationService.Navigate<UserLocationViewModel>();
                });
            }
        }

        public IMvxCommand TakeTasksCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    Int32 userId = Int32.Parse(CrossSecureStorage.Current.GetValue(SecureConstant.UserId));
                    var result = _taskService.GetUserTasksIdAsync(userId);
                });
            }
        }

        public IMvxAsyncCommand ShowTaskCommand
        {
            get
            {
                return new MvxAsyncCommand(async() =>
                {
                    ResultModel result = await _navigationService.Navigate<TaskViewModel,  Int32, ResultModel>(0);
                    if (result == null)
                    {
                        return;
                    }
                    if (result.Result == Enum.UserTaskResult.Save)
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

                    Int32 userId = Int32.Parse(CrossSecureStorage.Current.GetValue(SecureConstant.UserId));
                    List<Int32> list = _taskService.GetUserTasksIdAsync(userId);
                    var listTasks = new List<UserTask>();
                    foreach (var item in list)
                    {
                        var task = _taskService.GetUserTaskAsync(item);
                        listTasks.Add(new UserTask
                        {
                            Id = task.Id,
                            UserId = task.UserId,
                            ImagePath = task.ImagePath,
                            Title = task.Title,
                            Status = task.Status
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
                    var result = await _navigationService.Navigate<TaskViewModel, Int32, ResultModel>(item.Id);
                    if (result == null)
                    {
                        return;
                    }
                    if(result.Result == Enum.UserTaskResult.Delete)
                    {
                        var delete = ListOfTasks.Select(p => p)
                        .Where(p => p.Id == result.Changes.Id)
                        .FirstOrDefault();
                        ListOfTasks.Remove(delete);
                        return;
                    }
                    if(result.Result == Enum.UserTaskResult.UnChangeunchanged)
                    {
                        return;
                    }
                    if (result.Result == Enum.UserTaskResult.Save)
                    {
                        var modelToUpdate = ListOfTasks.Select(p => p)
                        .Where(p => p.Id == result.Changes.Id)
                        .FirstOrDefault();
                        ListOfTasks[ListOfTasks.IndexOf(modelToUpdate)] = result.Changes;
                    }
                });
            }
        }


        public IMvxAsyncCommand OpenProfileCommand
        {
            get
            {
                return new MvxAsyncCommand(async() =>
                {
                    await _navigationService.Navigate<UserProfileViewModel>();
                });
            }
        }


        public IMvxCommand ShowMenuCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    _navigationService.Navigate<MenuViewModel, TaskListViewModel>(this);
                });

            }
        }

        public IMvxAsyncCommand UserLogoutCommand
        {
            get
            {
                return new MvxAsyncCommand(async() =>
                {
                    await _navigationService.Close(this);
                });
            }
        }

        #endregion

    }
}

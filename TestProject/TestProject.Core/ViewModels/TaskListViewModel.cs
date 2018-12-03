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

namespace TestProject.Core.ViewModels
{
    public class TaskListViewModel
        : BaseViewModel
        {
        private readonly IMvxNavigationService _navigationService;
        private readonly ITasksRepository _taskService;
        private bool _isRefreshing;
        private ResultModel _userTask;
        private readonly IUserDialogs _userDialogs;

        private MvxObservableCollection<UserTask> _listOfTasks;

        public TaskListViewModel(IMvxNavigationService navigationService, ITasksRepository taskService, IUserDialogs userDialogs)
        {
            _listOfTasks = new MvxObservableCollection<UserTask>();
            ListOfTasks = new MvxObservableCollection<UserTask>();
            _navigationService = navigationService;
            _taskService = taskService;
            _userDialogs = userDialogs;
        }

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

        public IMvxCommand DeleteTaskCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    _taskService.SwipeTaskDelete(this);
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

        public IMvxAsyncCommand RefreshTaskCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    IsRefreshing = true;

                    ListOfTasks.RemoveRange(0, ListOfTasks.Count);
                    await Initialize();
                    
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

        #endregion

    }
}

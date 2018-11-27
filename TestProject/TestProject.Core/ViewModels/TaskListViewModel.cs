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
    public class TaskListViewModel
        : BaseViewModel<ResultModel>
        {
        private readonly IMvxNavigationService _navigationService;
        private ITaskService _taskService;
        private bool _isRefreshing;
        private ResultModel _userTask;
        private readonly IUserDialogs _userDialogs;

        private MvxObservableCollection<UserTask> _listOfTasks;

        public TaskListViewModel(IMvxNavigationService navigationService, ITaskService taskService, IUserDialogs userDialogs)
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
                _userTask = value;
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
            var list = await _taskService.GetTasksAsync(UserTask.Changes.UserId);
            foreach (var item in list)
            {
                ListOfTasks.Add(item);
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

        public IMvxAsyncCommand TakeTasksCommand
        {
            get
            {
                return new MvxAsyncCommand(async() =>
                {
                    var result = await _taskService.GetTasksAsync(UserTask.Changes.UserId);
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
                        Changes = new UserTask
                        {
                            UserId = UserTask.Changes.UserId
                        },
                        Result = Enum.UserTaskResult.Save
                    };
                    var result = await _navigationService.Navigate<TaskViewModel, ResultModel, ResultModel>(task);
                    if (result == null)
                    {
                        return;
                    }
                    if (result.Result == Enum.UserTaskResult.Save)
                    {
                        ListOfTasks.Add(result.Changes);
                        RefreshTaskCommand.Execute();
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

                    var result = await _taskService.RefreshUserTasks(UserTask.Changes.UserId);

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
                    var taskToNavigate = new ResultModel
                    {
                        Result = Enum.UserTaskResult.Update,
                        Changes = new UserTask
                        {
                            Id = task.Id,
                            Note = task.Note,
                            Title = task.Title,
                            Status = task.Status,
                            UserId = UserTask.Changes.UserId,
                            ImagePath = task.ImagePath
                        }
                    };

                    var result = await _navigationService.Navigate<TaskViewModel, ResultModel, ResultModel>(taskToNavigate);
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
                    }
                    if(result.Result == Enum.UserTaskResult.UnChangeunchanged)
                    {
                        return;
                    }
                    if (result.Result == Enum.UserTaskResult.Update)
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

        public override void Prepare(ResultModel model)
        {
            UserTask = model;
        }
    }
}

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

namespace TestProject.Core.ViewModels
{
    public class TaskListViewModel: BaseViewModel<ResultModel, ResultModel>
        {
        private readonly IMvxNavigationService _navigationService;
        private ITaskService _taskService;

        private MvxObservableCollection<UserTask> _listOfTasks;

        public TaskListViewModel(IMvxNavigationService navigationService, ITaskService taskService)
        {
            _taskService = taskService;
            _navigationService = navigationService;
            var userTasks = new List<UserTask>();
            _listOfTasks = new MvxObservableCollection<UserTask>();
            //_listOfTasks.Add(new UserTask { Id = 1, Title = "Just do it!", Note = "You can do this bro!", Status = false });
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
            var list = await _taskService.GetTasksAsync();
            foreach (var item in list)
            {
                _listOfTasks.Add(item);
            }
            return _listOfTasks;
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
                RaisePropertyChanged(() => ListOfTasks);
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
                    var result = await _taskService.GetTasksAsync();
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
                        Changes = new UserTask(),
                        Result = Enum.UserTaskResult.Save
                    };
                    var result = await _navigationService.Navigate<TaskViewModel, ResultModel, ResultModel>(task);
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
                return new MvxAsyncCommand(async() =>
                {
                    var result = await _taskService.GetCustomUserTasks();
                });
            }
        }

        public IMvxCommand<UserTask> ItemSelectedCommand
        {
            get
            {
                return new MvxCommand<UserTask>(async (UserTask task) =>
                {
                    var taskToNavigate = new ResultModel { Result = Enum.UserTaskResult.Update, Changes = new UserTask {Id=task.Id, Note=task.Note,Title=task.Title, Status=task.Status } };

                    var result = await _navigationService.Navigate<TaskViewModel, ResultModel, ResultModel>(taskToNavigate);
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
        

        #endregion

        public override void Prepare(ResultModel model)
        {
            base.Prepare();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TestProject.Core.Models;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using TestProject.Core.Interfaces;
using MvvmCross;
using TestProject.Core.Enum;

namespace TestProject.Core.ViewModels
{
    public class TaskViewModel : BaseViewModel<UserTask, ResultModel>
    {

        private readonly IMvxNavigationService _navigationService;
        private readonly ITaskService _taskService;
        private ResultModel _resultModel;

        private UserTask _userTask;

        public UserTask UserTask
        {
            get
            {
                return _userTask;
            }
            set
            {
                _userTask = value;
                RaisePropertyChanged(() => UserTask);
            }
        }

        public TaskViewModel(IMvxNavigationService navigationService, ITaskService taskService)
        {

            _resultModel = new ResultModel();
            if(UserTask == null)
            {
                _resultModel.result = UserTaskResult.Save;
            }
            if(UserTask != null)
            {
                _resultModel.result = UserTaskResult.Update;
            }
            
            _navigationService = navigationService;
            _taskService = taskService;
        }

        public override Task Initialize()
        {
            return Task.FromResult(0);
        }


        #region Commands

        public IMvxAsyncCommand ShowMenuCommand
        {
            get
            {
                return new MvxAsyncCommand(async() =>
                {
                    ResultModel resultModel = new ResultModel();
                    resultModel.Changes = UserTask;
                    resultModel.result = Enum.UserTaskResult.UnChangeunchanged;
                    await _navigationService.Close<ResultModel>(this, resultModel);
                });
            }
        }

        public IMvxAsyncCommand DeleteUserTaskCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    var result = await DeleteUserTask();
                    ResultModel resultModel = new ResultModel();
                    resultModel.Changes = UserTask;
                    resultModel.result = Enum.UserTaskResult.Delete;
                    await _navigationService.Close<ResultModel>(this, resultModel);
                });
            }
        }

        public IMvxAsyncCommand SaveUserTaskCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {

                    var result = await SaveTask(UserTask);
                    
                    _resultModel.Changes = new UserTask
                    {
                        Id = UserTask.Id,
                        Note= UserTask.Note,
                        Status = UserTask.Status,
                        Title = UserTask.Title
                    };
                    _resultModel.result = Enum.UserTaskResult.Update;
                    await _navigationService.Close<ResultModel>(this, _resultModel);
                });
            }
        }


        #endregion


        private Task<UserTask> GetUserTask()
        {
            var result = _taskService.GetTaskAsync(_userTask.Id);
            return result;
        }

        private Task<List<UserTask>> LoadAllTask()
        {
            var result = _taskService.GetTasksAsync();
            return result;
        }

        private Task<Int32> SaveTask(UserTask userTask)
        {
            var result = _taskService.SaveTaskAsync(userTask);
            return result;
        }

        private Task<Int32> DeleteUserTask()
        {
            var result = _taskService.DeleteTaskAsync(_userTask);
            return result;
        }

        public override void Prepare(UserTask parameter)
        {
            UserTask = parameter;
        }
    }
}

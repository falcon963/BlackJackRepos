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
    public class TaskViewModel : BaseViewModel<ResultModel, ResultModel>
    {

        private readonly IMvxNavigationService _navigationService;
        private readonly ITaskService _taskService;
        private ResultModel _resultModel;

        public ResultModel UserTask
        {
            get
            {
                return _resultModel;
            }
            set
            {
                _resultModel = value;
                RaisePropertyChanged(() => UserTask);
            }
        }

        public TaskViewModel(IMvxNavigationService navigationService, ITaskService taskService)
        {
            _resultModel = new ResultModel();
            _resultModel.Changes = new UserTask();
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
                    _resultModel.Result = Enum.UserTaskResult.UnChangeunchanged;
                    await _navigationService.Close<ResultModel>(this, _resultModel);
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
                    _resultModel.Result = Enum.UserTaskResult.Delete;
                    await _navigationService.Close<ResultModel>(this, _resultModel);
                });
            }
        }

        public IMvxAsyncCommand SaveUserTaskCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {

                    var result = await SaveTask(UserTask.Changes);
                    
                    _resultModel.Changes = new UserTask
                    {
                        Id = UserTask.Changes.Id,
                        Note= UserTask.Changes.Note,
                        Status = UserTask.Changes.Status,
                        Title = UserTask.Changes.Title
                    };
                    await _navigationService.Close<ResultModel>(this, _resultModel);
                });
            }
        }


        #endregion


        private Task<UserTask> GetUserTask()
        {
            var result = _taskService.GetTaskAsync(_resultModel.Changes.Id);
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
            var result = _taskService.DeleteTaskAsync(_resultModel.Changes);
            return result;
        }

        public override void Prepare(ResultModel parameter)
        {
            UserTask = parameter;
        }

    }
}

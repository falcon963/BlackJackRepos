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
using Acr.UserDialogs;
using MvvmCross.UI;

namespace TestProject.Core.ViewModels
{
    public class TaskViewModel
        : BaseViewModel<ResultModel, ResultModel>
    {

        private readonly IMvxNavigationService _navigationService;
        private readonly ITaskService _taskService;
        private ResultModel _resultModel;
        private readonly IUserDialogs _userDialogs;
        private UserTask _userTaskDublicate;

        public MvxColor ColorTheme { get; set; }

        public ResultModel UserTask
        {
            get
            {
                return _resultModel;
            }
            set
            {
                SetProperty(ref _resultModel, value);
                RaisePropertyChanged(() => UserTask);
            }
        }


        public TaskViewModel(IMvxNavigationService navigationService, ITaskService taskService, IUserDialogs userDialogs)
        {
            _resultModel = new ResultModel();
            _resultModel.Changes = new UserTask();
            _navigationService = navigationService;
            _taskService = taskService;
            _userDialogs = userDialogs;
            ColorTheme = new MvxColor(251, 192, 45);
        }

        public override Task Initialize()
        {
            return Task.FromResult(0);
        }


        public Boolean TitleEnableStatus
        {
            get
            {
                return String.IsNullOrEmpty(UserTask.Changes.Title);
            }
        }

        #region Commands

        public IMvxAsyncCommand ShowMenuCommand
        {
            get
            {
                return new MvxAsyncCommand(async() =>
                {
                    if(UserTask.Changes.Note != _userTaskDublicate.Note
                    || UserTask.Changes.Title != _userTaskDublicate.Title 
                    || UserTask.Changes.Status != _userTaskDublicate.Status)
                    {
                        var goBack = await _userDialogs.ConfirmAsync(new ConfirmConfig
                        {
                            Title = "Alert Messege",
                            Message = "If you go on TaskyDrop whithout save, you changes will be lose! Do you want this?",
                            OkText = "Yes",
                            CancelText = "No"
                        });
                        if (!goBack)
                        {
                            return;
                        }
                    }
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
                    var delete = await _userDialogs.ConfirmAsync(new ConfirmConfig
                    {
                        Title = "Delete Messege",
                        Message = "Do you want delete this task?",
                        OkText = "Yes",
                        CancelText = "No"
                    });
                    if (!delete)
                    {
                        return;
                    }

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
                    if(String.IsNullOrEmpty(UserTask.Changes.Title) 
                    || String.IsNullOrEmpty(UserTask.Changes.Note))
                    {
                        var alert = UserDialogs.Instance.Alert(
                            new AlertConfig
                            {
                                Message = "You can't save task when field is empty!",
                                OkText = "Ok",
                                Title = "System Alert"
                            });
                        return;
                    }

                    _resultModel.Changes = new UserTask
                    {
                        Id = UserTask.Changes.Id,
                        Note= UserTask.Changes.Note,
                        Status = UserTask.Changes.Status,
                        Title = UserTask.Changes.Title,
                        ImagePath = UserTask.Changes.ImagePath,
                        UserId = UserTask.Changes.UserId
                    };
                    await SaveTask(UserTask.Changes);
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

            _userTaskDublicate = new UserTask
            {
                UserId = parameter.Changes.UserId,
                Id = parameter.Changes.Id,
                Title = parameter.Changes.Title,
                Note = parameter.Changes.Note,
                Status = parameter.Changes.Status,
                ImagePath = parameter.Changes.ImagePath
            };
        }
    }
}

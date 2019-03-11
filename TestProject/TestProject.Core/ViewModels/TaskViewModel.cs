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
using Plugin.SecureStorage;
using TestProject.Core.Constant;
using System.Threading;

namespace TestProject.Core.ViewModels
{
    public class TaskViewModel
        : BaseViewModel<Int32, ResultModel>
    {
        #region Fields

        private readonly IMvxNavigationService _navigationService;
        private readonly ITasksService _taskService;
        private ResultModel _resultModel;
        private readonly IUserDialogs _userDialogs;
        private UserTask _userTaskDublicate;

        #endregion

        #region Propertys

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
            }
        }

        public Boolean TitleEnableStatus
        {
            get
            {
                return String.IsNullOrEmpty(UserTask.Changes.Title);
            }
        }

        public Boolean DeleteButtonStutus
        {
            get
            {
                return (UserTask.Changes.Id == 0) ? false : true;
            }
        }

        #endregion


        public TaskViewModel(IMvxNavigationService navigationService, ITasksService taskService, IUserDialogs userDialogs)
        {
            _taskService = taskService;
            _userDialogs = userDialogs;
            _navigationService = navigationService;
            _resultModel = new ResultModel();
            _resultModel.Changes = new UserTask();
            _resultModel.Result = new UserTaskResult();
            UserTask = new ResultModel();
            UserTask.Changes = new UserTask();
            UserTask.Result = new UserTaskResult();
            ColorTheme = new MvxColor(251, 192, 45);
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
                    if(UserTask.Changes.Note != _userTaskDublicate.Note
                    || UserTask.Changes.Title != _userTaskDublicate.Title 
                    || UserTask.Changes.Status != _userTaskDublicate.Status)
                    {
                        var goBack = await _userDialogs.ConfirmAsync(new ConfirmConfig
                        {
                            Title = MessengeFields.AlertMessege,
                            Message = MessengeFields.ChangeLoseMessege,
                            OkText = MessengeFields.OkText,
                            CancelText = MessengeFields.NoText
                        });
                        if (!goBack)
                        {
                            return;
                        }
                    }
                    UserTask.Result = Enum.UserTaskResult.UnChangeunchanged;
                    await _navigationService.Close<ResultModel>(this, UserTask);
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
                        Title = MessengeFields.AlertMessege,
                        Message = MessengeFields.DeleteMessege,
                        OkText = MessengeFields.OkText,
                        CancelText = MessengeFields.NoText
                    });
                    if (!delete)
                    {
                        return;
                    }

                    var result = DeleteUserTask(UserTask.Changes);
                    UserTask.Result = Enum.UserTaskResult.Delete;
                    await _navigationService.Close<ResultModel>(this, UserTask);
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
                                Message = MessengeFields.EmptyTaskFieldMessege,
                                OkText = MessengeFields.OkText,
                                Title = MessengeFields.AlertMessege
                            });
                        return;
                    }

                    UserTask.Changes.UserId = Int32.Parse(CrossSecureStorage.Current.GetValue(SecureConstant.UserId));
                    SaveTask(UserTask.Changes);
                    UserTask.Result = Enum.UserTaskResult.Save;
                    await _navigationService.Close<ResultModel>(this, UserTask);
                });
            }
        }

        #endregion


        private Int32 SaveTask(UserTask userTask)
        {
            var result = _taskService.SaveUserTaskAsync(userTask);
            return result;
        }

        private Int32 DeleteUserTask(UserTask userTask)
        {
            var result = _taskService.DeleteUserTaskAsync(userTask);
            return result;
        }

        private UserTask GetTask(Int32 taskId)
        {
            return _taskService.GetUserTaskAsync(taskId);
        }

        public override void Prepare(Int32 taskId)
        {
            UserTask task = GetTask(taskId);

            if (task != null)
            {
                UserTask.Changes = new UserTask
                {
                    Id = task.Id,
                    UserId = task.UserId,
                    ImagePath = task.ImagePath,
                    Note = task.Note,
                    Title = task.Title,
                    Status = task.Status
                };

                _userTaskDublicate = new UserTask
                {
                    Id = task.Id,
                    UserId = task.Id,
                    ImagePath = task.ImagePath,
                    Note = task.Note,
                    Title = task.Title,
                    Status = task.Status
                };
            }

            if(task == null)
            {
                UserTask = new ResultModel
                {
                    Changes = new UserTask(),
                    Result = new UserTaskResult()
                };
                _userTaskDublicate = new UserTask();
            }
            
        }
    }
}

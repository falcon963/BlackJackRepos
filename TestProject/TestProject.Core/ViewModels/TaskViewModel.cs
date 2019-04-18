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
using TestProject.Core.Constants;
using System.Threading;
using TestProject.Core.Services;
using TestProject.Core.DBConnection;
using TestProject.Core.Repositorys.Interfaces;
using TestProject.Resources;

namespace TestProject.Core.ViewModels
{
    public class TaskViewModel
        : BaseViewModel<Int32, ResultModel>
    {
        #region Fields

        private readonly ITasksRepository _taskService;
        private readonly IUserDialogs _userDialogs;
        private UserTask _userTaskDublicate;
        private readonly IFileRepository _fileService;

        #endregion

        #region Propertys

        public MvxColor ColorTheme { get; set; }

        public ResultModel UserTask { get; set; }

        public bool AddFile { get; set; }

        public MvxObservableCollection<FileItemViewModel> ListOfFiles { get; set; }

        public bool TitleEnableStatus
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


        public TaskViewModel(IMvxNavigationService navigationService, ITasksRepository taskService, IUserDialogs userDialogs, IFileRepository fileService)
        {
            #region Init Service`s
            _taskService = taskService;
            _userDialogs = userDialogs;
            NavigationService = navigationService;
            _fileService = fileService;
            #endregion

            #region Init Fields
            ListOfFiles = new MvxObservableCollection<FileItemViewModel>();
            UserTask = new ResultModel();
            UserTask.Changes = new UserTask();
            UserTask.Result = new UserTaskResult();
            ColorTheme = new MvxColor(251, 192, 45);
            #endregion
        }

        public override Task Initialize()
        {
            MvxObservableCollection<FileItemViewModel> result = new MvxObservableCollection<FileItemViewModel>();
            Task.Factory.StartNew(async () =>
            {
                result = await TaskFilesInitialize();
            });
            return base.Initialize();
        }

        public async Task<MvxObservableCollection<FileItemViewModel>> TaskFilesInitialize()
        {
            List<TaskFileModel> list = _fileService.GetAllTaskFiles(UserTask.Changes.Id);
            foreach (var item in list)
            {
                ListOfFiles.Add(new FileItemViewModel
                {
                    Id = item.Id,
                    TaskId = item.TaskId,
                    FileContent = item.FileContent,
                    FileName = item.FileName,
                    FileExtension = item.FileExtension,
                    ViewModel = this,
                    DeleteFile = (FileItemViewModel file) =>
                    {
                        ListOfFiles.Remove(file);
                        RaisePropertyChanged(() => ListOfFiles);
                        return true;
                    }
                }
                );
            }
            return ListOfFiles;
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
                            Title = Strings.AlertMessege,
                            Message = Strings.ChangeLoseMessege,
                            OkText = Strings.OkText,
                            CancelText = Strings.NoText
                        });
                        if (!goBack)
                        {
                            return;
                        }
                    }
                    UserTask.Result = Enum.UserTaskResult.NotChanged;
                    await NavigationService.Close<ResultModel>(this, UserTask);
                });
            }
        }


        public IMvxAsyncCommand DeleteUserTaskCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    if(UserTask.Changes.Id == 0)
                    {
                        var cantDelete = UserDialogs.Instance.Alert(new AlertConfig
                        {
                            Title = Strings.AlertMessege,
                            Message = Strings.CantDelete,
                            OkText = Strings.OkText,
                        });
                        return;
                    }
                    var delete = await _userDialogs.ConfirmAsync(new ConfirmConfig
                    {
                        Title = Strings.AlertMessege,
                        Message = Strings.DeleteMessege,
                        OkText = Strings.OkText,
                        CancelText = Strings.NoText
                    });
                    if (!delete)
                    {
                        return;
                    }
                    List<TaskFileModel> listFile = new List<TaskFileModel>();
                    foreach(FileItemViewModel file in ListOfFiles)
                    {
                        _fileService.DeleteById(file.Id);
                    }
                    DeleteUserTask(UserTask.Changes);
                    UserTask.Result = Enum.UserTaskResult.Deleted;
                    await NavigationService.Close<ResultModel>(this, UserTask);
                });
            }
        }

        public IMvxCommand<FileItemViewModel> AddFileCommand
        {
            get
            {
                return new MvxCommand<FileItemViewModel>((file) =>
                {
                    var modelToUpdate = ListOfFiles.Select(p => p)
                        .Where(p => p.FileName == file.FileName && p.FileExtension == file.FileExtension)
                        .FirstOrDefault();
                    if (modelToUpdate != null)
                    {
                        var oldFile = ListOfFiles[ListOfFiles.IndexOf(modelToUpdate)];
                        file.Id = oldFile.Id;
                        file.ViewModel = oldFile.ViewModel;
                        file.TaskId = oldFile.TaskId;
                        ListOfFiles[ListOfFiles.IndexOf(modelToUpdate)] = file;
                        RaisePropertyChanged(() => ListOfFiles);
                        return;
                    }
                    var addFile = new FileItemViewModel
                    {
                        Id = 0,
                        TaskId = UserTask.Changes.Id,
                        FileContent = file.FileContent,
                        FileExtension = file.FileExtension,
                        FileName = file.FileName,
                        ViewModel = this
                    };
                    ListOfFiles.Add(addFile);
                    RaisePropertyChanged(() => ListOfFiles);
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
                                Message = Strings.EmptyTaskFieldMessege,
                                OkText = Strings.OkText,
                                Title = Strings.AlertMessege
                            });
                        return;
                    }   
                        UserTask.Changes.UserId = Int32.Parse(CrossSecureStorage.Current.GetValue(SecureConstant.UserId));
                        int taskId = SaveTask(UserTask.Changes);
                        TaskFileModel fileItem;
                        foreach (FileItemViewModel file in ListOfFiles)
                        {
                            file.TaskId = taskId;
                            fileItem = new TaskFileModel
                            {
                                Id = file.Id,
                                TaskId = taskId,
                                FileContent = file.FileContent,
                                FileExtension = file.FileExtension,
                                FileName = file.FileName
                            };
                            _fileService.Save(fileItem);
                        }
                        UserTask.Result = Enum.UserTaskResult.Saved;
                        await NavigationService.Close<ResultModel>(this, UserTask);             
                });
            }
        }

        #endregion

        public Boolean DeleteFile(FileItemViewModel file)
        {
            if(file == null)
            {
                return false;
            }
            ListOfFiles.Remove(file);
            RaisePropertyChanged(() => ListOfFiles);
            if(file.Id != 0)
            {
                _fileService.DeleteById(file.Id);
            }
            return true;
        }

        private int SaveTask(UserTask userTask)
        {
            return _taskService.SaveTask(userTask);
        }

        private void DeleteUserTask(UserTask userTask)
        {
            _taskService.Delete(userTask);
        }

        private UserTask GetTask(int taskId)
        {
            return _taskService.GetDate(taskId);
        }

        public override void Prepare(int taskId)
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

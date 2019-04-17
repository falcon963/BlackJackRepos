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
        private Boolean _addFile;
        private MvxObservableCollection<FileItemViewModel> _listOfFile;
        private IFileService _fileService;

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

        public Boolean AddFile
        {
            get
            {
                return _addFile;
            }
            set
            {
                SetProperty(ref _addFile, value);
            }
        }

        public Boolean DeleteButtonStutus
        {
            get
            {
                return (UserTask.Changes.Id == 0) ? false : true;
            }
        }

        public MvxObservableCollection<FileItemViewModel> ListOfFiles
        {
            get
            {
                return _listOfFile;
            }
            set
            {
                SetProperty(ref _listOfFile, value);
            }
        }

        #endregion


        public TaskViewModel(IMvxNavigationService navigationService, ITasksService taskService, IUserDialogs userDialogs, IFileService fileService)
        {
            #region Init Service`s
            _fileService = fileService;
            _taskService = taskService;
            _userDialogs = userDialogs;
            _navigationService = navigationService;
            #endregion

            #region Init Fields
            _listOfFile = new MvxObservableCollection<FileItemViewModel>();
            _resultModel = new ResultModel();
            _resultModel.Changes = new UserTask();
            _resultModel.Result = new UserTaskResult();
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
            List<TaskFileModel> list = _fileService.TakeAllTaskFiles(UserTask.Changes.Id);
            foreach (var item in list)
            {
                FileItemViewModel file = new FileItemViewModel
                {
                    Id = item.Id,
                    TaskId = item.TaskId,
                    FileContent = item.FileContent,
                    FileName = item.FileName,
                    FileExtension = item.FileExtension,
                    ViewModel = this
                };
                ListOfFiles.Add(file);
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
                    if(UserTask.Changes.Id == 0)
                    {
                        var cantDelete = UserDialogs.Instance.Alert(new AlertConfig
                        {
                            Title = MessengeFields.AlertMessege,
                            Message = MessengeFields.CantDelete,
                            OkText = MessengeFields.OkText,
                        });
                        return;
                    }
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
                    List<TaskFileModel> listFile = new List<TaskFileModel>();
                    foreach(FileItemViewModel file in ListOfFiles)
                    {
                        _fileService.DeleteFile(file.Id);
                    }
                    var result = DeleteUserTask(UserTask.Changes);
                    UserTask.Result = Enum.UserTaskResult.Delete;
                    await _navigationService.Close<ResultModel>(this, UserTask);
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
                                Message = MessengeFields.EmptyTaskFieldMessege,
                                OkText = MessengeFields.OkText,
                                Title = MessengeFields.AlertMessege
                            });
                        return;
                    }   
                        UserTask.Changes.UserId = Int32.Parse(CrossSecureStorage.Current.GetValue(SecureConstant.UserId));
                        Int32 taskId = SaveTask(UserTask.Changes);
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
                            _fileService.AddFile(fileItem);
                        }
                        UserTask.Result = Enum.UserTaskResult.Save;
                        await _navigationService.Close<ResultModel>(this, UserTask);             
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
                _fileService.DeleteFile(file.Id);
            }
            return true;
        }

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

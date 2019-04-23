using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TestProject.Core.Models;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using MvvmCross;
using TestProject.Core.Enums;
using Acr.UserDialogs;
using MvvmCross.UI;
using Plugin.SecureStorage;
using TestProject.Core.Constants;
using System.Threading;
using TestProject.Core.Servicies;
using TestProject.Core.DBConnection;
using TestProject.Core.Repositories.Interfacies;
using TestProject.Resources;
using TestProject.Core.Colors;
using TestProject.Core.Servicies.Interfacies;
using TestProject.Core.Helpers.Interfaces;

namespace TestProject.Core.ViewModels
{
    public class TaskViewModel
        : BaseViewModel<Int32, ResultModel>
    {
        #region Fields

        private readonly ITasksRepository _taskService;
        private readonly IUserDialogs _userDialogs;
        private readonly IFileRepository _fileService;
        private readonly IDialogsService _dialogsService;
        private readonly IUserHelper _userHelper;
        private readonly IValidationService _validationService;
        private UserTask _userTaskDublicate;

        #endregion

        #region Propertys

        public MvxColor ColorTheme { get; set; }

        public ResultModel UserTask { get; set; }

        public bool AddFile { get; set; }

        public MvxObservableCollection<FileItemViewModel> ListOfFiles { get; set; }

        public bool IsTitleEnabled
        {
            get
            {
                return String.IsNullOrEmpty(UserTask.Changes.Title);
            }
        }

        #endregion


        public TaskViewModel(IMvxNavigationService navigationService, ITasksRepository taskService, IUserDialogs userDialogs, IFileRepository fileService, IDialogsService dialogsService, IUserHelper userHelper, IValidationService validationService):base(navigationService)
        {
            #region Init Service`s
            _taskService = taskService;
            _userDialogs = userDialogs;
            _fileService = fileService;
            _dialogsService = dialogsService;
            _userHelper = userHelper;
            _validationService = validationService;
            #endregion

            #region Init Fields
            ListOfFiles = new MvxObservableCollection<FileItemViewModel>();
            UserTask = new ResultModel();
            UserTask.Changes = new UserTask();
            UserTask.Result = new UserTaskResult();
            #endregion
        }

        public override void Prepare()
        {
            base.Prepare();
            ColorTheme = AppColors.LoginColor;
        }

        public override Task Initialize()
        {
            TaskFilesInitialize();
            return base.Initialize();
        }

        public async Task<MvxObservableCollection<FileItemViewModel>> TaskFilesInitialize()
        {
            List<TaskFileModel> list = _fileService.GetRange(UserTask.Changes.Id).ToList();
            foreach (var item in list)
            {
                ListOfFiles.Add(new FileItemViewModel
                {
                    Id = item.Id,
                    TaskId = item.TaskId,
                    Content = item.FileContent,
                    Name = item.FileName,
                    Extension = item.FileExtension,
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
                return new MvxAsyncCommand(async () =>
                {
                    if(!UserTask.Changes.Equals(_userTaskDublicate))
                    {
                        var userChose = await _dialogsService.ShowConfirmDialogAsync(message: Strings.ChangeLoseMessege, title: Strings.AlertMessege);
                        if (!userChose)
                        {
                            return;
                        }
                    }
                    UserTask.Result = UserTaskResult.NotChanged;
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
                        _dialogsService.ShowAlert(message: Strings.CantDelete);
                        return;
                    }
                    var deleteUserRequest = await _dialogsService.ShowConfirmDialogAsync(message: Strings.DeleteMessege, title: Strings.AlertMessege);
                    if (!deleteUserRequest)
                    {
                        return;
                    }
                    List<TaskFileModel> listFile = new List<TaskFileModel>();
                    foreach(FileItemViewModel file in ListOfFiles)
                    {
                        _fileService.Delete(file.Id);
                    }
                    DeleteUserTask(UserTask.Changes);
                    UserTask.Result = UserTaskResult.Deleted;
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
                    var modelToUpdate = ListOfFiles
                        .FirstOrDefault(p => p.Name == file.Name && p.Extension == file.Extension);
                    if (modelToUpdate != null)
                    {
                        int modelId = ListOfFiles.IndexOf(modelToUpdate);
                        var oldFile = ListOfFiles[modelId];
                        file.Id = oldFile.Id;
                        file.ViewModel = oldFile.ViewModel;
                        file.TaskId = oldFile.TaskId;
                        ListOfFiles[modelId] = file;
                        RaisePropertyChanged(() => ListOfFiles);
                        return;
                    }
                    var addFile = new FileItemViewModel
                    {
                        Id = 0,
                        TaskId = UserTask.Changes.Id,
                        Content = file.Content,
                        Extension = file.Extension,
                        Name = file.Name,
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
                    var validationModel = new TaskFieldValidationModel
                    {
                        Note = UserTask.Changes.Note,
                        Title = UserTask.Changes.Title
                    };

                    ModelState modelState =_validationService.Validate(validationModel);

                    if(!modelState.IsValid)
                    {
                        foreach(string error in modelState.Errors)
                        {
                            _dialogsService.ShowAlert(message: error);
                        }
                        return;
                    }   
                        UserTask.Changes.UserId = _userHelper.UserId;
                        int taskId = SaveTask(UserTask.Changes);
                        TaskFileModel fileItem;
                        foreach (FileItemViewModel file in ListOfFiles)
                        {
                            file.TaskId = taskId;
                            fileItem = new TaskFileModel
                            {
                                Id = file.Id,
                                TaskId = taskId,
                                FileContent = file.Content,
                                FileExtension = file.Extension,
                                FileName = file.Name
                            };
                            _fileService.Save(fileItem);
                        }
                        UserTask.Result = UserTaskResult.Saved;
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
                _fileService.Delete(file.Id);
            }
            return true;
        }

        private int SaveTask(UserTask userTask)
        {
            return _taskService.Save(userTask);
        }

        private void DeleteUserTask(UserTask userTask)
        {
            _taskService.Delete(userTask);
        }

        private UserTask GetTask(int taskId)
        {
            return _taskService.Get(taskId);
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

                _userTaskDublicate = new UserTask(UserTask.Changes);
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

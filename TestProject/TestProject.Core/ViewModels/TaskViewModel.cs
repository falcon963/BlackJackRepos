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
using TestProject.Core.Services;
using TestProject.Core.DBConnection;
using TestProject.Core.Repositories.Interfaces;
using TestProject.Resources;
using TestProject.Core.Colors;
using TestProject.Core.Services.Interfaces;
using TestProject.Core.Helpers.Interfaces;

namespace TestProject.Core.ViewModels
{
    public class TaskViewModel
        : BaseViewModel<int, ResultModel>
    {
        #region Fields

        private readonly ITasksRepository _taskService;
        private readonly IUserDialogs _userDialogs;
        private readonly IFileRepository _fileService;
        private readonly IDialogsService _dialogsService;
        private readonly IUserHelper _userHelper;
        private readonly IValidationService _validationService;

        private UserTask _userTaskCopy;

        #endregion

        #region Propertys

        public MvxColor ColorTheme { get; set; }

        public ResultModel UserTask { get; set; }

        public bool AddFile { get; set; }

        public MvxObservableCollection<FileItemViewModel> Files { get; set; }

        public bool IsTitleEnabled
        {
            get
            {
                return String.IsNullOrEmpty(UserTask.Changes.Title);
            }
        }

        public bool IsDeleteButtonHidden
        {
            get
            {
                if(UserTask.Changes.Id == 0)
                {
                    return false;
                }
                return true;
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
            Files = new MvxObservableCollection<FileItemViewModel>();
            UserTask = new ResultModel();
            UserTask.Changes = new UserTask();
            #endregion

            ColorTheme = AppColors.LoginBackgroundColor;
        }

        public override Task Initialize()
        {
            TaskFilesInitialize();
            return base.Initialize();
        }

        public async Task<MvxObservableCollection<FileItemViewModel>> TaskFilesInitialize()
        {
            List<TaskFileModel> list = _fileService.GetFilesList(UserTask.Changes.Id).ToList();
            foreach (var item in list)
            {
                Files.Add(new FileItemViewModel
                {
                    Id = item.Id,
                    TaskId = item.TaskId,
                    Content = item.FileContent,
                    Name = item.FileName,
                    Extension = item.FileExtension,
                    DeleteFile = DeleteFile
                }
                );
            }
            return Files;
        }


        #region Commands

        public IMvxAsyncCommand ShowMenuCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    if(UserTask.Changes.Equals(_userTaskCopy))
                    {
                        await NavigationService.Close<ResultModel>(this, UserTask);
                    }

                    var isGoBackConfirmed = await _dialogsService.ShowConfirmDialogAsync(message: Strings.IfYouGoOnTaskyDropWithoutSaveYourChangesWillBeLoseDoYouWantThis,
                            title: Strings.AlertMessege);

                    if (!isGoBackConfirmed)
                    {
                        return;
                    }
                });
            }
        }


        public IMvxAsyncCommand DeleteUserTaskCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    var deleteUserConfirmation = await _dialogsService.ShowConfirmDialogAsync(message: Strings.DoYouWantDeleteThisTask, title: Strings.AlertMessege);

                    if (!deleteUserConfirmation)
                    {
                        return;
                    }

                    _taskService.Delete(UserTask.Changes);

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
                    var modelToUpdate = Files
                        .FirstOrDefault(p => p.Name == file.Name && p.Extension == file.Extension);
                    if (modelToUpdate != null)
                    {
                        int fileId = Files.IndexOf(modelToUpdate);
                        var oldFile = Files[fileId];

                        oldFile.Content = file.Content;

                        Files[fileId] = oldFile;

                        return;
                    }

                    var newFile = new FileItemViewModel
                    {
                        TaskId = UserTask.Changes.Id,
                        Content = file.Content,
                        Extension = file.Extension,
                        Name = file.Name,
                        DeleteFile = DeleteFile
                    };

                    Files.Add(newFile);
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
                        _dialogsService.ShowAlert(modelState.Errors.FirstOrDefault());

                        return;
                    }   

                        UserTask.Changes.UserId = _userHelper.UserId;

                        int taskId = _taskService.Save(UserTask.Changes);
                
                        TaskFileModel fileItem;
                        foreach (FileItemViewModel file in Files)
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

        private void DeleteFile(FileItemViewModel file)
        {
            Files.Remove(file);
        }

        public override void Prepare(int taskId)
        {
            UserTask task = _taskService.Get(taskId);

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

                _userTaskCopy = new UserTask(UserTask.Changes);
            }

            if(task == null)
            {
                UserTask = new ResultModel
                {
                    Changes = new UserTask(),
                    Result = new UserTaskResult()
                };
                _userTaskCopy = new UserTask();
            }
            
        }
    }
}

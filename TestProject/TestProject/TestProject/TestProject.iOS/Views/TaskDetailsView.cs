using CoreGraphics;
using Foundation;
using MobileCoreServices;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using System;
using System.Linq;
using TestProject.Core.ViewModels;
using TestProject.iOS.Converters;
using TestProject.iOS.ResourcesHelpers;
using TestProject.iOS.Services;
using TestProject.iOS.Sources;
using TestProject.LanguageResources;
using UIKit;

namespace TestProject.iOS.Views
{
    [MvxModalPresentation]
    public partial class TaskDetailsView 
        : BaseMenuView<TaskViewModel>
    {
        #region Init Fealds
        private UITapGestureRecognizer _tap;

        private UIDocumentMenuViewController _documentPickerController;

        private readonly PhotoService<TaskDetailsView> _photoService;

        private readonly DocumentsService<TaskDetailsView> _documentsService;

        #endregion

        protected override UIScrollView ScrollView { get => base.ScrollView; set => base.ScrollView = value; }

        public TaskDetailsView() : base("TaskDetailsView", null)
        {
            Action<FileItemViewModel> saveAction = (FileItemViewModel file) => { ViewModel.AddFileCommand.Execute(file); };
            _photoService = new PhotoService<TaskDetailsView>(this, TaskImage);
            _documentsService = new DocumentsService<TaskDetailsView>(this, saveAction);
            HideKeyboard(_tap);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ScrollView = MainScrollView;

            FileList.BackgroundColor = UIColor.Clear;
            var source = new TaskFilesListSource(FileList, this);

            #region Init Property Sub

            //ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            ViewModel.Files.CollectionChanged +=   ListOfFiles_CollectionChanged;

            #endregion

            #region Init MvvmCrossBinds

            var set = this.CreateBindingSet<TaskDetailsView, TaskViewModel>();
            set.Bind(TaskName).To(vm => vm.UserTask.Changes.Title);
            set.Bind(TaskName).For(v => v.Enabled).To(vm => vm.IsTitleEnabled);
            set.Bind(TaskNote).To(vm => vm.UserTask.Changes.Note);
            set.Bind(TaskStatus).To(vm => vm.UserTask.Changes.Status);
            set.Bind(DeleteButton).To(vm => vm.DeleteUserTaskCommand);
            set.Bind(BackButton).To(vm => vm.ShowMenuCommand);
            set.Bind(DeleteButton).For(v => v.Hidden).To(vm => vm.IsDeleteButtonHidden);
            set.Bind(source).For(x => x.ItemsSource).To(vm => vm.Files);
            set.Bind(View).For(v => v.BackgroundColor).To(vm => vm.ColorTheme).WithConversion(new ColorValueConverter());
            set.Bind(TaskImage).To(vm => vm.UserTask.Changes.ImagePath).WithConversion(new ImageValueConverter());
            set.Bind(TaskTitle).For(x => x.Title).To(vm => vm.UserTask.Changes.Title).WithConversion(new TaskTitleValueConverter());
            set.Apply();

            #endregion

            FileList.RowHeight = 40;
            FileList.Source = source;
            InitFileList();
           

            #region Init shadows
            AddShadow(TaskName);
            AddShadow(TaskNote);
            AddShadow(TaskImage);
            AddShadow(DeleteButton);
            AddShadow(SaveButton);
            AddShadow(TaskStatus);
            AddShadow(AddFileButton);
            #endregion

            

            #region Init NSNotificationCenter.Keyboard
            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidHideNotification, HandleKeyboardDidHide);

            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, HandleKeyboardDidShow);
            #endregion

            #region Init TapRecognizer

            UITapGestureRecognizer recognizer = new UITapGestureRecognizer(_photoService.OpenCamera);
            TaskImage.AddGestureRecognizer(recognizer);

            #endregion

            BackButton.Image = UIImage.FromBundle("back");

            MainScrollView.ContentSize = new CGSize(0, MainScrollView.Frame.Height);

            TaskNote.ContentInset = UIEdgeInsets.Zero;
            TaskNote.ClipsToBounds = true;

            this.AutomaticallyAdjustsScrollViewInsets = false;


            FileList.ReloadData();
        }

        private void ListOfFiles_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            InitFileList();
        }


        private void InitFileList()
        {
            var cellHeight = FileList.RowHeight;
            int count = ViewModel.Files.Count;

            if (count < 2 && FileViewHeight.Constant != cellHeight)
            {
                FileViewHeight.Constant = cellHeight;
            }
            if (count < 3 && count > 1 && FileViewHeight.Constant != 2 * cellHeight)
            {
                FileViewHeight.Constant = 2 * cellHeight;
            }
            if (count < 4 && count > 2 && FileViewHeight.Constant != 3 * cellHeight)
            {
                FileViewHeight.Constant = 3 * cellHeight;
            }
        }

        #region Ovveride Method`s

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        public override void HandleKeyboardDidShow(NSNotification obj)
        {
            base.HandleKeyboardDidShow(obj);
        }

        public override void HandleKeyboardDidHide(NSNotification obj)
        {
            base.HandleKeyboardDidHide(obj);
        }

        #endregion

        #region ButtonEvents

        partial void PressSaveButton(UIButton sender)
        {
            var data = TaskImage.Image.AsJPEG();
            ViewModel.UserTask.Changes.ImagePath = data.GetBase64EncodedString(NSDataBase64EncodingOptions.SixtyFourCharacterLineLength);
            ViewModel.SaveUserTaskCommand.Execute();
        }

        partial void PressAddButton(UIButton sender)
        {
            _documentsService.ImportFromDocMenu(sender);
        }

        #endregion
    }
}
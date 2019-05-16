using CoreGraphics;
using Foundation;
using MobileCoreServices;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using System;
using System.Linq;
using TestProject.Core.Services.Interfaces;
using TestProject.Core.ViewModels;
using TestProject.iOS.Converters;
using TestProject.iOS.ResourcesHelpers;
using TestProject.iOS.Services;
using TestProject.iOS.Services.Interfaces;
using TestProject.iOS.Sources;
using TestProject.LanguageResources;
using UIKit;
using IDocumentPickerService = TestProject.iOS.Services.Interfaces.IDocumentPickerService;

namespace TestProject.iOS.Views
{
    [MvxModalPresentation]
    public partial class TaskDetailsView 
        : BaseView<TaskDetailsView ,TaskViewModel>
    {
        #region Init Fealds
        private UITapGestureRecognizer _tap;

        private UIDocumentMenuViewController _documentPickerController;

        private readonly IPhotoService _photoService;

        private readonly IDocumentPickerService _documentsPickerService;

        private TaskFilesListSource _source;

        #endregion

        public override bool SetupBindings()
        {
            BindingSet.Bind(TaskName).To(vm => vm.UserTask.Changes.Title);
            BindingSet.Bind(TaskName).For(v => v.Enabled).To(vm => vm.IsTitleEnabled);
            BindingSet.Bind(TaskNote).To(vm => vm.UserTask.Changes.Note);
            BindingSet.Bind(TaskStatus).To(vm => vm.UserTask.Changes.Status);
            BindingSet.Bind(DeleteButton).To(vm => vm.DeleteUserTaskCommand);
            BindingSet.Bind(AddFileButton).To(vm => vm.AddFileCommand);
            BindingSet.Bind(BackButton).To(vm => vm.ShowMenuCommand);
            BindingSet.Bind(SaveButton).To(vm => vm.SaveUserTaskCommand);
            BindingSet.Bind(DeleteButton).For(v => v.Hidden).To(vm => vm.IsDeleteButtonHidden);
            BindingSet.Bind(_source).For(x => x.ItemsSource).To(vm => vm.Files);
            BindingSet.Bind(View).For(v => v.BackgroundColor).To(vm => vm.ColorTheme).WithConversion("NativeColor");
            BindingSet.Bind(TaskImage).To(vm => vm.UserTask.Changes.ImagePath).WithConversion(new ImageValueConverter());
            BindingSet.Bind(TaskTitle).For(x => x.Title).To(vm => vm.UserTask.Changes.Title).WithConversion(new TaskTitleValueConverter());

            return base.SetupBindings();
        }

        public TaskDetailsView() : base(nameof(TaskDetailsView), null)
        {

        }

        public TaskDetailsView(IDocumentPickerService documentPickerService, IPhotoService photoService) : base(nameof(TaskDetailsView), null)
        {
            _photoService = photoService;
            _documentsPickerService = documentPickerService;

            _documentsPickerService.PresentedDocumentPicker += (sender, e) => {
                PresentViewController(e, true, null);
            };

            _documentsPickerService.PresentedMenuDocumentPicker += (sender, e) => {
                PresentViewController(e, true, null);
            };

            _photoService.ImagePickerDelegateSubscription += (sender, e) => {
                e = this;
            };

            _photoService.PresentPicker += (sender, e) => {
                PresentViewController(e, true, null);
            };

            _photoService.PresentAlert += (sender, e) => {
                PresentViewController(e, true, null);
            };
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            FileList.BackgroundColor = UIColor.Clear;
            _source = new TaskFilesListSource(FileList, this);

            #region Init Property Sub

            //ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            ViewModel.Files.CollectionChanged +=   ListOfFiles_CollectionChanged;

            #endregion

            FileList.RowHeight = 40;
            FileList.Source = _source;
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
            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidHideNotification, OnKeyboardWillHide);

            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, OnKeyboardWillShow);
            #endregion

            #region Init TapRecognizer

            UITapGestureRecognizer recognizer = new UITapGestureRecognizer(() => { ViewModel?.PickPhotoCommand?.Execute(); });
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

        #endregion

    }
}
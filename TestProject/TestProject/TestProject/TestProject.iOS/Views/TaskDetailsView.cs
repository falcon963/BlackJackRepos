using CoreGraphics;
using Foundation;
using MobileCoreServices;
using MvvmCross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using System;
using System.Linq;
using TestProject.Core.Services.Interfaces;
using TestProject.Core.ViewModels;
using TestProject.iOS.Converters;
using TestProject.iOS.Helpers;
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

        private UIDocumentMenuViewController _documentPickerController;

        private  IPhotoService _photoService;

        private  IDocumentPickerService _documentsPickerService;

        private TaskFilesListSource _source;

        private const float FileListRowHeight = 40f;

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
            BindingSet.Bind(AddFileButton).To(vm => vm.AddFileCommand);
            BindingSet.Bind(SaveButton).To(vm => vm.SaveUserTaskCommand);
            BindingSet.Bind(DeleteButton).For(v => v.Hidden).To(vm => vm.IsDeleteButtonHidden);
            BindingSet.Bind(_source).For(x => x.ItemsSource).To(vm => vm.Files);
            BindingSet.Bind(View).For(v => v.BackgroundColor).To(vm => vm.ColorTheme).WithConversion("NativeColor");
            BindingSet.Bind(TaskImage).To(vm => vm.TaskImage).WithConversion(new ImageValueConverter());
            BindingSet.Bind(TaskTitle).For(x => x.Title).To(vm => vm.UserTask.Changes.Title).WithConversion(new TaskTitleValueConverter());

            return base.SetupBindings();
        }

        public override bool SetupEvents()
        {
            _documentsPickerService.PresentedDocumentPicker += _documentsPickerService_PresentedDocumentPicker;

            _documentsPickerService.PresentedMenuDocumentPicker += _documentsPickerService_PresentedMenuDocumentPicker;

            _documentsPickerService.CanceledPick += _documentsPickerService_CanceledPick;

            _photoService.ImagePickerDelegateSubscription += _photoService_ImagePickerDelegateSubscription;

            _photoService.PresentPicker += _photoService_PresentPicker;

            _photoService.PresentAlert += _photoService_PresentAlert;

            _photoService.DismissSubview += _photoService_DismissSubview;

            ViewModel.Files.CollectionChanged += ListOfFiles_CollectionChanged;

            #region Init NSNotificationCenter.Keyboard
            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidHideNotification, HandleKeyboardDidHide);

            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, HandleKeyboardDidShow);
            #endregion

            #region Init TapRecognizer

            UITapGestureRecognizer recognizer = new UITapGestureRecognizer(() => { ViewModel?.PickPhotoCommand?.Execute(); });
            TaskImage.AddGestureRecognizer(recognizer);

            #endregion

            return base.SetupEvents();
        }

        public override bool CustomizeViews()
        {
            FileList.BackgroundColor = UIColor.Clear;

            FileList.RowHeight = FileListRowHeight;

            BackButton.Image = UIImage.FromBundle("back");

            MainScrollView.ContentSize = new CGSize(0, MainScrollView.Frame.Height);

            TaskNote.ContentInset = UIEdgeInsets.Zero;

            TaskNote.ClipsToBounds = true;

            #region Init shadows
            AddShadow(TaskName);
            AddShadow(TaskNote);
            AddShadow(TaskImage);
            AddShadow(DeleteButton);
            AddShadow(SaveButton);
            AddShadow(TaskStatus);
            AddShadow(AddFileButton);
            #endregion

            AutomaticallyAdjustsScrollViewInsets = false;

            return base.CustomizeViews();
        }

        public TaskDetailsView() : base(nameof(TaskDetailsView), null)
        {

        }

        #region Handlers

        private void _photoService_ImagePickerDelegateSubscription(object sender, NSObject e)
        {
            e = this;
        }

        private void _documentsPickerService_CanceledPick(object sender, UIDocumentPickerViewController e)
        {
            e.View.RemoveFromSuperview();
        }

        private void _photoService_DismissSubview(object sender, UIImagePickerController e)
        {
            e.View.RemoveFromSuperview();
        }

        private void _documentsPickerService_PresentedDocumentPicker(object sender, UIDocumentPickerViewController e)
        {
            View.AddSubview(e.View);
            //PresentViewController(e, true, null);
        }

        private void _documentsPickerService_PresentedMenuDocumentPicker(object sender, UIDocumentMenuViewController e)
        {
            PresentViewController(e, true, null);
        }

        private void _photoService_PresentPicker(object sender, UIImagePickerController e)
        {
            View.AddSubview(e.View);
        }

        private void _photoService_PresentAlert(object sender, UIAlertController e)
        {
            //View.AddSubview(e.View);
            //TabBarController.PresentModalViewController(e, true);
            PresentViewController(e, true, null);
        }

        #endregion

        public override void ViewDidLoad()
        {
            _photoService = Mvx.IoCProvider.Resolve<IPhotoService>();
            _documentsPickerService = Mvx.IoCProvider.Resolve<IDocumentPickerService>();

            ScrollView = MainScrollView;

            _source = new TaskFilesListSource(FileList, this);

            base.ViewDidLoad();

            FileList.Source = _source;

            InitFileList();

            FileList.ReloadData();
        }

        private void ListOfFiles_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            InitFileList();
        }


        private void InitFileList()
        {
            int count = ViewModel.Files.Count;

            if (count < 2 && FileViewHeight.Constant != FileListRowHeight)
            {
                FileViewHeight.Constant = FileListRowHeight;
            }
            if (count < 3 && count > 1 && FileViewHeight.Constant != 2 * FileListRowHeight)
            {
                FileViewHeight.Constant = 2 * FileListRowHeight;
            }
            if (count < 4 && count > 2 && FileViewHeight.Constant != 3 * FileListRowHeight)
            {
                FileViewHeight.Constant = 3 * FileListRowHeight;
            }
        }

        #region Ovveride Method`s

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        #endregion


        public void Unsubscribe()
        {
            _documentsPickerService.PresentedDocumentPicker -= _documentsPickerService_PresentedDocumentPicker;

            _documentsPickerService.PresentedMenuDocumentPicker -= _documentsPickerService_PresentedMenuDocumentPicker;

            _documentsPickerService.CanceledPick -= _documentsPickerService_CanceledPick;

            _photoService.ImagePickerDelegateSubscription -= _photoService_ImagePickerDelegateSubscription;

            _photoService.PresentPicker -= _photoService_PresentPicker;

            _photoService.PresentAlert -= _photoService_PresentAlert;
        }

        public override void ViewDidDisappear(bool animated)
        {
            Unsubscribe();

            base.ViewDidDisappear(animated);
        }
    }
}
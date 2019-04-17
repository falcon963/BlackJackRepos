using CoreGraphics;
using Foundation;
using MobileCoreServices;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using System;
using TestProject.Core.ViewModels;
using TestProject.iOS.Converters;
using TestProject.iOS.ResourcesHelper;
using TestProject.iOS.Source;
using UIKit;

namespace TestProject.iOS.Views
{
    [MvxModalPresentation]
    public partial class TaskDetailsView 
        : BaseMenuView<TaskViewModel>
    {
        #region Init Fealds
        private UITapGestureRecognizer _tap;

        private UIImagePickerController _imagePickerController = new UIImagePickerController();

        private UIDocumentMenuViewController _documentPickerController;

        private String[] _allowedUTIs;

        private NSUrl _documentURL;
        #endregion

        public override UIScrollView ScrollView { get => base.ScrollView; set => base.ScrollView = value; }

        public TaskDetailsView() : base("TaskDetailsView", null)
        {
            HideKeyboard(_tap);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ScrollView = MainScrollView;

            FileList.BackgroundColor = UIColor.Clear;
            var source = new TaskFilesListSource(FileList, this);

            #region Init Property Sub

            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            #endregion

            #region Init MvvmCrossBinds

            var set = this.CreateBindingSet<TaskDetailsView, TaskViewModel>();
            set.Bind(TaskName).To(vm => vm.UserTask.Changes.Title);
            set.Bind(TaskName).For(v => v.Enabled).To(vm => vm.TitleEnableStatus);
            set.Bind(TaskNote).To(vm => vm.UserTask.Changes.Note);
            set.Bind(TaskStatus).To(vm => vm.UserTask.Changes.Status);
            set.Bind(DeleteButton).To(vm => vm.DeleteUserTaskCommand);
            set.Bind(BackButton).To(vm => vm.ShowMenuCommand);
            set.Bind(source).For(x => x.ItemsSource).To(vm => vm.ListOfFiles);
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

            #region Init UTIs
            _allowedUTIs = new string[] {
                    UTType.RTF,
                    UTType.PNG,
                    UTType.Text,
                    UTType.PDF,
                    UTType.JPEG
                };
            #endregion

            #region Init NSNotificationCenter.Keyboard
            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidHideNotification, HandleKeyboardDidHide);

            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, HandleKeyboardDidShow);
            #endregion

            #region Init TapRecognizer

            UITapGestureRecognizer recognizer = new UITapGestureRecognizer(OpenImage);
            TaskImage.AddGestureRecognizer(recognizer);

            #endregion

            BackButton.Image = UIImage.FromFile("back_to_50.png");

            MainScrollView.ContentSize = new CGSize(0, MainScrollView.Frame.Height);

            TaskNote.ContentInset = UIEdgeInsets.Zero;
            TaskNote.ClipsToBounds = true;

            _imagePickerController.Delegate = this;

            this.AutomaticallyAdjustsScrollViewInsets = false;


            FileList.ReloadData();
        }


        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {         
            if (e.PropertyName == "ListOfFiles")
            {
                InitFileList();
            }
        }


        private void InitFileList()
        {
            var cellHeight = FileList.RowHeight;
            Int32 count = ViewModel.ListOfFiles.Count;

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

        #region ImagePicker

        private void OpenImage()
        {
            var actionSheet = UIAlertController.Create("Photo Source", "Choose a source", UIAlertControllerStyle.ActionSheet);

            actionSheet.AddAction(UIAlertAction.Create("Camera", UIAlertActionStyle.Default, (actionCamera) =>
            {
                OpenCamera();
            }));

            actionSheet.AddAction(UIAlertAction.Create("Gallery", UIAlertActionStyle.Default, (actionLibrary) =>
            {
                OpenLibrary();
            }));

            actionSheet.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));

            _imagePickerController.FinishedPickingMedia += Handle_FinishedPickingMedia;

            this.PresentViewController(actionSheet, true, null);
        }


        public void OpenCamera()
        {
            if (UIImagePickerController.IsSourceTypeAvailable(UIImagePickerControllerSourceType.Camera))
            {
                _imagePickerController.Canceled += (sender, e) => { _imagePickerController.DismissViewController(true, null); };
                _imagePickerController.SourceType = UIImagePickerControllerSourceType.Camera;
                _imagePickerController.AllowsEditing = true;
                this.PresentViewController(_imagePickerController, true, null);
            }
            else
            {
                var alert = UIAlertController.Create("Warning", "You don't have camera", UIAlertControllerStyle.Alert);
                alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
                this.PresentViewController(alert, true, null);
            }
        }


        public void OpenLibrary()
        {
            _imagePickerController.Canceled += (sender, e) => { _imagePickerController.DismissViewController(true, null); };
            _imagePickerController.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
            _imagePickerController.AllowsEditing = true;

            this.PresentViewController(_imagePickerController, true, null);
        }

        public void Canceled(object sender, EventArgs e)
        {
            _imagePickerController.DismissViewController(true, null);
        }

        protected void Handle_FinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs e)
        {
            UIImage originalImage = e.Info[UIImagePickerController.EditedImage] as UIImage;

            if (originalImage != null)
            {
                TaskImage.Image = originalImage;
            }

            _imagePickerController.DismissViewController(true, null);
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
            ImportFromDocMenu(sender);
        }

        #endregion

        #region Document picker's actions

        [Export("importFromDocPicker:")]
        public void ImportFromDocPicker(UIButton sender)
        {
            UIDocumentPickerViewController vc = new UIDocumentPickerViewController(_allowedUTIs, UIDocumentPickerMode.Import);
            vc.WasCancelled += OnPickerCancel;
            vc.DidPickDocument += DidPickDocumentForImport;
            PresentViewController(vc, true, null);
        }

        private void DocumentPicker_DidPickDocumentAtUrls(object sender, UIDocumentPickedAtUrlsEventArgs e)
        {
            SaveFile(e.Urls[0]);
        }

        void DidPickDocumentForImport(object sender, UIDocumentPickedEventArgs e)
        {
            // The url refers to a copy of the selected document.
            // This document is a temporary file.
            // It remains available only until your application terminates.
            // To keep a permanent copy, you must move this file to a permanent location inside your sandbox.
            NSUrl temporaryFileUrl = e.Url;
            PrintFileContent(temporaryFileUrl);
        }

        [Export("exportToDocPicker:")]
        public void ExportToDocPicker(UIButton sender)
        {
            if (TryShowFileNotExistsError())
                return;

            UIDocumentPickerViewController vc = new UIDocumentPickerViewController(_documentURL, UIDocumentPickerMode.ExportToService);
            vc.WasCancelled += OnPickerCancel;
            vc.DidPickDocument += DidPickDocumentForExport;

            PresentViewController(vc, true, null);
        }

        void DidPickDocumentForExport(object sender, UIDocumentPickedEventArgs e)
        {
            // The URL refers to the new copy of the exported document at the selected destination.
            // This URL refers to a file outside your app’s sandbox.
            // You cannot access this copy; the URL is passed only to indicate success.
            NSUrl url = e.Url;
            Console.WriteLine("{0} exported to new location outside your app’s sandbox {1}", _documentURL, url);
        }

        [Export("openDocPicker:")]
        public void OpenDocPicker(UIButton sender)
        {
            UIDocumentPickerViewController vc = new UIDocumentPickerViewController(_allowedUTIs, UIDocumentPickerMode.Open);
            vc.WasCancelled += OnPickerCancel;
            vc.DidPickDocument += DidPickDocumentForOpen;
            PresentViewController(vc, true, null);
        }

        void DidPickDocumentForOpen(object sender, UIDocumentPickedEventArgs e)
        {
           
            var securityScopedUrl = e.Url;
            PrintOutsideFileContent(securityScopedUrl);
        }

        [Export("moveToDocPicker:")]
        private void MoveToDocPicker(UIButton sender)
        {
            if (TryShowFileNotExistsError())
                return;

            UIDocumentPickerViewController vc = new UIDocumentPickerViewController(_documentURL, UIDocumentPickerMode.MoveToService);
            vc.WasCancelled += OnPickerCancel;
            vc.DidPickDocument += DidPickDocumentForMove;
            PresentViewController(vc, true, null);
        }

        void DidPickDocumentForMove(object sender, UIDocumentPickedEventArgs e)
        {
            NSUrl securityScopedUrl = e.Url;
            PrintOutsideFileContent(securityScopedUrl);
        }

        void PrintOutsideFileContent(NSUrl securityScopedUrl)
        {
            if (!securityScopedUrl.StartAccessingSecurityScopedResource())
                return;

            PrintFileContent(securityScopedUrl);

            securityScopedUrl.StopAccessingSecurityScopedResource();
        }

        void PrintFileContent(NSUrl url)
        {
            NSData data = null;
            NSError error = null;
            NSFileCoordinator fileCoordinator = new NSFileCoordinator();
            fileCoordinator.CoordinateRead(url, (NSFileCoordinatorReadingOptions)0, out error, newUrl => {
                data = NSData.FromUrl(newUrl);
            });

            if (error != null)
            {
                Console.WriteLine("CoordinateRead error {0}", error);
            }
            else
            {
                Console.WriteLine("File name: {0}", url.LastPathComponent);
                Console.WriteLine(data);
            }
        }

        void SaveFile(NSUrl url)
        {
            var file = new FileItemViewModel();
            file.FileContent = System.IO.File.ReadAllBytes(url.Path);
            file.FileName = System.IO.Path.GetFileNameWithoutExtension(url.Path);
            file.FileExtension = url.PathExtension;
            ViewModel.AddFileCommand.Execute(file);
        }

        #endregion

        #region Documnet menu's actions

        [Export("importFromDocMenu:")]
        public void ImportFromDocMenu(UIButton sender)
        {
            UIDocumentMenuViewController vc = new UIDocumentMenuViewController(_allowedUTIs, UIDocumentPickerMode.Import);
            SetupDelegateThenPresent(vc, sender);
        }

        [Export("openDocMenu:")]
        public void OpenDocMenu(UIButton sender)
        {
            UIDocumentMenuViewController vc = new UIDocumentMenuViewController(_allowedUTIs, UIDocumentPickerMode.Open);
            SetupDelegateThenPresent(vc, sender);
        }

        [Export("exportToDocMenu:")]
        public void ExportToDocMenu(UIButton sender)
        {
            if (TryShowFileNotExistsError())
                return;

            UIDocumentMenuViewController vc = new UIDocumentMenuViewController(_documentURL, UIDocumentPickerMode.ExportToService);
            SetupDelegateThenPresent(vc, sender);
        }

        [Export("moveToDocMenu:")]
        public void MoveToDocMenu(UIButton sender)
        {
            if (TryShowFileNotExistsError())
                return;

            UIDocumentMenuViewController vc = new UIDocumentMenuViewController(_documentURL, UIDocumentPickerMode.MoveToService);
            SetupDelegateThenPresent(vc, sender);
        }

        void SetupDelegateThenPresent(UIDocumentMenuViewController vc, UIButton button)
        {
            vc.WasCancelled += OnPickerSelectionCancel;
            vc.DidPickDocumentPicker += OnPickerPicked;

            vc.AddOption("Custom Option", null, UIDocumentMenuOrder.First, () => {
                Console.WriteLine("completionHandler Hit");
            });

            vc.ModalPresentationStyle = UIModalPresentationStyle.Popover;
            PresentViewController(vc, true, null);

            UIPopoverPresentationController presentationPopover = vc.PopoverPresentationController;
            if (presentationPopover != null)
            {
                presentationPopover.SourceView = View;
                presentationPopover.PermittedArrowDirections = UIPopoverArrowDirection.Down;
                presentationPopover.SourceRect = button.Frame;
            }
        }

        #endregion

        #region Document menu's handlers

        void OnPickerSelectionCancel(object sender, EventArgs e)
        {
            var menu = (UIDocumentMenuViewController)sender;
            Unsibscribe(menu);

            Console.WriteLine("Picker selection was canceled");
        }

        void OnPickerPicked(object sender, UIDocumentMenuDocumentPickedEventArgs e)
        {
            var menu = (UIDocumentMenuViewController)sender;
            Unsibscribe(menu);

            var documentPicker = e.DocumentPicker;
            documentPicker.WasCancelled += OnPickerCancel;
            switch (documentPicker.DocumentPickerMode)
            {
                case UIDocumentPickerMode.Import:
                    documentPicker.DidPickDocument += DidPickDocumentForImport;
                    documentPicker.DidPickDocumentAtUrls += DocumentPicker_DidPickDocumentAtUrls;
                    break;

                case UIDocumentPickerMode.Open:
                    documentPicker.DidPickDocument += DidPickDocumentForOpen;
                    break;

                case UIDocumentPickerMode.ExportToService:
                    documentPicker.DidPickDocument += DidPickDocumentForExport;
                    break;

                case UIDocumentPickerMode.MoveToService:
                    documentPicker.DidPickDocument += DidPickDocumentForMove;
                    break;
            }

            PresentViewController(documentPicker, true, null);
        }

        #endregion

        void OnPickerCancel(object sender, EventArgs e)
        {
            Console.WriteLine("Cancel pick document");
        }

        bool TryShowFileNotExistsError()
        {
            if (NSFileManager.DefaultManager.FileExists(_documentURL.Path))
                return false;

            UIAlertController alert = UIAlertController.Create(_documentURL.LastPathComponent, "File doesn't exist. Maybe you moved or Exported it earlier. Re run the app", UIAlertControllerStyle.Alert);
            alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
            PresentViewController(alert, true, null);
            return true;
        }

        void Unsibscribe(UIDocumentMenuViewController menu)
        {
            menu.WasCancelled -= OnPickerSelectionCancel;
            menu.DidPickDocumentPicker -= OnPickerPicked;
        }
    }
}
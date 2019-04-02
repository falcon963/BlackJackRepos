using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using System;
using TestProject.Core.ViewModels;
using TestProject.iOS.Converters;
using UIKit;

namespace TestProject.iOS.Views
{
    [MvxModalPresentation]
    public partial class TaskDetailsView 
        : BaseMenuView<TaskViewModel>
    {

        private UITapGestureRecognizer _tap;



        UIImagePickerController _imagePickerController = new UIImagePickerController();

        public override UIScrollView ScrollView { get => base.ScrollView; set => base.ScrollView = value; }

        public TaskDetailsView() : base("TaskDetailsView", null)
        {
            HideKeyboard(_tap);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ScrollView = MainScrollView;

            var set = this.CreateBindingSet<TaskDetailsView, TaskViewModel>();
            set.Bind(TaskName).To(vm => vm.UserTask.Changes.Title);
            set.Bind(TaskName).For(v => v.Enabled).To(vm => vm.TitleEnableStatus);
            set.Bind(TaskNote).To(vm => vm.UserTask.Changes.Note);
            set.Bind(TaskStatus).To(vm => vm.UserTask.Changes.Status);
            set.Bind(DeleteButton).To(vm => vm.DeleteUserTaskCommand);
            set.Bind(NavigationItem.Title).To(vm => vm.UserTask.Changes.Title);
            set.Bind(BackButton).To(vm => vm.ShowMenuCommand);
            set.Bind(View).For(v => v.BackgroundColor).To(vm => vm.ColorTheme).WithConversion(new ColorValueConverter());
            set.Bind(TaskImage).To(vm => vm.UserTask.Changes.ImagePath).WithConversion(new ImageValueConverter());
            set.Apply();

            AddShadow(TaskName);
            AddShadow(TaskNote);
            AddShadow(TaskImage);
            AddShadow(DeleteButton);
            AddShadow(SaveButton);
            AddShadow(TaskStatus);



            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidHideNotification, HandleKeyboardDidHide);

            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, HandleKeyboardDidShow);

            UITapGestureRecognizer recognizer = new UITapGestureRecognizer(OpenImage);
            TaskImage.AddGestureRecognizer(recognizer);

            BackButton.Image = UIImage.FromFile("back_to_50.png");

            MainScrollView.ContentSize = new CoreGraphics.CGSize(0, MainScrollView.Frame.Height);
            TaskNote.ContentInset = UIEdgeInsets.Zero;
            TaskNote.ClipsToBounds = true;

            _imagePickerController.Delegate = this;

            this.AutomaticallyAdjustsScrollViewInsets = false;
        }

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

        partial void PressSaveButton(UIButton sender)
        {
            var data = TaskImage.Image.AsJPEG();
            ViewModel.UserTask.Changes.ImagePath = data.GetBase64EncodedString(NSDataBase64EncodingOptions.SixtyFourCharacterLineLength);
            ViewModel.SaveUserTaskCommand.Execute();
        }


        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        public void Canceled(object sender, EventArgs e)
        {
            _imagePickerController.DismissViewController(true, null);
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

        public override void HandleKeyboardDidShow(NSNotification obj)
        {
            base.HandleKeyboardDidShow(obj);
        }

        public override void HandleKeyboardDidHide(NSNotification obj)
        {
            base.HandleKeyboardDidHide(obj);
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
    }
}
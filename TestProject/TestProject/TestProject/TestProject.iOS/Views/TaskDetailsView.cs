using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using System;
using TestProject.Core.ViewModels;
using UIKit;

namespace TestProject.iOS.Views
{
    public partial class TaskDetailsView : BaseMenuView<TaskViewModel>
    {

        private UITapGestureRecognizer _tap;

        UIImagePickerController imagePickerController = new UIImagePickerController();

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
            set.Bind(SaveButton).To(vm => vm.SaveUserTaskCommand);
            set.Bind(NavigationItem.Title).To(vm => vm.UserTask.Changes.Title);
            set.Bind(BackButton).To(vm => vm.ShowMenuCommand);
            set.Bind(View).For(v => v.BackgroundColor).To(vm => vm.ColorTheme).WithConversion("NativeColor");
            set.Apply();

            AddShadow(TaskName);
            AddShadow(TaskNote);
            AddShadow(TaskImage);
            AddShadow(DeleteButton);
            AddShadow(SaveButton);
            AddShadow(ImageChoseButton);
            AddShadow(TaskStatus);

            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidHideNotification, HandleKeyboardDidHide);

            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, HandleKeyboardDidShow);

            var imagePath = ViewModel.UserTask.Changes.ImagePath;
            if (imagePath != null)
            {
                TaskImage.Image = UIImage.LoadFromData(imagePath);
            }
            if(imagePath == null)
            {
                TaskImage.Image = UIImage.FromFile("placeholder.png");
            }
            BackButton.Image = UIImage.FromFile("back_to_50.png");

            UITapGestureRecognizer imageButton = new UITapGestureRecognizer(ImageChose);
            ImageChoseButton.AddGestureRecognizer(imageButton);

            this.AutomaticallyAdjustsScrollViewInsets = false;
        }

        private void ImageChose()
        {
            imagePickerController.Delegate = this;

            var actionSheet = UIAlertController.Create("Photo Source", "Choose a source", UIAlertControllerStyle.ActionSheet);

            actionSheet.AddAction(UIAlertAction.Create("Camera", UIAlertActionStyle.Default, (actionCamera) =>
            {
                imagePickerController.SourceType = UIImagePickerControllerSourceType.Camera;
            }));

            actionSheet.AddAction(UIAlertAction.Create("Gallery", UIAlertActionStyle.Default, (actionCamera) =>
            {
                imagePickerController.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;

            }));

            actionSheet.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));

            var viewController = this.Window.RootViewController;
            imagePickerController.View.Frame = viewController.View.Frame;
            this.PresentViewController(actionSheet, true, null);
            imagePickerController.Canceled += Canceled;
            imagePickerController.FinishedPickingMedia += FinishedPickingMedia;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        public void Canceled(object sender, EventArgs e)
        {
            imagePickerController.DismissViewController(true, null);
        }

        public void FinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs e)
        {
            UIImage originalImage = e.Info[UIImagePickerController.OriginalImage] as UIImage;
            if(originalImage != null)
            {
                TaskImage.Image = originalImage;
            }
            imagePickerController.DismissViewController(true, null);
        }

        public override void HandleKeyboardDidShow(NSNotification obj)
        {
            base.HandleKeyboardDidShow(obj);
        }

        public override void HandleKeyboardDidHide(NSNotification obj)
        {
            base.HandleKeyboardDidHide(obj);
        }
    }
}
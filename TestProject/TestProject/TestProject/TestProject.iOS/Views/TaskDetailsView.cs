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

        public TaskDetailsView() : base("TaskDetailsView", null)
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = this.CreateBindingSet<TaskDetailsView, TaskViewModel>();
            set.Bind(TaskName).To(vm => vm.UserTask.Changes.Title);
            set.Bind(TaskName).For(v => v.Enabled).To(vm => vm.TitleEnableStatus);
            set.Bind(TaskNote).To(vm => vm.UserTask.Changes.Note);
            set.Bind(TaskStatus).To(vm => vm.UserTask.Changes.Status);
            set.Bind(DeleteButton).To(vm => vm.DeleteUserTaskCommand);
            set.Bind(SaveButton).To(vm => vm.SaveUserTaskCommand);
            set.Bind(NavigationItem.Title).To(vm => vm.UserTask.Changes.Title);
            set.Bind(BackButton).To(vm => vm.ShowMenuCommand);
            set.Apply();

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
        }

        private void ImageChose()
        {
            var imagePickerController = new UIImagePickerController();
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
            this.PresentViewController(actionSheet, true, null);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        public void Canceled(UIImagePickerController picker)
        {
            picker.DismissViewController(true, null);
        }

        public void FinishedPickingMedia(UIImagePickerController picker, NSDictionary info)
        {
            var originalImage = new NSString("UIImagePickerControllerOriginalImage");
            var image = (UIImage)info[originalImage];
            TaskImage.Image = image;

            picker.DismissViewController(true, null);
        }
    }
}
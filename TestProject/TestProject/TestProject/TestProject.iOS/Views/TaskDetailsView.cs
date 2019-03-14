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
            set.Bind(SaveButton).To(vm => vm.SaveUserTaskCommand);
            set.Bind(NavigationItem.Title).To(vm => vm.UserTask.Changes.Title);
            set.Bind(BackButton).To(vm => vm.ShowMenuCommand);
            set.Bind(View).For(v => v.BackgroundColor).To(vm => vm.ColorTheme).WithConversion("NativeColor");
            set.Bind(TaskImage).To(vm => vm.UserTask.Changes.ImagePath).WithConversion(new ImageValueConverter());
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

            
            BackButton.Image = UIImage.FromFile("back_to_50.png");

            _imagePickerController.Delegate = this;

            this.AutomaticallyAdjustsScrollViewInsets = false;
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



        partial void PressImgButton(UIButton sender)
        {
            this.ImageChoseButton.UserInteractionEnabled = true;


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

            switch (UIDevice.CurrentDevice.UserInterfaceIdiom)
            {
                case UIUserInterfaceIdiom.Pad:
                    actionSheet.PopoverPresentationController.SourceView = sender;
                    actionSheet.PopoverPresentationController.SourceRect = sender.Bounds;
                    actionSheet.PopoverPresentationController.PermittedArrowDirections = UIPopoverArrowDirection.Up;
                    break;
                default:
                    break;
            }

            _imagePickerController.FinishedPickingMedia += Handle_FinishedPickingMedia;

            this.PresentViewController(actionSheet, true, null);
        }

        public void OpenLibrary()
        {
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
            UIImage originalImage = e.Info[UIImagePickerController.OriginalImage] as UIImage;

            if (originalImage != null)
            {
                TaskImage.Image = originalImage;
            }

            _imagePickerController.DismissViewController(true, null);
        }
    }
}
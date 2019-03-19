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
    public partial class ProfileView 
        : BaseMenuView<UserProfileViewModel>
    {
        private UITapGestureRecognizer _tap;

        private UIImagePickerController _imagePickerController = new UIImagePickerController();

        public override UIScrollView ScrollView { get => base.ScrollView; set => base.ScrollView = value; }

        public ProfileView () : base ("ProfileView", null)
        {
            HideKeyboard(_tap);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //NavigationController.NavigationBar.Hidden = true;

            ScrollView = MainScrollView;

            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidHideNotification, HandleKeyboardDidHide);

            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, HandleKeyboardDidShow);

            var set = this.CreateBindingSet<ProfileView, UserProfileViewModel>();

            set.Bind(PasswordField).To(vm => vm.OldPassword);
            set.Bind(PasswordConfirmField).To(vm => vm.ConfirmPassword);
            set.Bind(NewPasswordField).To(vm => vm.NewPassword);
            set.Bind(BackButton).To(vm => vm.CloseProfileCommand);
            set.Bind(SaveNewPasswordButton).To(vm => vm.SavePasswordChangeCommand);
            set.Bind(MainScrollView).For(v => v.BackgroundColor).To(vm => vm.Background).WithConversion(new ColorValueConverter());
            set.Bind(ProfileImage).To(vm => vm.Profile.ImagePath).WithConversion(new ImageValueConverter());
            set.Apply();

            UITapGestureRecognizer recognizer = new UITapGestureRecognizer(OpenImage);
            ProfileImage.AddGestureRecognizer(recognizer);

            BackButton.Image = UIImage.FromFile("back_to_50.png");

            MainScrollView.ContentSize = new CoreGraphics.CGSize(0, View.Frame.Height - 20);

            this.AutomaticallyAdjustsScrollViewInsets = false;
        }

        public override void HandleKeyboardDidHide(NSNotification obj)
        {
            base.HandleKeyboardDidHide(obj);
        }

        public override void HandleKeyboardDidShow(NSNotification obj)
        {
            base.HandleKeyboardDidShow(obj);
        }

        public void OpenImage()
        {
            this.SaveImageButton.UserInteractionEnabled = true;


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

            _imagePickerController.Canceled += Canceled;

            _imagePickerController.FinishedPickingMedia += Handle_FinishedPickingMedia;

            this.PresentViewController(actionSheet, true, null);
        }

        partial void SaveImagePress(UIButton sender)
        {
            var data = ProfileImage.Image.AsJPEG();
            ViewModel.Profile.ImagePath = data.GetBase64EncodedString(NSDataBase64EncodingOptions.SixtyFourCharacterLineLength);
            this.ViewModel.SaveImageChangeCommand.Execute();
        }

        private void Canceled(object sender, EventArgs e)
        {
            _imagePickerController.DismissModalViewController(true);
        }

        private void Handle_FinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs e)
        {
            UIImage originalImage = e.Info[UIImagePickerController.OriginalImage] as UIImage;

            if (originalImage != null)
            {
                ProfileImage.Image = originalImage;
            }

            _imagePickerController.DismissViewController(true, null);
        }

        private void OpenLibrary()
        {
            _imagePickerController.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
            _imagePickerController.AllowsEditing = true;

            this.PresentViewController(_imagePickerController, true, null);
        }

        private void OpenCamera()
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

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }
    }
}
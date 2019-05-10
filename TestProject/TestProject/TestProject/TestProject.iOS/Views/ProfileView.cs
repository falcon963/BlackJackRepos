using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using System;
using TestProject.Core.ViewModels;
using TestProject.iOS.Converters;
using TestProject.iOS.Services;
using UIKit;

namespace TestProject.iOS.Views
{
    [MvxTabPresentation(WrapInNavigationController = true, TabName = "Profile", TabIconName = "profile")]
    public partial class ProfileView 
        : BaseView<ProfileView, ProfileViewModel>
    {
        private UITapGestureRecognizer _tap;

        private UIImagePickerController _imagePickerController = new UIImagePickerController();

        private readonly PhotoService<ProfileView, ProfileViewModel> _photoService;

        public ProfileView () : base ("ProfileView", null)
        {
            _photoService = new PhotoService<ProfileView, ProfileViewModel>(this, ProfileImage);
        }

        public override bool SetupBindings()
        {
            BindingSet.Bind(PasswordField).To(vm => vm.OldPassword);
            BindingSet.Bind(PasswordConfirmField).To(vm => vm.ConfirmPassword);
            BindingSet.Bind(NewPasswordField).To(vm => vm.NewPassword);
            BindingSet.Bind(MainScrollView).For(v => v.BackgroundColor).To(vm => vm.Background).WithConversion("NativeColor");
            BindingSet.Bind(ProfileImage).To(vm => vm.Profile.ImagePath).WithConversion(new ImageValueConverter());
            BindingSet.Bind(PasswordField).For(v => v.BackgroundColor).To(vm => vm.ConfirmPassword).WithConversion("NativeColor");
            BindingSet.Bind(NewPasswordField).For(v => v.BackgroundColor).To(vm => vm.ConfirmColor).WithConversion("NativeColor");
            BindingSet.Bind(PasswordConfirmField).For(v => v.BackgroundColor).To(vm => vm.ConfirmColor).WithConversion("NativeColor");
            BindingSet.Bind(LogoutButton).To(vm => vm.LogOutCommand);

            return base.SetupBindings();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidHideNotification, OnKeyboardWillHide);

            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, OnKeyboardWillShow);

            UITapGestureRecognizer recognizer = new UITapGestureRecognizer(_photoService.OpenImage);
            ProfileImage.AddGestureRecognizer(recognizer);

            AddShadow(PasswordField);
            AddShadow(NewPasswordField);
            AddShadow(PasswordConfirmField);
            AddShadow(SaveButton);
            AddShadow(LogoutButton);
            AddShadow(ProfileImage);

            MainScrollView.ContentSize = new CGSize(0, MainScrollView.Frame.Height - 80);

            this.AutomaticallyAdjustsScrollViewInsets = false;
        }


        partial void SaveImagePress(UIButton sender)
        {
            var data = ProfileImage.Image.AsJPEG();
            ViewModel.Profile.ImagePath = data.GetBase64EncodedString(NSDataBase64EncodingOptions.SixtyFourCharacterLineLength);
            this.ViewModel.UpdateProfileCommand.Execute();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }
    }
}
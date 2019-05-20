using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using System;
using TestProject.Core.ViewModels;
using TestProject.iOS.Converters;
using TestProject.iOS.Services;
using TestProject.iOS.Services.Interfaces;
using UIKit;

namespace TestProject.iOS.Views
{
    [MvxTabPresentation(WrapInNavigationController = true, TabName = "Profile", TabIconName = "profile")]
    public partial class ProfileView 
        : BaseView<ProfileView, ProfileViewModel>
    {
        private UIImagePickerController _imagePickerController = new UIImagePickerController();

        private readonly IPhotoService _photoService;

        #region ctor

        public ProfileView() : base(nameof(ProfileView), null)
        {

        }

        public ProfileView (IPhotoService photoService) : base (nameof(ProfileView), null)
        {
            _photoService = photoService;

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

        #endregion

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
            BindingSet.Bind(SaveButton).To(vm => vm.UpdateProfileCommand);
            BindingSet.Bind(LogoutButton).To(vm => vm.LogOutCommand);

            return base.SetupBindings();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidHideNotification, OnKeyboardWillHide);

            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, OnKeyboardWillShow);

            UITapGestureRecognizer recognizer = new UITapGestureRecognizer(() => { ViewModel?.PickPhotoCommand?.Execute(); });
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
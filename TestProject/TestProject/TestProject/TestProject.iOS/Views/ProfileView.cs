using CoreGraphics;
using Foundation;
using MvvmCross;
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
        private const float SpaceSize = 80f;

        private UIImagePickerController _imagePickerController = new UIImagePickerController();

        private readonly IPhotoService _photoService;

        #region ctor

        public ProfileView () : base (nameof(ProfileView), null)
        {
            _photoService = Mvx.IoCProvider.Resolve<IPhotoService>();

            base.ViewDidLoad();
        }

        public override bool SetupEvents()
        {
            _photoService.ImagePickerDelegateSubscription += _photoService_ImagePickerDelegateSubscription;

            _photoService.PresentPicker += _photoService_PresentPicker;

            _photoService.PresentAlert += _photoService_PresentAlert;

            UITapGestureRecognizer recognizer = new UITapGestureRecognizer(() => { ViewModel?.PickPhotoCommand?.Execute(); });

            ProfileImage.AddGestureRecognizer(recognizer);

            return base.SetupEvents();
        }

        private void _photoService_PresentAlert(object sender, UIAlertController e)
        {
            PresentViewController(e, true, null);
        }

        private void _photoService_PresentPicker(object sender, UIImagePickerController e)
        {
            PresentViewController(e, true, null);
        }

        private void _photoService_ImagePickerDelegateSubscription(object sender, NSObject e)
        {
            e = this;
        }

        #endregion

        public override bool SetupBindings()
        {
            BindingSet.Bind(PasswordField).To(vm => vm.OldPassword);
            BindingSet.Bind(PasswordConfirmField).To(vm => vm.ConfirmPassword);
            BindingSet.Bind(NewPasswordField).To(vm => vm.NewPassword);
            BindingSet.Bind(MainScrollView).For(v => v.BackgroundColor).To(vm => vm.Background).WithConversion("NativeColor");
            BindingSet.Bind(ProfileImage).To(vm => vm.ProfileImage).WithConversion(new ImageValueConverter());
            BindingSet.Bind(PasswordField).For(v => v.BackgroundColor).To(vm => vm.ConfirmPassword).WithConversion("NativeColor");
            BindingSet.Bind(NewPasswordField).For(v => v.BackgroundColor).To(vm => vm.ConfirmColor).WithConversion("NativeColor");
            BindingSet.Bind(PasswordConfirmField).For(v => v.BackgroundColor).To(vm => vm.ConfirmColor).WithConversion("NativeColor");
            BindingSet.Bind(SaveButton).To(vm => vm.UpdateProfileCommand);
            BindingSet.Bind(LogoutButton).To(vm => vm.LogOutCommand);

            return base.SetupBindings();
        }

        public override void ViewDidLoad()
        {
            ScrollView = MainScrollView;

            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidHideNotification, HandleKeyboardDidHide);

            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, HandleKeyboardDidShow);

            AddShadow(PasswordField);
            AddShadow(NewPasswordField);
            AddShadow(PasswordConfirmField);
            AddShadow(SaveButton);
            AddShadow(LogoutButton);
            AddShadow(ProfileImage);

            MainScrollView.ContentSize = new CGSize(0, MainScrollView.Frame.Height - SpaceSize);

            AutomaticallyAdjustsScrollViewInsets = false;

        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        protected override void Dispose(bool disposing)
        {
            _photoService.ImagePickerDelegateSubscription -= _photoService_ImagePickerDelegateSubscription;

            _photoService.PresentPicker -= _photoService_PresentPicker;

            _photoService.PresentAlert -= _photoService_PresentAlert;

            base.Dispose(disposing);
        }
    }
}
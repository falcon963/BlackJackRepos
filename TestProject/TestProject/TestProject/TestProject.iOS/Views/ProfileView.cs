using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Views;
using System;
using TestProject.Core.ViewModels;
using UIKit;

namespace TestProject.iOS.Views
{
    public partial class ProfileView : BaseMenuView<UserProfileViewModel>
    {
        private UITapGestureRecognizer _tap;

        public override UIScrollView ScrollView { get => base.ScrollView; set => base.ScrollView = value; }

        public ProfileView () : base ("ProfileView", null)
        {
            HideKeyboard(_tap);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ScrollView = MainScrollView;

            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidHideNotification, HandleKeyboardDidHide);

            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, HandleKeyboardDidShow);

            var set = this.CreateBindingSet<ProfileView, UserProfileViewModel>();

            set.Bind(PasswordField).To(vm => vm.OldPassword);
            set.Bind(PasswordConfirmField).To(vm => vm.ConfirmPassword);
            set.Bind(NewPasswordField).To(vm => vm.NewPassword);
            set.Bind(BackButton).To(vm => vm.CloseProfileCommand);
            set.Bind(SaveImageButton).To(vm => vm.SaveImageChangeCommand);
            set.Bind(SaveNewPasswordButton).To(vm => vm.SavePasswordChangeCommand);
            set.Apply();

            BackButton.Image = UIImage.FromFile("back_to_50.png");
        }

        public override void HandleKeyboardDidHide(NSNotification obj)
        {
            base.HandleKeyboardDidHide(obj);
        }

        public override void HandleKeyboardDidShow(NSNotification obj)
        {
            base.HandleKeyboardDidShow(obj);
        }
    }
}
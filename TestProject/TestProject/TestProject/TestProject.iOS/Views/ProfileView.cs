using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Views;
using System;
using TestProject.Core.ViewModels;
using UIKit;

namespace TestProject.iOS.Views
{
    public partial class ProfileView : MvxViewController<UserProfileViewModel>
    {
        public ProfileView (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

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
    }
}
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

            set.Bind(PasswordField).For(p => p.Text).To(vm => vm.OldPassword);
            set.Bind(PasswordConfirmField).For(p => p.Text).To(vm => vm.ConfirmPassword);
            set.Bind(NewPasswordField).For(p => p.Text).To(vm => vm.NewPassword);

            set.Apply();
        }

        partial void SaveImagePress(UIButton sender)
        {
            ViewModel.SaveImageChangeCommand.Execute();
        }

        partial void SavePasswordPress(UIButton sender)
        {
            ViewModel.SavePasswordChangeCommand.Execute();
        }


    }
}
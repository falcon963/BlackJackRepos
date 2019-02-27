using Foundation;
using MvvmCross.Platforms.Ios.Views;
using System;
using TestProject.Core.ViewModels;
using UIKit;

namespace TestProject.iOS.ViewControllers
{
    public partial class RegistrationViewController : MvxViewController<RegistrationViewModel>
    {
        public RegistrationViewController (IntPtr handle) : base (handle)
        {
        }

        public RegistrationViewController() : base("RegistrationViewController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //this.DelayBind(() =>
            //{
            //    this.AddBindings(LoginRegistrationField, "Text Login");
            //    this.AddBindings(PasswordRegistrationField, "Text Password");
            //    this.AddBindings(VerificationPasswordField, "Text PasswordRevise");
            //});

            //RegistrationButton.Enabled = ViewModel.EnableStatus;
        }
    }
}
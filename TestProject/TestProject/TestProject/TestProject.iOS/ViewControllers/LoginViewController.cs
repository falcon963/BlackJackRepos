
using System;
using System.Drawing;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Views;
using TestProject.Core.ViewModels;
using UIKit;

namespace TestProject.iOS.ViewControllers
{
    public partial class LoginViewController : MvxViewController<LoginViewModel>
    {
        public LoginViewController(IntPtr handle) : base(handle)
        {
        }

        public LoginViewController() : base("LoginViewController", null)
        {
        }


        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        #region View lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = this.CreateBindingSet<LoginViewController, LoginViewModel>();
            //set.Bind(RegistrationButton).To(vm => vm.LoginCommand);
            set.Apply();

            //this.DelayBind(() => {
            //    this.AddBindings(LoginField, "Text Login");
            //    this.AddBindings(PasswordField, "Text Password");

            //    LoginButton.Enabled = ViewModel.EnableStatus;
            //});
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
        }

        #endregion
    }
}
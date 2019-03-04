using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Views;
using System;
using TestProject.Core.ViewModels;
using UIKit;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using System.Drawing;
using CoreGraphics;

namespace TestProject.iOS.Views
{
    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class LoginView
        : MvxViewController<LoginViewModel>
    {
        private UITapGestureRecognizer _tap;

        public LoginView(IntPtr handle) : base(handle)
        {
        }

        public LoginView() : base("LoginView", null)
        {
            HideKeyboard();
        }



        public override void ViewDidLoad()
        {
            base.ViewDidLoad();


            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidHideNotification, HandleKeyboardDidHide);

            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, HandleKeyboardDidShow);

            var set = this.CreateBindingSet<LoginView, LoginViewModel>();
            set.Bind(RegistrationButton).To(vm => vm.GoRegistrationPageCommand);
            set.Bind(LoginButton).To(vm => vm.LoginCommand);
            set.Bind(LoginField).To(vm => vm.Login);
            set.Bind(PasswordField).To(vm => vm.Password);
            set.Bind(RememberSwitch).To(vm => vm.RememberMeStatus);

            set.Apply();

        }

        private void HandleKeyboardDidShow(NSNotification obj)
        {
            LoginScrollView.ContentSize = new CoreGraphics.CGSize(View.Frame.Width, View.Frame.Height + UIKeyboard.FrameBeginFromNotification(obj).Height / 2);
        }

        private void HandleKeyboardDidHide(NSNotification obj)
        {
            LoginScrollView.ContentSize = new CoreGraphics.CGSize(View.Frame.Width, View.Frame.Height - UIKeyboard.FrameBeginFromNotification(obj).Height / 2);
        }

        public void HideKeyboard()
        {
            _tap = new UITapGestureRecognizer();
            _tap.AddTarget(() =>
            {
                View.EndEditing(true);
            });
            _tap.CancelsTouchesInView = false;
            View.AddGestureRecognizer(_tap);
        }


    }
}

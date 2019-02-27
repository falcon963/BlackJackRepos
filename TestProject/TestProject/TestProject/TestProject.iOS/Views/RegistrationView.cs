using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Views;
using System;
using TestProject.Core.ViewModels;
using UIKit;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using System.Drawing;

namespace TestProject.iOS.Views
{
    public partial class RegistrationView
        :MvxViewController<RegistrationViewModel>
    {

        UITapGestureRecognizer _tap;


        public RegistrationView(IntPtr handle) : base(handle)
        {
        }

        public RegistrationView() : base("RegistrationView", null)
        {
            HideKeyboard();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

           // NavigationController.NavigationBarHidden = true;

            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidHideNotification, HandleKeyboardDidHide);

            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, HandleKeyboardDidShow);

            var set = this.CreateBindingSet<RegistrationView, RegistrationViewModel>();
            set.Bind(RegistrationButton).To(vm => vm.RegistrationCommand);
            set.Bind(LoginField).For(f => f.Text).To(vm => vm.Login);
            set.Bind(PasswordField).For(f => f.Text).To(vm => vm.Password);
            set.Bind(PasswordConfirmField).For(f => f.Text).To(vm => vm.PasswordRevise);
            set.Apply();
            
        }

        private void HandleKeyboardDidShow(NSNotification obj)
        {
            RegistrationScrollView.ContentSize = new CoreGraphics.CGSize(View.Frame.Width, View.Frame.Height + UIKeyboard.FrameBeginFromNotification(obj).Height/2);
        }

        private void HandleKeyboardDidHide(NSNotification obj)
        {
            RegistrationScrollView.ContentSize = new CoreGraphics.CGSize(View.Frame.Width, View.Frame.Height - UIKeyboard.FrameBeginFromNotification(obj).Height/2);
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

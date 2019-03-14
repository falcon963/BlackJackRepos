using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Views;
using System;
using TestProject.Core.ViewModels;
using UIKit;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using System.Drawing;
using CoreGraphics;
using TestProject.iOS.Converters;

namespace TestProject.iOS.Views
{
    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class LoginView
        : BaseMenuView<LoginViewModel>
    {
        private UITapGestureRecognizer _tap;

        public override UIScrollView ScrollView { get => base.ScrollView; set => base.ScrollView = value; }

        public LoginView() : base("LoginView", null)
        {
            HideKeyboard(_tap);
        }



        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ScrollView = LoginScrollView;

            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidHideNotification, HandleKeyboardDidHide);

            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, HandleKeyboardDidShow);

            var set = this.CreateBindingSet<LoginView, LoginViewModel>();
            set.Bind(RegistrationButton).To(vm => vm.GoRegistrationPageCommand);
            set.Bind(LoginButton).To(vm => vm.LoginCommand);
            set.Bind(LoginField).To(vm => vm.Login);
            set.Bind(PasswordField).To(vm => vm.Password);
            set.Bind(RememberSwitch).To(vm => vm.RememberMeStatus);
            //set.Bind(LoginScrollView).For("Visibility").To(vm => vm.LoginColor).WithConversion(new ColorValueConverter());
            set.Bind(LoginScrollView).For(v => v.BackgroundColor).To(vm => vm.LoginColor).WithConversion(new ColorValueConverter());
            //set.Bind(LoginScrollView).For(v => v.BackgroundColor).To(vm => vm.LoginColor).WithConversion("RGBA");
            //set.Bind(View).For(v => v.BackgroundColor).To(vm => vm.LoginColor).WithConversion(new ColorValueConverter());
            set.Apply();

            AddShadow(LoginField);
            AddShadow(PasswordField);
            AddShadow(LoginButton);
            AddShadow(RegistrationButton);
            AddShadow(RememberSwitch);

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

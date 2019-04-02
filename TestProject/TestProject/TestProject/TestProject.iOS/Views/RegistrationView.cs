using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Views;
using System;
using TestProject.Core.ViewModels;
using UIKit;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using System.Drawing;
using TestProject.iOS.Converters;
using CoreGraphics;

namespace TestProject.iOS.Views
{
    public partial class RegistrationView
        :BaseMenuView<RegistrationViewModel>
    {

        UITapGestureRecognizer _tap;

        public override UIScrollView ScrollView { get => base.ScrollView; set => base.ScrollView = value; }


        public RegistrationView() : base("RegistrationView", null)
        {
            HideKeyboard(_tap);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ScrollView = RegistrationScrollView;

            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidHideNotification, HandleKeyboardDidHide);

            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, HandleKeyboardDidShow);

            var set = this.CreateBindingSet<RegistrationView, RegistrationViewModel>();
            set.Bind(RegistrationButton).To(vm => vm.RegistrationCommand);
            set.Bind(LoginField).To(vm => vm.Login);
            set.Bind(PasswordField).To(vm => vm.Password);
            set.Bind(PasswordConfirmField).To(vm => vm.PasswordRevise);
            set.Bind(LoginField).For(v => v.BackgroundColor).To(vm => vm.LoginEnebleColor).WithConversion(new ColorValueConverter());
            set.Bind(PasswordField).For(v => v.BackgroundColor).To(vm => vm.ValidateColor).WithConversion(new ColorValueConverter());
            set.Bind(PasswordConfirmField).For(v => v.BackgroundColor).To(vm => vm.ValidateColor).WithConversion(new ColorValueConverter());
            set.Bind(View).For(v => v.BackgroundColor).To(vm => vm.LoginColor).WithConversion(new ColorValueConverter());
            set.Apply();

            AddShadow(LoginField);
            AddShadow(PasswordField);
            AddShadow(PasswordConfirmField);
            AddShadow(RegistrationButton);
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

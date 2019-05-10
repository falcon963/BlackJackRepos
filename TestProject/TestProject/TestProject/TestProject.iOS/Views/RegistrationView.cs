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
        :BaseView<RegistrationView ,RegistrationViewModel>
    {


        public RegistrationView() : base("RegistrationView", null)
        {
            ;
        }

        public override bool SetupBindings()
        {
            BindingSet.Bind(RegistrationButton).To(vm => vm.RegistrationCommand);
            BindingSet.Bind(LoginField).To(vm => vm.Login);
            BindingSet.Bind(PasswordField).To(vm => vm.Password);
            BindingSet.Bind(PasswordConfirmField).To(vm => vm.PasswordConfirmation);
            BindingSet.Bind(LoginField).For(v => v.BackgroundColor).To(vm => vm.LoginEnebleColor).WithConversion("NativeColor");
            BindingSet.Bind(PasswordField).For(v => v.BackgroundColor).To(vm => vm.ValidateColor).WithConversion("NativeColor");
            BindingSet.Bind(PasswordConfirmField).For(v => v.BackgroundColor).To(vm => vm.ValidateColor).WithConversion("NativeColor");
            BindingSet.Bind(View).For(v => v.BackgroundColor).To(vm => vm.LoginColor).WithConversion("NativeColor");

            return base.SetupBindings();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();


            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidHideNotification, OnKeyboardWillHide);

            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, OnKeyboardWillShow);

            AddShadow(LoginField);
            AddShadow(PasswordField);
            AddShadow(PasswordConfirmField);
            AddShadow(RegistrationButton);
        }

    }


}

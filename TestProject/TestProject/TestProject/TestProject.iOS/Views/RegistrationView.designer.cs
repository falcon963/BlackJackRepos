// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace TestProject.iOS.Views
{
    [Register ("RegistrationView")]
    partial class RegistrationView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ConfirmPasswordView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField LoginField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView LoginView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView MainView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField PasswordConfirmField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField PasswordField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView PasswordView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton RegistrationButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView RegistrationScrollView { get; set; }

        [Action ("RegistratePress:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void RegistratePress (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (ConfirmPasswordView != null) {
                ConfirmPasswordView.Dispose ();
                ConfirmPasswordView = null;
            }

            if (LoginField != null) {
                LoginField.Dispose ();
                LoginField = null;
            }

            if (LoginView != null) {
                LoginView.Dispose ();
                LoginView = null;
            }

            if (MainView != null) {
                MainView.Dispose ();
                MainView = null;
            }

            if (PasswordConfirmField != null) {
                PasswordConfirmField.Dispose ();
                PasswordConfirmField = null;
            }

            if (PasswordField != null) {
                PasswordField.Dispose ();
                PasswordField = null;
            }

            if (PasswordView != null) {
                PasswordView.Dispose ();
                PasswordView = null;
            }

            if (RegistrationButton != null) {
                RegistrationButton.Dispose ();
                RegistrationButton = null;
            }

            if (RegistrationScrollView != null) {
                RegistrationScrollView.Dispose ();
                RegistrationScrollView = null;
            }
        }
    }
}
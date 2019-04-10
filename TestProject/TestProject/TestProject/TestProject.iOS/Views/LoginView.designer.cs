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
    [Register ("LoginView")]
    partial class LoginView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ButtonsView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton LoginButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView LoginFacebookButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField LoginField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView LoginFieldView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView LoginGoogleButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView LoginScrollView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField PasswordField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView PasswordFieldView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton RegistrationButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch RememberSwitch { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ShadowView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ShadowViewPasswordField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView SocialView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView SwitchView { get; set; }

        [Action ("PressLogin:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void PressLogin (UIKit.UIButton sender);

        [Action ("PressRegistration:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void PressRegistration (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (ButtonsView != null) {
                ButtonsView.Dispose ();
                ButtonsView = null;
            }

            if (LoginButton != null) {
                LoginButton.Dispose ();
                LoginButton = null;
            }

            if (LoginFacebookButton != null) {
                LoginFacebookButton.Dispose ();
                LoginFacebookButton = null;
            }

            if (LoginField != null) {
                LoginField.Dispose ();
                LoginField = null;
            }

            if (LoginFieldView != null) {
                LoginFieldView.Dispose ();
                LoginFieldView = null;
            }

            if (LoginGoogleButton != null) {
                LoginGoogleButton.Dispose ();
                LoginGoogleButton = null;
            }

            if (LoginScrollView != null) {
                LoginScrollView.Dispose ();
                LoginScrollView = null;
            }

            if (PasswordField != null) {
                PasswordField.Dispose ();
                PasswordField = null;
            }

            if (PasswordFieldView != null) {
                PasswordFieldView.Dispose ();
                PasswordFieldView = null;
            }

            if (RegistrationButton != null) {
                RegistrationButton.Dispose ();
                RegistrationButton = null;
            }

            if (RememberSwitch != null) {
                RememberSwitch.Dispose ();
                RememberSwitch = null;
            }

            if (ShadowView != null) {
                ShadowView.Dispose ();
                ShadowView = null;
            }

            if (ShadowViewPasswordField != null) {
                ShadowViewPasswordField.Dispose ();
                ShadowViewPasswordField = null;
            }

            if (SocialView != null) {
                SocialView.Dispose ();
                SocialView = null;
            }

            if (SwitchView != null) {
                SwitchView.Dispose ();
                SwitchView = null;
            }
        }
    }
}
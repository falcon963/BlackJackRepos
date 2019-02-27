// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace TestProject.iOS.ViewControllers
{
    [Register ("RegistrationViewController")]
    partial class RegistrationViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField LoginRegistrationField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField PasswordRegistrationField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton RegistrationButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField VerificationPasswordField { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (LoginRegistrationField != null) {
                LoginRegistrationField.Dispose ();
                LoginRegistrationField = null;
            }

            if (PasswordRegistrationField != null) {
                PasswordRegistrationField.Dispose ();
                PasswordRegistrationField = null;
            }

            if (RegistrationButton != null) {
                RegistrationButton.Dispose ();
                RegistrationButton = null;
            }

            if (VerificationPasswordField != null) {
                VerificationPasswordField.Dispose ();
                VerificationPasswordField = null;
            }
        }
    }
}
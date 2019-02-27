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
    [Register ("ProfileView")]
    partial class ProfileView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField NewPasswordField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField PasswordConfirmField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField PasswordField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ProfileImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SaveImageButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SaveNewPasswordButton { get; set; }

        [Action ("SaveImagePress:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void SaveImagePress (UIKit.UIButton sender);

        [Action ("SavePasswordPress:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void SavePasswordPress (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (NewPasswordField != null) {
                NewPasswordField.Dispose ();
                NewPasswordField = null;
            }

            if (PasswordConfirmField != null) {
                PasswordConfirmField.Dispose ();
                PasswordConfirmField = null;
            }

            if (PasswordField != null) {
                PasswordField.Dispose ();
                PasswordField = null;
            }

            if (ProfileImage != null) {
                ProfileImage.Dispose ();
                ProfileImage = null;
            }

            if (SaveImageButton != null) {
                SaveImageButton.Dispose ();
                SaveImageButton = null;
            }

            if (SaveNewPasswordButton != null) {
                SaveNewPasswordButton.Dispose ();
                SaveNewPasswordButton = null;
            }
        }
    }
}
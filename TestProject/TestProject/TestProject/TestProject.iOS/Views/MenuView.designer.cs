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

namespace TestProject.iOS.Views
{
    [Register ("MenuView")]
    partial class MenuView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView BurgerMenu { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView NavigateList { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ProfileView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ShadowView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView UserProfileImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel UserProfileName { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BurgerMenu != null) {
                BurgerMenu.Dispose ();
                BurgerMenu = null;
            }

            if (NavigateList != null) {
                NavigateList.Dispose ();
                NavigateList = null;
            }

            if (ProfileView != null) {
                ProfileView.Dispose ();
                ProfileView = null;
            }

            if (ShadowView != null) {
                ShadowView.Dispose ();
                ShadowView = null;
            }

            if (UserProfileImage != null) {
                UserProfileImage.Dispose ();
                UserProfileImage = null;
            }

            if (UserProfileName != null) {
                UserProfileName.Dispose ();
                UserProfileName = null;
            }
        }
    }
}
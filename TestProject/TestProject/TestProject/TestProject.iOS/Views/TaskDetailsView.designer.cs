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
    [Register ("TaskDetailsView")]
    partial class TaskDetailsView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton DeleteButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SaveButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView TaskImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField TaskName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField TaskNote { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch TaskStatus { get; set; }

        [Action ("DeletePress:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void DeletePress (UIKit.UIButton sender);

        [Action ("SavePress:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void SavePress (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (DeleteButton != null) {
                DeleteButton.Dispose ();
                DeleteButton = null;
            }

            if (SaveButton != null) {
                SaveButton.Dispose ();
                SaveButton = null;
            }

            if (TaskImage != null) {
                TaskImage.Dispose ();
                TaskImage = null;
            }

            if (TaskName != null) {
                TaskName.Dispose ();
                TaskName = null;
            }

            if (TaskNote != null) {
                TaskNote.Dispose ();
                TaskNote = null;
            }

            if (TaskStatus != null) {
                TaskStatus.Dispose ();
                TaskStatus = null;
            }
        }
    }
}
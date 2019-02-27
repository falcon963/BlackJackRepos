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

namespace TestProject.iOS.Views.Cells
{
    [Register ("ContentCell")]
    partial class ContentCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch CheckBox { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView TaskImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TaskName { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (CheckBox != null) {
                CheckBox.Dispose ();
                CheckBox = null;
            }

            if (TaskImage != null) {
                TaskImage.Dispose ();
                TaskImage = null;
            }

            if (TaskName != null) {
                TaskName.Dispose ();
                TaskName = null;
            }
        }
    }
}
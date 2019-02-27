// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace TestProject.iOS.Views.Cells
{
    [Register ("ContentTasksCell")]
    partial class ContentTasksCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView TaskImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TaskName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch TaskStatus { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (TaskImage != null) {
                TaskImage.Dispose ();
                TaskImage = null;
            }

            if (TaskName != null) {
                TaskName.Dispose ();
                TaskName = null;
            }

            if (TaskStatus != null) {
                TaskStatus.Dispose ();
                TaskStatus = null;
            }
        }
    }
}
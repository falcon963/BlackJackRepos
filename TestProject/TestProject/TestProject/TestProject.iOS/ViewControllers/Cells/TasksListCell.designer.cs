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

namespace TestProject.iOS.ViewControllers.Cells
{
    [Register ("TasksListCell")]
    partial class TasksListCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView TasksListImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch TasksListSwitch { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TasksListTitle { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (TasksListImage != null) {
                TasksListImage.Dispose ();
                TasksListImage = null;
            }

            if (TasksListSwitch != null) {
                TasksListSwitch.Dispose ();
                TasksListSwitch = null;
            }

            if (TasksListTitle != null) {
                TasksListTitle.Dispose ();
                TasksListTitle = null;
            }
        }
    }
}
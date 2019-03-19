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
    [Register ("TasksListView")]
    partial class TasksListView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton FabButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView TasksList { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (FabButton != null) {
                FabButton.Dispose ();
                FabButton = null;
            }

            if (TasksList != null) {
                TasksList.Dispose ();
                TasksList = null;
            }
        }
    }
}
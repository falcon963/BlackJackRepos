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
        UIKit.UIButton AddFileButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem BackButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ButtonView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton DeleteButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView FileList { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint FileViewHeight { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView MainScrollView { get; set; }

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
        UIKit.UITextView TaskNote { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch TaskStatus { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem TaskTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIToolbar TaskToolbar { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AddFileButton != null) {
                AddFileButton.Dispose ();
                AddFileButton = null;
            }

            if (BackButton != null) {
                BackButton.Dispose ();
                BackButton = null;
            }

            if (ButtonView != null) {
                ButtonView.Dispose ();
                ButtonView = null;
            }

            if (DeleteButton != null) {
                DeleteButton.Dispose ();
                DeleteButton = null;
            }

            if (FileList != null) {
                FileList.Dispose ();
                FileList = null;
            }

            if (FileViewHeight != null) {
                FileViewHeight.Dispose ();
                FileViewHeight = null;
            }

            if (MainScrollView != null) {
                MainScrollView.Dispose ();
                MainScrollView = null;
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

            if (TaskTitle != null) {
                TaskTitle.Dispose ();
                TaskTitle = null;
            }

            if (TaskToolbar != null) {
                TaskToolbar.Dispose ();
                TaskToolbar = null;
            }
        }
    }
}
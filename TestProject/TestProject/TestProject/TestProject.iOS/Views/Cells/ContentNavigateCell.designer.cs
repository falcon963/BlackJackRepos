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
    [Register ("ContentNavigateCell")]
    partial class ContentNavigateCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel NavigatePageName { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (NavigatePageName != null) {
                NavigatePageName.Dispose ();
                NavigatePageName = null;
            }
        }
    }
}
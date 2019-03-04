// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace TestProject.iOS
{
    [Register ("AppDelegate")]
    partial class AppDelegate
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MapKit.MKMapView MainMapView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (MainMapView != null) {
                MainMapView.Dispose ();
                MainMapView = null;
            }
        }
    }
}
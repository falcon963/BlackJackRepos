using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using TestProject.Core.ViewModels;
using UIKit;

namespace TestProject.iOS.Fragments
{
    [MvxChildPresentation]
    public class MapFragment: MvxViewController<UserLocationViewModel>
    {
        public MapFragment()
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }
    }
}
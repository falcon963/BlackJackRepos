using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using TestProject.Core.ViewModels;

namespace TestProject.iOS.Fragments
{
    [MvxTabPresentation(WrapInNavigationController = true,
        TabName = "Menu",
        TabSelectedIconName = "ic_menu")]
    public class MenuFragment
        :MvxViewController<MenuViewModel> 
    {
        public MenuFragment()
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }
    }
}
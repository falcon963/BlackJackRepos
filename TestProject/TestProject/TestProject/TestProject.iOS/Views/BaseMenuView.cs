using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.ViewModels;
using UIKit;

namespace TestProject.iOS.Views
{
    public abstract class BaseMenuView<TParam>
        :MvxViewController<TParam> where TParam : MvxViewModel
    {
        protected SidebarNavigation.SidebarController SidebarController
        {
            get
            {
                return (UIApplication.SharedApplication.Delegate as AppDelegate).MenuRootView.SidebarController;
            }
        }

        public BaseMenuView(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
        }


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }
    }
}
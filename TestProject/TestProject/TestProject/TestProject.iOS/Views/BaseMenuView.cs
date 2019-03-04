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

        protected UIWindow Window
        {
            get
            {
                return (UIApplication.SharedApplication.Delegate as AppDelegate).Window;
            }
            set
            {
                (UIApplication.SharedApplication.Delegate as AppDelegate).Window = value;
            }
        }

        public BaseMenuView(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
        }


        //public override void ViewDidLoad()
        //{
        //    base.ViewDidLoad();
        //}
    }
}
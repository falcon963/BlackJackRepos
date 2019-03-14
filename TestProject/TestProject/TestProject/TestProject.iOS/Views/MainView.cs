using System;
using System.Drawing;

using CoreFoundation;
using UIKit;
using Foundation;
using MvvmCross.Platforms.Ios.Views;
using TestProject.Core.ViewModels;
using MvvmCross.Platforms.Ios.Presenters.Attributes;

namespace TestProject.iOS.Views
{
    //[MvxRootPresentation]
    public class MainView 
        : MvxTabBarViewController<MainViewModel>
    {
        public MainView()
        {
        }

        private bool _firstTimePresented = true;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            NavigationController.NavigationBarHidden = true;

            if (_firstTimePresented)
            {
                _firstTimePresented = false;
                ViewModel.ShowMenuCommand.Execute();
                ViewModel.CloseMain.Execute();
            }
        }
    }
}
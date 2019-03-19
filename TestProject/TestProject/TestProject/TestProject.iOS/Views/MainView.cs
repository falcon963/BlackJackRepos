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
    [MvxRootPresentation]
    public class MainView 
        : MvxTabBarViewController<MainViewModel>
    {

        private bool _firstTimePresented = true;
        private bool _constructed;

        public MainView()
        {
            _constructed = true;

            ViewDidLoad();
        }


        public override void ViewDidLoad()
        {
            if (!_constructed)
                return;

            base.ViewDidLoad();

            var vm = (MainViewModel)this.ViewModel;
            if (vm == null)
                return;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (_firstTimePresented)
            {
                _firstTimePresented = false;
                ViewModel.ShowMenuCommand.Execute();
            }
        }
    }
}
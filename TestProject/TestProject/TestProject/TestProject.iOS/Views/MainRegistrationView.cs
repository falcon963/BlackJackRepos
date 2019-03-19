using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using TestProject.Core.ViewModels;
using UIKit;

namespace TestProject.iOS.Views
{
    //[MvxRootPresentation]
    public class MainRegistrationView
        : MvxViewController<MainRegistrationViewModel>
    {
        private bool _firstTimePresented = true;

        public MainRegistrationView()
        {

        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (_firstTimePresented)
            {
                _firstTimePresented = false;
                ViewModel.ShowLoginPageCommand.Execute();
            }
        }
    }
}
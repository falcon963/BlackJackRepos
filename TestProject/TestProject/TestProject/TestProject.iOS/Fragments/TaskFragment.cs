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
    [MvxChildPresentation]
    public class TaskFragment
        :MvxViewController<TaskViewModel>
    {
        public TaskFragment()
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }
    }
}
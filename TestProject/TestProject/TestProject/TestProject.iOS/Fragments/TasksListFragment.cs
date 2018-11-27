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
        TabName = "TasksList",
        TabIconName = "ic_taskslist")]
    public class TasksListFragment
        :MvxViewController<TaskListViewModel>
    {
        public TasksListFragment()
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }
    }
}
using Foundation;
using MvvmCross.Platforms.Ios.Views;
using SidebarNavigation;
using System;

namespace TestProject.iOS.Views
{
        public partial class MenuRootView
                : MvxViewController
        {
            public SidebarController SidebarController { get; private set; }

            public MenuRootView() : base(null, null)
            {

            }

            public override void ViewDidLoad()
            {
                base.ViewDidLoad();

                // create a slideout navigation controller with the top navigation controller and the menu view controller
                SidebarController = new SidebarController(this, new TasksListView(), new MenuView());
                SidebarController.MenuLocation = MenuLocations.Left;
            }
        }
}
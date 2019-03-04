using CoreAnimation;
using Foundation;
using MonoTouch.SlideoutNavigation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using TestProject.Core.ViewModels;
using TestProject.iOS.Source;
using TestProject.iOS.Views.Cells;
using UIKit;

namespace TestProject.iOS.Views
{
    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class TasksListView 
        : BaseMenuView<TaskListViewModel>
    {

        public static NSString MyCellId = new NSString("ContentTasksCell");

        private Boolean _isMenuOpen = false;

        private CATransition _transition  = new CATransition();

        #region Property

        public MvxUIRefreshControl RefreshControl { get; private set; }

        #endregion

        #region ctor


        public TasksListView() : base("TasksListView", null)
        {
            //TasksList.RegisterClassForCellReuse(typeof(ContentTasksCell), MyCellId);
        }

        #endregion

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

           // _menuView.View.Frame = UIScreen.MainScreen.Bounds;


           //// _menuView.View.Bounds = new CoreGraphics.CGRect(_menuView.View.Frame.Width/2, )

           //// this.PresentViewControllerAsync(_menuView, true);
           // _isMenuOpen = true;
           // _menuView.View.Bounds = MenuView.Bounds;
           //this.AddChildViewController(_menuView);
           //this.TasksList.AddSubview(_menuView.View);
           // _menuView.DidMoveToParentViewController(this);

            NavigationController.NavigationBarHidden = true;

            //ContentScrollView.AddSubview(MenuView);

            var source = new TasksListSource(TasksList, this);

            var refreshControl = new MvxUIRefreshControl();
            this.RefreshControl = refreshControl;

            var set = this.CreateBindingSet<TasksListView, TaskListViewModel>();
            set.Bind(refreshControl).For(r => r.IsRefreshing).To(vm => vm.IsRefreshing);
            set.Bind(refreshControl).For(r => r.RefreshCommand).To(vm => vm.RefreshTaskCommand);
            set.Apply();

            this.AddBindings(new Dictionary<object, String>
            {
                { source, "ItemsSource ListOfTasks" }
            });

            TasksList.Source = source;
            TasksList.ReloadData();
        }

        partial void PressMenu(UIBarButtonItem sender)
        {
            //ViewModel.ShowMenuCommand.Execute();

            //        if (_isMenuOpen)
            //        {

            //            _isMenuOpen = false;
            //            _menuView.WillMoveToParentViewController(null);
            //            _menuView.View.RemoveFromSuperview();
            //            _menuView.RemoveFromParentViewController();

            //}
            //        else
            //        {
            //            _isMenuOpen = true;
            //            this.AddChildViewController(_menuView);
            //            this.View.AddSubview(_menuView.View);
            //            _menuView.DidMoveToParentViewController(this);
            //         }

            //if (_isMenuOpen)
            //{
            //    UIView.Animate(
            //       duration: 0.3,
            //       delay: 0,
            //       options: UIViewAnimationOptions.CurveEaseIn,
            //       animation: () =>
            //       {
            //           MenuView.Center = new CoreGraphics.CGPoint(-MenuView.Bounds.Width / 2, MenuView.Center.Y);
            //       },
            //       completion: () =>
            //       {
            //           MenuView.Center = MenuView.Center;
            //       }
            //       );
            //    _isMenuOpen = false;
            //    return;
            //}
            //if (!_isMenuOpen)
            //{

            //    UIView.Animate(
            //        duration: 0.3,
            //        delay: 0,
            //        options: UIViewAnimationOptions.CurveEaseIn,
            //        animation: () =>
            //        {
            //            MenuView.Center = new CoreGraphics.CGPoint(MenuView.Bounds.Width / 2, MenuView.Center.Y);
            //        },
            //        completion: () =>
            //        {
            //            MenuView.Center = MenuView.Center;
            //        }
            //        );
            //    _isMenuOpen = true;
            //    return;
            //}
            //NavigationController.PushViewController(new MenuView(), true);
            ViewModel.ShowMenuCommand.Execute();
        }

    }
}
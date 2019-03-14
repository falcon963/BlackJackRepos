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

        }

        #endregion

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationController.NavigationBarHidden = true;

            var source = new TasksListSource(TasksList, this);
            var refreshControl = new MvxUIRefreshControl();
            this.RefreshControl = refreshControl;

            var set = this.CreateBindingSet<TasksListView, TaskListViewModel>();
            set.Bind(refreshControl).For(r => r.IsRefreshing).To(vm => vm.IsRefreshing);
            set.Bind(refreshControl).For(r => r.RefreshCommand).To(vm => vm.RefreshTaskCommand);
            set.Bind(MenuButton).To(vm => vm.ShowMenuCommand);
            set.Bind(source).For(x => x.ItemsSource).To(vm => vm.ListOfTasks);
            set.Bind(source).For(x => x.SelectionChangedCommand).To(vm => vm.ItemSelectedCommand);            
            //set.Bind(View).For(v => v.BackgroundColor).To(vm => vm.TasksListColor).WithConversion("NativeColor");
            set.Apply();

            //this.AddBindings(new Dictionary<object, String>
            //{
            //    { source, "ItemsSource ListOfTasks" }
            //});

            TasksList.Source = source;
            TasksList.RegisterNibForCellReuse(UINib.FromName("ContentTasksCell", NSBundle.MainBundle), ContentTasksCell.Key);
            TasksList.RowHeight = UITableView.AutomaticDimension;
            //TasksList.ScrollEnabled = false;
            //if (TasksList.ContentSize.Height > TasksList.VisibleSize.Height)
            //{
            //    TasksList.ScrollEnabled = true;
            //}
            TasksList.ReloadData();

            var myTabBar = new MainView();
            Window.RootViewController = myTabBar;
        }
    }
}
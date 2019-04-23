using CoreAnimation;
using CoreGraphics;
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
using TestProject.iOS.Converters;
using TestProject.iOS.Sources;
using TestProject.iOS.Views.Cells;
using UIKit;
using Xam.iOS.Fab;
using Xam.iOS.Fab.Views;

namespace TestProject.iOS.Views
{
    [MvxTabPresentation(WrapInNavigationController = true, TabName = "Tasks", TabIconName = "icons8_task_24")]
    public partial class TasksListView 
        : BaseMenuView<TaskListViewModel>
    {


        public static NSString MyCellId = new NSString("ContentTasksCell");

        private bool _isMenuOpen = false;

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

            NavigationController.NavigationBar.TopItem.Title = "Tasks List";
            

            var source = new TasksListSource(TasksList, this);
            var refreshControl = new MvxUIRefreshControl();
            this.RefreshControl = refreshControl;

            var set = this.CreateBindingSet<TasksListView, TaskListViewModel>();
            set.Bind(refreshControl).For(r => r.IsRefreshing).To(vm => vm.IsRefreshing);
            set.Bind(refreshControl).For(r => r.RefreshCommand).To(vm => vm.RefreshTaskCommand);
            set.Bind(source).For(x => x.ItemsSource).To(vm => vm.ListOfTasks);
            set.Bind(source).For(x => x.SelectionChangedCommand).To(vm => vm.ItemSelectedCommand);            
            set.Bind(TasksList).For(v => v.BackgroundColor).To(vm => vm.TasksListColor).WithConversion(new ColorValueConverter());
            set.Bind(FabButton).To(vm => vm.ShowTaskCommand);
            set.Apply();

            TasksList.AddSubview(refreshControl);
            TasksList.Source = source;
            TasksList.RegisterNibForCellReuse(UINib.FromName("ContentTasksCell", NSBundle.MainBundle), ContentTasksCell.Key);
            TasksList.RowHeight = 60;
            TasksList.ReloadData();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            CreateFloatingButton();
        }

        public void CreateFloatingButton()
        {
            FabButton.BackgroundColor = UIColor.White;
            FabButton.Layer.CornerRadius = FabButton.Frame.Height / 2;
            this.FabButton.Layer.ShadowColor = UIColor.Black.CGColor;
            this.FabButton.Layer.ShadowOpacity = 1/4;
            this.FabButton.Layer.ShadowOffset = new CGSize(0, 10);
            this.FabButton.Layer.MasksToBounds = false;
            this.FabButton.Layer.ShadowRadius = 5;
            this.FabButton.Layer.ShadowOpacity = 1 / 2;
        }
    }
}
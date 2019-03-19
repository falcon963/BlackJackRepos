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
using TestProject.iOS.Source;
using TestProject.iOS.Views.Cells;
using UIKit;
using Xam.iOS.Fab;
using Xam.iOS.Fab.Views;

namespace TestProject.iOS.Views
{
    [MvxTabPresentation(WrapInNavigationController = true, TabName = "Tasks")]
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

            //NavigationController.NavigationBarHidden = true;
            NavigationController.NavigationBar.TopItem.Title = "Tasks List";

            UIViewController button = new UIViewController();
            button.ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
            this.PresentViewController(button, true, ()=> { ViewModel.ShowTaskCommand.Execute(); });
            

            var source = new TasksListSource(TasksList, this);
            var refreshControl = new MvxUIRefreshControl();
            this.RefreshControl = refreshControl;

            var set = this.CreateBindingSet<TasksListView, TaskListViewModel>();
            set.Bind(refreshControl).For(r => r.IsRefreshing).To(vm => vm.IsRefreshing);
            set.Bind(refreshControl).For(r => r.RefreshCommand).To(vm => vm.RefreshTaskCommand);
            set.Bind(source).For(x => x.ItemsSource).To(vm => vm.ListOfTasks);
            set.Bind(source).For(x => x.SelectionChangedCommand).To(vm => vm.ItemSelectedCommand);            
            set.Bind(TasksList).For(v => v.BackgroundColor).To(vm => vm.TasksListColor).WithConversion(new ColorValueConverter());
            set.Apply();

            TasksList.Source = source;
            TasksList.RegisterNibForCellReuse(UINib.FromName("ContentTasksCell", NSBundle.MainBundle), ContentTasksCell.Key);
            TasksList.RowHeight = UITableView.AutomaticDimension;
            TasksList.ReloadData();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            CreateFloatingButton();
        }

        public void CreateFloatingButton()
        {
            FabButton.BackgroundColor = UIColor.Red;
            var keyWindow = UIApplication.SharedApplication.KeyWindow;
            keyWindow.AddSubview(this.FabButton);
            FabButton.Layer.CornerRadius = FabButton.Frame.Height / 2;
            this.FabButton.Layer.ShadowColor = UIColor.Black.CGColor;
            this.FabButton.Layer.ShadowOffset = new CGSize(0, 5);
            this.FabButton.Layer.MasksToBounds = false;
            this.FabButton.Layer.ShadowRadius = 2;
            this.FabButton.Layer.ShadowOpacity = 1/2;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            this.FabButton.RemoveFromSuperview();
            FabButton = null;
        }
    }
}
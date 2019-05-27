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
using TestProject.LanguageResources;
using UIKit;
using Xam.iOS.Fab;
using Xam.iOS.Fab.Views;

namespace TestProject.iOS.Views
{
    [MvxTabPresentation(WrapInNavigationController = true, TabName = "Tasks", TabIconName = "task")]
    public partial class TasksListView 
        : BaseView<TasksListView, TasksListViewModel>
    {
        private const float ShadowOpacity = 0.5f;
        private const float ShadowRadius = 5f;
        private const float TasksListRowHeight = 60f;
        private const float ShadowOffsetWidth = 0f;
        private const float ShadowOffsetHeight = 10f;

        public static NSString MyCellId = new NSString(nameof(ContentNavigateCell));

        private CATransition _transition  = new CATransition();

        private TasksListSource _source;

        private MvxUIRefreshControl _refreshControl;

        private bool _constructed;


        #region Property

        public MvxUIRefreshControl RefreshControl { get; private set; }

        #endregion

        #region ctor


        public TasksListView() : base(nameof(TasksListView), null)
        {

        }

        #endregion

        public override bool SetupBindings()
        {
            BindingSet.Bind(_refreshControl).For(r => r.IsRefreshing).To(vm => vm.IsRefreshing);
            BindingSet.Bind(_refreshControl).For(r => r.RefreshCommand).To(vm => vm.RefreshTaskCommand);
            BindingSet.Bind(_source).For(x => x.ItemsSource).To(vm => vm.Tasks);
            BindingSet.Bind(_source).For(x => x.SelectionChangedCommand).To(vm => vm.ItemSelectedCommand);
            BindingSet.Bind(TasksList).For(v => v.BackgroundColor).To(vm => vm.TasksListColor).WithConversion("NativeColor");
            BindingSet.Bind(FabButton).To(vm => vm.CreateTaskCommand);

            return base.SetupBindings();
        }

        public override bool CustomizeViews()
        {
            TasksList.RowHeight = TasksListRowHeight;

            return base.CustomizeViews();
        }

        public override void ViewDidLoad()
        {
            //NavigationController.NavigationBar.TopItem.Title = Strings.TasksList;           
            //_constructed = true;

            _source = new TasksListSource(TasksList, this);
            _refreshControl = new MvxUIRefreshControl();
            RefreshControl = _refreshControl;

            base.ViewDidLoad();

            TasksList.AddSubview(_refreshControl);
            TasksList.Source = _source;
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
            FabButton.Layer.ShadowColor = UIColor.Black.CGColor;
            FabButton.Layer.ShadowOffset = new CGSize(ShadowOffsetWidth, ShadowOffsetHeight);
            FabButton.Layer.MasksToBounds = false;
            FabButton.Layer.ShadowRadius = ShadowRadius;
            FabButton.Layer.ShadowOpacity = ShadowOpacity;
        }
    }
}
using CoreAnimation;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platform.UI;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using TestProject.Core.ViewModels;
using TestProject.iOS.Converters;
using TestProject.iOS.Sources;
using TestProject.iOS.Views.Cells;
using UIKit;
//using MvvmCross.iOS.Support.XamarinSidebar;

namespace TestProject.iOS.Views
{
    [MvxTabPresentation(WrapInNavigationController = true, TabName = "Menu", TabIconName = "icons8_menu_26")]
    public partial class MenuView 
        : BaseMenuView<MenuViewModel>
    {

        public static NSString MyCellId = new NSString("ContentNavigateCell");

        public MenuView() : base("MenuView", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var source = new MenuItemSource(NavigateList);


            NavigationController.NavigationBar.TopItem.Title = "Menu";


            var set = this.CreateBindingSet<MenuView, MenuViewModel>();
            set.Bind(NavigateList).For(v => v.BackgroundColor).To(vm => vm.MenuColor).WithConversion(new ColorValueConverter());
            set.Bind(UserProfileImage).For(v => v.Image).To(vm => vm.Profile.ImagePath).WithConversion(new ImageValueConverter());
            set.Bind(source).For(x => x.ItemsSource).To(vm => vm.MenuItems);
            set.Bind(source).For(x => x.SelectionChangedCommand).To(vm => vm.ItemSelectCommand);
            set.Apply();

            NavigateList.Source = source;
;
            this.AutomaticallyAdjustsScrollViewInsets = false;
            NavigateList.ScrollEnabled = false;

            var transition = new CATransition();
            transition.Duration = 0.5;
            transition.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseInEaseOut);
            transition.Type = CATransition.TransitionPush;
            transition.Subtype = CATransition.TransitionFromLeft;

            this.NavigationController.View.Layer.AddAnimation(transition, null);

            UserProfileImage.Layer.BorderWidth = 3;
            UserProfileImage.Layer.BorderColor = UIColor.White.CGColor;
            UserProfileImage.Layer.MasksToBounds = true;
            UserProfileName.Text = ViewModel.Profile.Login;

            NavigateList.ReloadData();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            UserProfileImage.Layer.CornerRadius = UserProfileImage.Frame.Size.Height / 2;
        }

    }
}
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
using TestProject.LanguageResources;
using UIKit;

namespace TestProject.iOS.Views
{
    [MvxTabPresentation(WrapInNavigationController = true, TabName = "Menu", TabIconName = "menu")]
    public partial class MenuView 
        : BaseView<MenuView ,MenuViewModel>
    {
        private MenuItemSource _source;
        public static NSString MyCellId = new NSString("ContentNavigateCell");

        public MenuView() : base("MenuView", null)
        {
        }

        public override bool SetupBindings()
        {
            BindingSet.Bind(NavigateList).For(v => v.BackgroundColor).To(vm => vm.MenuColor).WithConversion("NativeColor");
            BindingSet.Bind(UserProfileImage).For(v => v.Image).To(vm => vm.Profile.ImagePath).WithConversion("NativeColor");
            BindingSet.Bind(_source).For(x => x.ItemsSource).To(vm => vm.MenuItems);
            BindingSet.Bind(_source).For(x => x.SelectionChangedCommand).To(vm => vm.ItemSelectCommand);

            return base.SetupBindings();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _source = new MenuItemSource(NavigateList);

            NavigationController.NavigationBar.TopItem.Title = Strings.Menu;

            NavigateList.Source = _source;
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
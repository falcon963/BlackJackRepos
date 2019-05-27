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
        private const float AnimationDuration = 0.5f;
        private const float UserProfileImageBorderWidth = 3f;
        private MenuItemSource _source;
        public static NSString MyCellId = new NSString(nameof(ContentNavigateCell));

        public MenuView() : base(nameof(MenuView), null)
        {
            base.ViewDidLoad();
        }

        public override bool SetupBindings()
        {
            BindingSet.Bind(NavigateList).For(v => v.BackgroundColor).To(vm => vm.MenuColor).WithConversion("NativeColor");
            BindingSet.Bind(UserProfileImage).For(v => v.Image).To(vm => vm.ProfileImage).WithConversion(new ImageValueConverter());
            BindingSet.Bind(_source).For(x => x.ItemsSource).To(vm => vm.MenuItems);
            BindingSet.Bind(_source).For(x => x.SelectionChangedCommand).To(vm => vm.ItemSelectCommand);
            BindingSet.Bind(UserProfileName).For(v => v.Text).To(vm => vm.UserName);

            return base.SetupBindings();
        }

        public override void ViewDidLoad()
        {

            _source = new MenuItemSource(NavigateList);

            NavigationController.NavigationBar.TopItem.Title = Strings.Menu;

            NavigateList.Source = _source;
;
            AutomaticallyAdjustsScrollViewInsets = false;
            NavigateList.ScrollEnabled = false;

            var transition = new CATransition();
            transition.Duration = AnimationDuration;
            transition.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseInEaseOut);
            transition.Type = CATransition.TransitionPush;
            transition.Subtype = CATransition.TransitionFromLeft;

            NavigationController.View.Layer.AddAnimation(transition, null);

            UserProfileImage.Layer.BorderWidth = UserProfileImageBorderWidth;
            UserProfileImage.Layer.BorderColor = UIColor.White.CGColor;
            UserProfileImage.Layer.MasksToBounds = true;

            NavigateList.ReloadData();


        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            UserProfileImage.Layer.CornerRadius = UserProfileImage.Frame.Size.Height / 2;
        }

    }
}
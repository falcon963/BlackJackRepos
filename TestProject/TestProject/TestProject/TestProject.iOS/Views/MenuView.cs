using CoreAnimation;
using Foundation;
using MvvmCross.Binding.BindingContext;
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
using TestProject.iOS.Source;
using TestProject.iOS.Views.Cells;
using UIKit;
//using MvvmCross.iOS.Support.XamarinSidebar;

namespace TestProject.iOS.Views
{
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

            var source = new MenuItemSource(this, NavigateList);

            this.AddBindings(new Dictionary<object, String>
            {
                { source, "ItemsSource MenuItems" }
            });

            NavigateList.Source = source;
            NavigateList.ReloadData();

            var set = this.CreateBindingSet<MenuView, MenuViewModel>();
            set.Bind(NavigateList).For(v => v.BackgroundColor).To(vm => vm.MenuColor).WithConversion("NativeColor");
            //set.Bind(ProfileView).To(vm => vm.OpenProfileCommand);
            set.Apply();

           

            UISwipeGestureRecognizer recognizer = new UISwipeGestureRecognizer(CloseMenu);
            UITapGestureRecognizer tupRecognizer = new UITapGestureRecognizer(CloseMenu);
            recognizer.Direction = UISwipeGestureRecognizerDirection.Right;
            View.AddGestureRecognizer(recognizer);
            ShadowView.AddGestureRecognizer(tupRecognizer);
            this.AutomaticallyAdjustsScrollViewInsets = false;
            NavigateList.ScrollEnabled = false;

            var transition = new CATransition();
            transition.Duration = 0.5;
            transition.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseInEaseOut);
            transition.Type = CATransition.TransitionPush;
            transition.Subtype = CATransition.TransitionFromLeft;

            this.NavigationController.View.Layer.AddAnimation(transition, null);

            // profile load
            String profileImage = ViewModel.Profile.ImagePath;
            if (profileImage != null)
            {
                UserProfileImage.Image = UIImage.LoadFromData(ViewModel.Profile.ImagePath);
            }
            if (profileImage == null)
            {
                UserProfileImage.Image = UIImage.FromFile("placeholder.png");
            }
            UserProfileImage.Layer.BorderWidth = 3;
            UserProfileImage.Layer.BorderColor = UIColor.White.CGColor;
            UserProfileImage.Layer.MasksToBounds = true;
            UserProfileName.Text = ViewModel.Profile.Login;   
        }

        private void CloseMenu()
        {
            ViewModel.CloseMenu.Execute();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            UserProfileImage.Layer.CornerRadius = UserProfileImage.Frame.Size.Height / 2;
        }

    }
}
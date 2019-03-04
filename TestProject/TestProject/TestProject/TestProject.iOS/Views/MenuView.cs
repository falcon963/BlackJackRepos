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
using TestProject.Core.ViewModels;
using TestProject.iOS.Source;
using TestProject.iOS.Views.Cells;
using UIKit;
//using MvvmCross.iOS.Support.XamarinSidebar;

namespace TestProject.iOS.Views
{
   // [MvxSidebarPresentation(MvxPanelEnum.Left, MvxPanelHintType.ResetRoot, false)]
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

            NavigationController.NavigationBarHidden = true;

            var source = new MenuItemSource(this, NavigateList);

            this.AddBindings(new Dictionary<object, String>
            {
                { source, "ItemsSource MenuItems" }
            });

            NavigateList.Source = source;
            //NavigateList.RowHeight = ContentNavigateCell.GetCellHeight();
            NavigateList.ReloadData();

            UISwipeGestureRecognizer recognizer = new UISwipeGestureRecognizer(CloseMenu);
            UITapGestureRecognizer tupRecognizer = new UITapGestureRecognizer(CloseMenu);
            recognizer.Direction = UISwipeGestureRecognizerDirection.Right;
            View.AddGestureRecognizer(recognizer);
            ShadowView.AddGestureRecognizer(tupRecognizer);

            // profile load
            //var profileImage = ViewModel.Profile.ImagePath;
            //if (profileImage != null)
            //{
            //    UserProfileImage.Image = UIImage.LoadFromData(ViewModel.Profile.ImagePath);
            //}
            //if (profileImage == null)
            //{
                UserProfileImage.Image = UIImage.FromFile("placeholder.png");
            //}
            //UserProfileName.Text = ViewModel.Profile.Login;
            //

            ///
            // MenuRightConstraint.Constant = -NavigationDrawer.Bounds.Width;
            ///   

        }

        private void CloseMenu()
        {
            ViewModel.CloseMenu.Execute();
        }
    }
}
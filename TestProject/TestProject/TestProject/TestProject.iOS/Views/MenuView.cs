using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using TestProject.Core.ViewModels;
using TestProject.iOS.Source;
using TestProject.iOS.Views.Cells;
using UIKit;
//using MvvmCross.iOS.Support.XamarinSidebar;

namespace TestProject.iOS.Views
{
    [Register("MenuView")]
   // [MvxSidebarPresentation(MvxPanelEnum.Left, MvxPanelHintType.ResetRoot, false)]
    public partial class MenuView : BaseMenuView<MenuViewModel>
    {

        public static NSString MyCellId = new NSString("ContentNavigateCell");

        private bool _constructed;

        public MenuView() : base("MenuView", null)
        {
            _constructed = true;
            //NavigateList.RegisterClassForCellReuse(typeof(NavigateCell), MyCellId);
            ViewDidLoad();
        }

        public override void ViewDidLoad()
        {
            if (!_constructed)
            {
                return;
            }

            base.ViewDidLoad();

            //NavigationController.NavigationBarHidden = true;

            //NavigateList.RegisterClassForCellReuse(typeof(ContentNavigateCell), MyCellId);

            //var source = new MenuItemSource(NavigateList);

            //this.AddBindings(new Dictionary<object, String>()
            //{
            //    { source, "ItemsSource MenuItems" }
            //});

            //NavigateList.Source = source;
            //NavigateList.ReloadData();

            ///profile load
            //var profileImage = ViewModel.Profile.ImagePath;
            //if (profileImage != null)
            //{
            //    UserProfileImage.Image = UIImage.LoadFromData(ViewModel.Profile.ImagePath);
            //}
            //if (profileImage == null)
            //{
            //    UserProfileImage.Image = UIImage.FromFile("placeholder.png");
            //}
            //UserProfileName.Text = ViewModel.Profile.Login;
            ///

            ///
            // MenuRightConstraint.Constant = -NavigationDrawer.Bounds.Width;
            ///

        }
    }
}
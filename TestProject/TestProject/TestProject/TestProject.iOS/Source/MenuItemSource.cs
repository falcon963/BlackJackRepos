using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using MvvmCross.Platforms.Ios.Binding.Views;
using TestProject.Core.Models;
using TestProject.iOS.Views;
using TestProject.iOS.Views.Cells;
using UIKit;

namespace TestProject.iOS.Source
{
    public class MenuItemSource : MvxTableViewSource
    {
        private static readonly NSString cellIdentifier = new NSString("ContentNavigateCell");
        private MenuView _view;

        public MenuItemSource(MenuView view, UITableView tableView) : base(tableView)
        {
            _view = view;
            //DeselectAutomatically = true;
            tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            tableView.RegisterNibForCellReuse(UINib.FromName("ContentNavigateCell", NSBundle.MainBundle), cellIdentifier);
        }

        public MenuItemSource(IntPtr handle) : base(handle)
        {
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            var cell = new UITableViewCell();
            if (tableView.DequeueReusableCell(cellIdentifier) is ContentNavigateCell navigateCell)
            {
                cell = navigateCell;
                if (item is MenuItem menuItem)
                {
                    (cell as ContentNavigateCell).UpdateCell(menuItem);
                }
            }
            cell.Layer.BorderColor = UIColor.Black.CGColor;
            cell.Layer.BorderWidth = 1;
            cell.Layer.CornerRadius = 8;
            cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            return cell;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return 3;
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            var headerView = new UIView();
            headerView.BackgroundColor = UIColor.Clear;
            return headerView;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return 60;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            _view.ViewModel.ItemSelectCommand.Execute(_view.ViewModel.MenuItems[indexPath.Row]);
        }
    }
}
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

namespace TestProject.iOS.Sources
{
    public class MenuItemSource 
        : MvxTableViewSource
    {
        public string cellIdentifier = "ContentNavigateCell";

        public MenuItemSource(UITableView tableView) : base(tableView)
        {
            DeselectAutomatically = true;
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
            }
            cell.Layer.BorderColor = UIColor.White.CGColor;
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

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return 30;
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            return 0;
        }
    }
}
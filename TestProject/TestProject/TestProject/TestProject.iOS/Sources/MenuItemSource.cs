using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using MvvmCross.Platforms.Ios.Binding.Views;
using TestProject.Core.Models;
using TestProject.iOS.Constants;
using TestProject.iOS.Views;
using TestProject.iOS.Views.Cells;
using UIKit;

namespace TestProject.iOS.Sources
{
    public class MenuItemSource 
        : MvxTableViewSource
    {
        public string cellIdentifier = nameof(ContentNavigateCell);

        public const int rowHeight = 30;

        public const int headerHeight = 0;

        public const int sectionsNumber = 1;

        public MenuItemSource(UITableView tableView) : base(tableView)
        {
            DeselectAutomatically = true;
            tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            tableView.RegisterNibForCellReuse(UINib.FromName(nameof(ContentNavigateCell), NSBundle.MainBundle), cellIdentifier);
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
            cell.Layer.BorderWidth = SizeConstants.BorderWidth;
            cell.Layer.CornerRadius = SizeConstants.CornerRadius;
            cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            return cell;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return sectionsNumber;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return rowHeight;
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            return headerHeight;
        }
    }
}
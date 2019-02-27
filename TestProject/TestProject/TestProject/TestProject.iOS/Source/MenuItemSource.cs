using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using MvvmCross.Platforms.Ios.Binding.Views;
using TestProject.Core.Models;
using TestProject.iOS.Views.Cells;
using UIKit;

namespace TestProject.iOS.Source
{
    public class MenuItemSource : MvxTableViewSource
    {
        public String cellIdentifier = "ContentNavigateCell";

        public MenuItemSource(UITableView tableView) : base(tableView)
        {
            DeselectAutomatically = true;
            tableView.RegisterNibForCellReuse(UINib.FromName("ContentNavigateCell", NSBundle.MainBundle), ContentNavigateCell.Key);
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
            return cell;
        }
    }
}
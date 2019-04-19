using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using MvvmCross.Platforms.Ios.Binding.Views;
using TestProject.iOS.Views;
using TestProject.iOS.Views.Cells;
using UIKit;

namespace TestProject.iOS.Sources
{
    public class TaskFilesListSource
        : MvxTableViewSource
    {

        public string cellIdentifier = "FileCell";

        private TaskDetailsView _mainView;

        public TaskFilesListSource(UITableView tableView, TaskDetailsView view) : base(tableView)
        {
            _mainView = view;
            DeselectAutomatically = true;
            tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            tableView.RegisterNibForCellReuse(UINib.FromName("FileCell", NSBundle.MainBundle), cellIdentifier);
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            var cell = tableView.DequeueReusableCell(FileCell.Key) as FileCell;
            if (cell == null)
            {
                return new MvxTableViewCell();
            }
            
            return cell;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using MvvmCross.Platforms.Ios.Binding.Views;
using TestProject.Core.Models;
using TestProject.Core.ViewModels;
using TestProject.iOS.Views;
using TestProject.iOS.Views.Cells;
using UIKit;

namespace TestProject.iOS.Source
{
    public class TasksListSource: MvxTableViewSource
    {

        public String cellIdentifier = "ContentTasksCell";

        private TasksListView _view;

        //private TasksListViewController _owner;

        public TasksListSource(UITableView tableView, TasksListView view) : base(tableView)
        {
            _view = view;
            DeselectAutomatically = true;
            tableView.RegisterNibForCellReuse(UINib.FromName("ContentTasksCell", NSBundle.MainBundle), cellIdentifier);
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            var cell = new UITableViewCell();
            if (tableView.DequeueReusableCell(cellIdentifier) is ContentTasksCell taskCell)
            {
                cell = taskCell;
                if (item is UserTask taskItem)
                {
                    (cell as ContentTasksCell).UpdateCell(taskItem);
                }
            }
            cell.Layer.BorderColor = UIColor.Black.CGColor;
            cell.Layer.BorderWidth = 1;
            cell.Layer.CornerRadius = 8;
            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var task = _view.ViewModel.ListOfTasks[indexPath.Row];
            _view.ViewModel.ItemSelectedCommand.Execute(task);
        }

        public override UISwipeActionsConfiguration GetLeadingSwipeActionsConfiguration(UITableView tableView, NSIndexPath indexPath)
        {
            var editAction = ContextualEditAction(indexPath.Row);
            var deleteAction = ContextualDeleteAction(indexPath.Row);

            var leadingSwipe = UISwipeActionsConfiguration.FromActions(new UIContextualAction[] { editAction, deleteAction });

            leadingSwipe.PerformsFirstActionWithFullSwipe = false;
            return leadingSwipe;
        }

        public UIContextualAction ContextualDeleteAction(int row)
        {
            var action = UIContextualAction.FromContextualActionStyle(UIContextualActionStyle.Normal, "Delete", (DeleteAction, view, success) =>
            {
                var vm = _view.ViewModel;
                vm.DeleteTaskCommand.Execute(vm.ListOfTasks[row]);
                vm.ListOfTasks.RemoveAt(row);
                success(true);
            });

            action.BackgroundColor = UIColor.Red;

            return action;
        }

        public UIContextualAction ContextualEditAction(int row)
        {
            var action = UIContextualAction.FromContextualActionStyle(UIContextualActionStyle.Normal, "Edit", (EditAction, view, success) =>
            {
                var vm = _view.ViewModel;
                vm.ItemSelectedCommand.Execute(vm.ListOfTasks[row]);
                success(true);
            });

            action.BackgroundColor = UIColor.Green;

            return action;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _view.ViewModel.ListOfTasks.Count;
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
    }
}
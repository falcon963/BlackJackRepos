using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using MvvmCross.Platforms.Ios.Binding.Views;
using TestProject.Core.Models;
using TestProject.Core.ViewModels;
using TestProject.iOS.Constants;
using TestProject.iOS.Views;
using TestProject.iOS.Views.Cells;
using TestProject.LanguageResources;
using UIKit;

namespace TestProject.iOS.Sources
{
    public class TasksListSource
        : MvxTableViewSource
    {

        public string cellIdentifier = nameof(ContentTasksCell);

        private TasksListView _view;

        public const int sectionsNumber = 1;

        public TasksListSource(UITableView tableView, TasksListView view) : base(tableView)
        {
            _view = view;
            DeselectAutomatically = true;
            tableView.RegisterNibForCellReuse(UINib.FromName(nameof(ContentTasksCell), NSBundle.MainBundle), cellIdentifier);
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            var cell = tableView.DequeueReusableCell(cellIdentifier) as ContentTasksCell;
            if (cell == null)
            {
                return new MvxTableViewCell();
            }
            cell.Layer.BorderColor = UIColor.Black.CGColor;
            cell.Layer.BorderWidth = SizeConstants.BorderWidth;
            cell.Layer.CornerRadius = SizeConstants.CornerRadius;

            if (item is UserTask taskItem)
            {
                (cell as ContentTasksCell).UpdateCell(taskItem);
            }

            cell.SelectionStyle = UITableViewCellSelectionStyle.None;

            return cell;
        }

        public override UISwipeActionsConfiguration GetTrailingSwipeActionsConfiguration(UITableView tableView, NSIndexPath indexPath)
        {
            var deleteAction = ContextualDeleteAction(indexPath.Row);

            var trailingSwipe = UISwipeActionsConfiguration.FromActions(new UIContextualAction[] { deleteAction });

            trailingSwipe.PerformsFirstActionWithFullSwipe = false;
            return trailingSwipe;
        }

        public UIContextualAction ContextualDeleteAction(int row)
        {
            var action = UIContextualAction.FromContextualActionStyle(UIContextualActionStyle.Normal, Strings.Delete , (DeleteAction, view, success) =>
            {
                var vm = _view.ViewModel;
                vm.DeleteTaskCommand.Execute(vm.Tasks[row]);
                vm.Tasks.RemoveAt(row);
                success(true);
            });

            action.BackgroundColor = UIColor.Red;

            return action;
        }

        public UIContextualAction ContextualEditAction(int row)
        {
            var action = UIContextualAction.FromContextualActionStyle(UIContextualActionStyle.Normal, Strings.Edit, (EditAction, view, success) =>
            {
                var vm = _view.ViewModel;
                vm.ItemSelectedCommand.Execute(vm.Tasks[row]);
                success(true);
            });

            action.BackgroundColor = UIColor.Green;

            return action;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return sectionsNumber;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _view.ViewModel.Tasks.Count;
        }
    }
}
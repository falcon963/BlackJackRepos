using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V7.RecyclerView;
using TestProject.Core.ViewModels;

namespace TestProject.Droid.Adapter
{
    public class SwipeItemDelete
        : ItemTouchHelper.SimpleCallback
    {
        public SwipeItemDelete(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public SwipeItemDelete() 
            : base(0, ItemTouchHelper.Left
                  | ItemTouchHelper.Right)
        {
        }

        public override bool OnMove(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, RecyclerView.ViewHolder target)
        {
            return false;
        }

        public override void OnSwiped(RecyclerView.ViewHolder viewHolder, int direction)
        {
            var holder = (MvxRecyclerViewHolder)viewHolder;
            var vm = (TaskListViewModel)holder.DataContext;
            vm.DeleteTaskCommand.Execute(this);
        }
    }
}
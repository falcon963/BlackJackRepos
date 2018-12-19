using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Views;
using Android.Widget;
using TestProject.Core.Models;
using TestProject.Core.ViewModels;
using TestProject.Droid.Fragments;
using TestProject.Core.Enum;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using TestProject.Droid.Views;

namespace TestProject.Droid.Adapter
{
    public class MyItemTouchHelper
        : ItemTouchHelper.Callback
    {
        private TasksFragment _view;
        private Boolean swipeBack = false;
        private ButtonState buttonShowedState = ButtonState.Gone;
        private readonly Single buttonWidth = 230;
        private RecyclerView.ViewHolder currentItemViewHolder = null;
        private RectF buttonInstance = null;
        public EventHandler<Int32> RightClick;
        private RectF _rightButton;

        private RecyclerImageAdapter _adapter;
        private Drawable background;
        private Drawable xMark;
        private Int32 xMarkMargin;
        private Boolean _initiated;

        private void Init()
        {
            background = new ColorDrawable(Color.Red);
            xMark = ContextCompat.GetDrawable(_view.Context, Resource.Drawable.icons8_broom_50);
            xMark.SetColorFilter(Color.White, PorterDuff.Mode.DstAtop);
            xMarkMargin = (Int32)_view.Context.Resources.GetDimension(Resource.Dimension.ic_clear_margin);
            _initiated = true;
        }

        public MyItemTouchHelper(TasksFragment view, RecyclerImageAdapter adapter)
        {
            _view = view;
            _adapter = adapter;
        }

        public override Int32 GetMovementFlags(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder)
        {
            Int32 swipeFlags = ItemTouchHelper.Left;
            return MakeMovementFlags(0, swipeFlags);
        }

        public override bool OnMove(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, RecyclerView.ViewHolder target)
        {
            return false;
        }

        public override void OnSwiped(RecyclerView.ViewHolder viewHolder, Int32 direction)
        {
            //UserTask task = _view.ViewModel.ListOfTasks[viewHolder.Position];
            //_view.ViewModel.DeleteTaskCommand.Execute(task);
            //_view.ViewModel.ListOfTasks.Remove(task);
            Int32 position = viewHolder.AdapterPosition;
            _adapter.PendingRemoval(position);
        }

        public override int ConvertToAbsoluteDirection(Int32 flags, Int32 layoutDirection)
        {
            if (swipeBack)
            {
                swipeBack = buttonShowedState != ButtonState.Gone;
                return 0;
            }
            return base.ConvertToAbsoluteDirection(flags, layoutDirection);
        }

        public override void OnChildDraw(Canvas c, RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, Single dX, Single dY, Int32 actionState, Boolean isCurrentlyActive)
        {
            View itemView = viewHolder.ItemView;

            if(viewHolder.AdapterPosition == -1)
            {
                return; 
            }

            if (!_initiated)
            {
                Init();
            }

            background.SetBounds(itemView.Right + (Int32)dX, itemView.Top, itemView.Right, itemView.Bottom);
            background.Draw(c);

            Int32 itemHeight = itemView.Bottom - itemView.Top;
            Int32 intrinsicWidth = xMark.IntrinsicWidth;
            Int32 intrinsicHeight = xMark.IntrinsicWidth;

            Int32 xMarkLeft = itemView.Right - xMarkMargin - intrinsicWidth;
            Int32 xMarkRight = itemView.Right - xMarkMargin;
            Int32 xMarkTop = itemView.Top + (itemHeight - intrinsicHeight) / 2;
            Int32 xMarkBottom = xMarkTop + intrinsicHeight;
            xMark.SetBounds(xMarkLeft, xMarkTop, xMarkRight, xMarkBottom);
            xMark.Draw(c);

            base.OnChildDraw(c, recyclerView, viewHolder, dX, dY, actionState, isCurrentlyActive);
        }
    }
}
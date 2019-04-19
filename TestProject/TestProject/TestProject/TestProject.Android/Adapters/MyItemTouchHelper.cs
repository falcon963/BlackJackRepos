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
using TestProject.Core.Enums;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using TestProject.Droid.Views;
using TestProject.Core.Enums;

namespace TestProject.Droid.Adapters
{
    public class MyItemTouchHelper
        : ItemTouchHelper.Callback
    {
        private TasksFragment _view;
        private Boolean swipeBack = false;
        private ButtonState buttonShowedState = ButtonState.Gone;
        public EventHandler<int> RightClick;

        private RecyclerImageAdapter _adapter;
        private Drawable background;
        private Drawable xMark;
        private Int32 xMarkMargin;
        private Boolean _initiated;

        private void Init()
        {
            background = new ColorDrawable(new Color(251, 192, 45));
            xMark = ContextCompat.GetDrawable(_view.Context, Resource.Drawable.delete);
            xMark.SetColorFilter(Color.White, PorterDuff.Mode.DstAtop);
            xMarkMargin = (int)_view.Context.Resources.GetDimension(Resource.Dimension.ic_clear_margin);
            _initiated = true;
        }

        public MyItemTouchHelper(TasksFragment view, RecyclerImageAdapter adapter)
        {
            _view = view;
            _adapter = adapter;
        }

        public override int GetMovementFlags(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder)
        {
            int swipeFlags = ItemTouchHelper.Left;
            return MakeMovementFlags(0, swipeFlags);
        }

        public override bool OnMove(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, RecyclerView.ViewHolder target)
        {
            return false;
        }

        public override void OnSwiped(RecyclerView.ViewHolder viewHolder, int direction)
        {
            int position = viewHolder.AdapterPosition;
            _adapter.PendingRemoval(position);
        }

        public override int ConvertToAbsoluteDirection(int flags, int layoutDirection)
        {
            if (swipeBack)
            {
                swipeBack = buttonShowedState != ButtonState.Gone;
                return 0;
            }
            return base.ConvertToAbsoluteDirection(flags, layoutDirection);
        }

        public override void OnChildDraw(Canvas c, RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, float dX, float dY, int actionState, bool isCurrentlyActive)
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

            background.SetBounds(itemView.Right + (int)dX, itemView.Top, itemView.Right, itemView.Bottom);
            background.Draw(c);

            int itemHeight = itemView.Bottom - itemView.Top;
            int intrinsicWidth = xMark.IntrinsicWidth;
            int intrinsicHeight = xMark.IntrinsicWidth;

            int xMarkLeft = itemView.Right - xMarkMargin - intrinsicWidth;
            int xMarkRight = itemView.Right - xMarkMargin;
            int xMarkTop = itemView.Top + (itemHeight - intrinsicHeight) / 2;
            int xMarkBottom = xMarkTop + intrinsicHeight;
            xMark.SetBounds(xMarkLeft, xMarkTop, xMarkRight, xMarkBottom);
            xMark.Draw(c);

            base.OnChildDraw(c, recyclerView, viewHolder, dX, dY, actionState, isCurrentlyActive);
        }
    }
}
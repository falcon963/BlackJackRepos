using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V7.RecyclerView;
using TestProject.Droid.Controls;

namespace TestProject.Droid.Adapter
{
    public class ImageViewHolder: RecyclerView.ViewHolder
    {
        public TextView Text { get; private set; }
        public CircleImageView Image { get; private set; }
        public CheckBox CheckBox { get; private set; }
        public View Divider { get; private set; }
        public Button UndoButton { get; private set; }

        public ImageViewHolder(View itemView, Action<Int32> listener): base(itemView)
        {
            Text = itemView.FindViewById<TextView>(Resource.Id.task_name);
            Image = itemView.FindViewById<CircleImageView>(Resource.Id.tasklist_image);
            CheckBox = itemView.FindViewById<CheckBox>(Resource.Id.list_checkbox);
            Divider = itemView.FindViewById<View>(Resource.Id.divider);
            UndoButton = itemView.FindViewById<Button>(Resource.Id.undoButton);

            itemView.Click += (sender, e) => listener(obj: base.AdapterPosition);
        }
    }
}
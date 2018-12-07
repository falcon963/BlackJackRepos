using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using DE.Hdodenhof.CircleImageView;
using Java.Lang;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.Views;
using MvvmCross.ViewModels;
using TestProject.Core.Models;
using TestProject.Core.ViewModels;
using TestProject.Droid.Fragments;
using Boolean = System.Boolean;

namespace TestProject.Droid.Adapter
{
    public class RecyclerImageAdapter 
        : RecyclerView.Adapter
    {
        public List<UserTask> _tasksList;
        public List<UserTask> _tasksListPendingRemoval;
        public event EventHandler<Int32> ItemClick;
        private readonly Int32 PENDING_REMOVAL_TIMEOUT = 3000;

        private Handler _handler = new Handler();
        Dictionary<UserTask, Action> _pendingRunnables = new Dictionary<UserTask, Action>();

        public RecyclerImageAdapter(TasksFragment view)
        {
            this.ItemClick += (sender, e) => { view.ViewModel.ItemSelectedCommand.Execute(view.ViewModel.ListOfTasks[e]); };
            _tasksList = view.ViewModel.ListOfTasks.ToList();
            _tasksListPendingRemoval = new List<UserTask>();
        }

        public override RecyclerView.ViewHolder 
            OnCreateViewHolder(ViewGroup parent, Int32 viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ListItemView, parent, false);
            ImageViewHolder holder = new ImageViewHolder(itemView, OnClick);
            return holder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, Int32 position)
        {
            ImageViewHolder viewHolder = holder as ImageViewHolder;

            UserTask item = _tasksList[position];

            Boolean contains = _tasksListPendingRemoval.Contains(item);

            if (contains)
            {
                viewHolder.ItemView.SetBackgroundColor(Color.Red);
                viewHolder.CheckBox.Visibility = ViewStates.Gone;
                viewHolder.Image.Visibility = ViewStates.Gone;
                viewHolder.Text.Visibility = ViewStates.Gone;
                viewHolder.Divider.Visibility = ViewStates.Gone;
                viewHolder.ItemView.SetOnClickListener(null);
                viewHolder.UndoButton.Visibility = ViewStates.Visible;
                viewHolder.UndoButton.Click += (sender, e) =>
                {
                    Action pendingRemovalRunnable = _pendingRunnables.GetValueOrDefault(item);
                    _pendingRunnables.Remove(item);
                    if (pendingRemovalRunnable != null)
                    {
                        _handler.RemoveCallbacks(pendingRemovalRunnable);
                    }
                    _tasksListPendingRemoval.Remove(item);
                    NotifyItemChanged(_tasksList.IndexOf(item));
                };
            }
            if(!contains)
            {
                viewHolder.Divider.Visibility = ViewStates.Visible;
                viewHolder.CheckBox.Visibility = ViewStates.Visible;
                viewHolder.Image.Visibility = ViewStates.Visible;
                viewHolder.Text.Visibility = ViewStates.Visible;
                viewHolder.UndoButton.Visibility = ViewStates.Gone;
                viewHolder.UndoButton.SetOnContextClickListener(null);
            }

            BitmapFactory.Options options = new BitmapFactory.Options();
            options.InSampleSize = CalculateInSampleSize(options, 60, 60);
            var imagePath = _tasksList[position].ImagePath;
            var bitmap = BitmapFactory.DecodeFile(imagePath, options);

            

            try
            {
                if (bitmap != null)
                {
                    viewHolder.Image.SetImageBitmap(bitmap);
                }
                if (bitmap == null)
                {
                    viewHolder.Image.SetImageResource(Resource.Drawable.placeholder);
                }
            }
            catch (Java.Lang.OutOfMemoryError)
            {
                System.GC.Collect();
            }

            if (_tasksList.ToList().Count == position + 1)
            {
                viewHolder.Divider.Visibility = ViewStates.Invisible;
            }

            viewHolder.Text.Text = _tasksList[position].Title;
            viewHolder.CheckBox.Checked = _tasksList[position].Status;
            
        }

        public void PendingRemoval(Int32 position)
        {
            UserTask item = _tasksList[position];
            if (!_tasksListPendingRemoval.Contains(item))
            {
                _tasksListPendingRemoval.Add(item);
                NotifyItemChanged(position);
                Action action = () => { Remove(position); };
                _handler.PostDelayed(action, PENDING_REMOVAL_TIMEOUT);
                _handler.RemoveCallbacks(action);
                _pendingRunnables.Add(item, action);
            }
        }

        public void Remove(Int32 position)
        {
            UserTask item = _tasksList[position];
            if (_tasksListPendingRemoval.Contains(item))
            {
                _tasksListPendingRemoval.Remove(item);
            }
            if (_tasksList.Contains(item))
            {
                _tasksList.Remove(item);
                NotifyItemChanged(position);
            }
        }

        public Boolean IsPendingRemoval(int position)
        {
            UserTask item = _tasksList[position];
            return _tasksListPendingRemoval.Contains(item);
        }

        public override Int32 ItemCount
        {
            get { return _tasksList.Count; }
        }

        public void OnClick(Int32 position)
        {
            ItemClick?.Invoke(this, position);
        }


        public static Int32 CalculateInSampleSize(BitmapFactory.Options options, Int32 reqWidth, Int32 reqHeight)
        {

            Int32 height = options.OutHeight;
            Int32 width = options.OutWidth;
            Int32 inSampleSize = 1;

            if (height > reqHeight
                || width > reqWidth)
            {

                Int32 halfHeight = height / 2;
                Int32 halfWidth = width / 2;


                while ((halfHeight / inSampleSize) >= reqHeight
                        && (halfWidth / inSampleSize) >= reqWidth)
                {
                    inSampleSize *= 2;
                }
            }

            return inSampleSize;
        }
    }
}
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

namespace TestProject.Droid.Adapters
{
    public class RecyclerImageAdapter 
        : RecyclerView.Adapter
    {
        private List<UserTask> _tasksList;
        private List<UserTask> _tasksListPendingRemoval;
        public event EventHandler<int> ItemClick;
        private readonly int PENDING_REMOVAL_TIMEOUT = 3000;
        private TasksFragment _tasksFragment;

        private Handler _handler = new Handler();
        Dictionary<UserTask, Action> _pendingRunnables = new Dictionary<UserTask, Action>();

        public List<UserTask> Tasks
        {
            get
            {
                return _tasksList;
            }
            set
            {
                _tasksList = value;
            }
        }

        public RecyclerImageAdapter(TasksFragment view)
        {
            this.ItemClick += (sender, e) => { view.ViewModel.ItemSelectedCommand.Execute(view.ViewModel.ListOfTasks[e]); };
            Tasks = view.ViewModel.ListOfTasks.ToList();
            _tasksListPendingRemoval = new List<UserTask>();
            this.NotifyDataSetChanged();
            _tasksFragment = view;
        }

        public override RecyclerView.ViewHolder 
            OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ListItemView, parent, false);
            ImageViewHolder holder = new ImageViewHolder(itemView, OnClick);
            return holder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            ImageViewHolder viewHolder = holder as ImageViewHolder;

            UserTask item = Tasks[position];

            bool contains = _tasksListPendingRemoval.Contains(item);

            if (contains)
            {
                viewHolder.ItemView.SetBackgroundColor(new Color(251, 192, 45));
                viewHolder.CheckBox.Visibility = ViewStates.Gone;
                viewHolder.Image.Visibility = ViewStates.Gone;
                viewHolder.Text.Visibility = ViewStates.Gone;
                viewHolder.Divider.Visibility = ViewStates.Gone;
                viewHolder.ItemView.SetOnClickListener(null);
                viewHolder.DeleteButton.Visibility = ViewStates.Visible;
                viewHolder.DeleteButton.Click += (sender, e) =>
                {
                    Action pendingRemovalRunnable = _pendingRunnables.GetValueOrDefault(item);
                    _pendingRunnables.Remove(item);
                    if (pendingRemovalRunnable != null)
                    {
                        _handler.RemoveCallbacks(pendingRemovalRunnable);
                    }
                    UserTask task = _tasksFragment.ViewModel.ListOfTasks[position];
                    _tasksFragment.ViewModel.DeleteTaskCommand.Execute(task);
                    _tasksFragment.ViewModel.ListOfTasks.Remove(task);
                    Tasks.Remove(item);
                };
            }
            if(!contains)
            {
                viewHolder.Divider.Visibility = ViewStates.Visible;
                viewHolder.CheckBox.Visibility = ViewStates.Visible;
                viewHolder.Image.Visibility = ViewStates.Visible;
                viewHolder.Text.Visibility = ViewStates.Visible;
                viewHolder.DeleteButton.Visibility = ViewStates.Gone;
                viewHolder.DeleteButton.SetOnContextClickListener(null);
            }

            BitmapFactory.Options options = new BitmapFactory.Options();
            options.InSampleSize = CalculateInSampleSize(options, 60, 60);
            var imagePath = Tasks[position].ImagePath;
           
            

            try
            {
                var bitmap = BitmapFactory.DecodeFile(imagePath, options);
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

            if (_tasksFragment.ViewModel.ListOfTasks.ToList().Count == position + 1)
            {
                viewHolder.Divider.Visibility = ViewStates.Invisible;
            }

            viewHolder.Text.Text = Tasks[position].Title;
            viewHolder.CheckBox.Checked = Tasks[position].Status;
            
        }

        public void PendingRemoval(int position)
        {
            UserTask item = Tasks[position];
            if (!_tasksListPendingRemoval.Contains(item))
            {
                _tasksListPendingRemoval.Add(item);
                NotifyItemChanged(position);
                Action action = () => { Remove(Tasks.IndexOf(item)); };
                _handler.PostDelayed(action, PENDING_REMOVAL_TIMEOUT);
                if (!_pendingRunnables.ContainsKey(item))
                {
                    _pendingRunnables.Add(item, action);
                }
            }
        }

        public void Remove(int position)
        {
            UserTask item = Tasks[position];
            if (_tasksListPendingRemoval.Contains(item))
            {
                _tasksListPendingRemoval.Remove(item);
            }
            if (Tasks.Contains(item))
            {
                NotifyItemChanged(position);
                NotifyItemRangeChanged(position, Tasks.Count);
            }
        }

        public Boolean IsPendingRemoval(int position)
        {
            UserTask item = Tasks[position];
            return _tasksListPendingRemoval.Contains(item);
        }

        public override int ItemCount
        {
            get { return Tasks.Count; }
        }


        public void OnClick(int position)
        {
            ItemClick?.Invoke(this, position);
        }


        public static int CalculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight)
        {

            int height = options.OutHeight;
            int width = options.OutWidth;
            int inSampleSize = 1;

            if (height > reqHeight
                || width > reqWidth)
            {

                int halfHeight = height / 2;
                int halfWidth = width / 2;


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
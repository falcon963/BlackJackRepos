using System;
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
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.Views;
using MvvmCross.ViewModels;
using TestProject.Core.Models;
using TestProject.Core.ViewModels;

namespace TestProject.Droid.Adapter
{
    public class RecyclerImageAdapter 
        : RecyclerView.Adapter
    {
        public List<UserTask> _tasksList;
        public event EventHandler<Int32> ItemClick;

        public RecyclerImageAdapter(List<UserTask> taskList)
        {
            _tasksList = taskList;
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
            BitmapFactory.Options options = new BitmapFactory.Options();
            options.InSampleSize = CalculateInSampleSize(options, 60, 60);
            var imagePath = _tasksList[position].ImagePath;
            var bitmap = BitmapFactory.DecodeFile(imagePath, options);

            ImageViewHolder viewHolder = holder as ImageViewHolder;

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
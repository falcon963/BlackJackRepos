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
using TestProject.Core.Models;

namespace TestProject.Droid
{
    public class ImageAdapter : MvxAdapter<UserTask>
    {
        private MvxListView listView;
        private Context _context;
        private Bitmap _bitmap;

        public ImageAdapter(Activity context, IMvxAndroidBindingContext bindingContext, MvxListView tableView) 
            : base(context, bindingContext)
        {
            listView = tableView;
            _context = context;
        }


        protected override View GetView(
            int position, View convertView, ViewGroup parent, int templateId)

        {
            var view = base.GetView(position, convertView, parent, templateId);
            var source = ItemsSource.Cast<UserTask>().ToList()[position];
            var imageView = view.FindViewById<TestProject.Droid.Controls.CircleImageView>(Resource.Id.tasklist_image);
            var taskView = view.FindViewById<LinearLayout>(Resource.Id.list_fragment);
            var divider = view.FindViewById<View>(Resource.Id.divider);


            if (ItemsSource.Cast<UserTask>().ToList().Count == position+1)
            {
                divider.Visibility = ViewStates.Invisible;
            }

            BitmapFactory.Options options = new BitmapFactory.Options();
            options.InSampleSize = CalculateInSampleSize(options, 60, 60);

            try
            {
                _bitmap = BitmapFactory.DecodeFile(source.ImagePath, options);
                imageView.SetImageBitmap(_bitmap);
            }
            catch (Java.Lang.OutOfMemoryError)
            {
                System.GC.Collect();
            }
            if (_bitmap == null)
            {
                imageView.SetImageResource(Resource.Drawable.placeholder);
            }                

            return view;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DE.Hdodenhof.CircleImageView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.Views;
using TestProject.Core.Models;

namespace TestProject.Droid
{
    public class ImageAdapter : MvxAdapter<UserTask>
    {
        private MvxListView listView;
        private Context _context;

        public ImageAdapter(Activity context, IMvxAndroidBindingContext bindingContext, MvxListView tableView) : base(context, bindingContext)
        {
            listView = tableView;
            _context = context;
        }


        protected override View GetView(int position, View convertView, ViewGroup parent, int templateId)

        {
            var view = base.GetView(position, convertView, parent, templateId);

            var source = ItemsSource.Cast<UserTask>().ToList()[position];

            var imageView = view.FindViewById<TestProject.Droid.Controls.CircleImageView>(Resource.Id.tasklist_image); 
            Bitmap bmImg = BitmapFactory.DecodeFile(source.ImagePath);
            if(bmImg == null)
            {
                bmImg = BitmapFactory.DecodeResource(Context.Resources ,Resource.Drawable.placeholder);
            }
            imageView.SetImageBitmap(bmImg);

            return view;
        }
    }
}
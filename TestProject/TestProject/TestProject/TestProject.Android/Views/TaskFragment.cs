
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Net;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Java.IO;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.Views;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using TestProject.Core.ViewModels;
using TestProject.Core.Interfaces;

namespace TestProject.Droid.Views
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, true)]
    [Register("testproject.droid.views.TaskFragment")]
    public class TaskFragment : BaseFragment<TaskViewModel>
    {
        protected override int FragmentId => Resource.Layout.TaskFragment;

        private LinearLayout _linearLayout;
        private Toolbar _toolbar;
        private ImageView _imageView;
        private static readonly Int32 REQUEST_CAMERA = 0;
        private static readonly Int32 SELECT_FILE = 1;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);
            _linearLayout = view.FindViewById<LinearLayout>(Resource.Id.task_linearlayout);
            _toolbar = view.FindViewById<Toolbar>(Resource.Id.fragment_toolbar);
            _imageView = view.FindViewById<ImageView>(Resource.Id.image_view);
            if (ViewModel.UserTask.Changes.ImagePath != null)
            {
                Bitmap bmImg = BitmapFactory.DecodeFile(ViewModel.UserTask.Changes.ImagePath);
                _imageView.SetImageBitmap(bmImg);
            }
            if (ViewModel.UserTask.Changes.ImagePath == null)
            {
                Bitmap bmImg = BitmapFactory.DecodeResource(Context.Resources, Resource.Drawable.placeholder);
                _imageView.SetImageBitmap(bmImg);
            }

                _imageView.Click += OnAddPhotoClicked;
            _linearLayout.Click += delegate { HideSoftKeyboard(); };
            _toolbar.Click += delegate { HideSoftKeyboard(); };


            return view;
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            


            if (requestCode == 0)
            {
               var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
               string name = "Test_Project_" + System.DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";
                var filePath = System.IO.Path.Combine(sdCardPath, name);
               var stream = new FileStream(filePath, FileMode.Create);

                Bitmap photo = (Bitmap)data.Extras.Get("data");
                photo.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
                _imageView.SetImageBitmap(photo);
                stream.Close();
                ViewModel.UserTask.Changes.ImagePath = filePath;
            }
            if (requestCode == 1)
            {
                var filePath = GetRealPathFromURI(data.Data);
                Bitmap bmImg = BitmapFactory.DecodeFile(filePath);
                _imageView.SetImageBitmap(bmImg);
                ViewModel.UserTask.Changes.ImagePath = filePath;
            }
        }

        public String GetRealPathFromURI(Android.Net.Uri contentUri)
        {
                String[] proj = { Android.Provider.MediaStore.Images.Media.InterfaceConsts.Data };
                var cursor = this.Activity.ContentResolver.Query(contentUri, proj, null, null, null);
                int column_index = cursor.GetColumnIndexOrThrow(Android.Provider.MediaStore.Images.Media.InterfaceConsts.Data);
                cursor.MoveToFirst();
                return cursor.GetString(column_index);   
        }


        void OnAddPhotoClicked(object sender, EventArgs e)
        {
            var popup = new PopupMenu(Activity, _imageView);
            popup.Menu.Add("Camera");
            popup.Menu.Add("Gallery");
            popup.MenuItemClick += OnMenuItemClicked;
            popup.Show();
        }

        private void OnMenuItemClicked(object sender, PopupMenu.MenuItemClickEventArgs e)
        {
            var label = e.Item.TitleFormatted.ToString();

            if(label == "Camera")
            {
                var intent = new Intent(MediaStore.ActionImageCapture);
                this.StartActivityForResult(intent, REQUEST_CAMERA);
            }
            if(label == "Gallery")
            {
                var intent = new Intent(Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri);
                intent.SetType("image/*");
                this.StartActivityForResult(Intent.CreateChooser(intent, "Select Picture"), SELECT_FILE);
            }
        }

        public void HideSoftKeyboard()
        {
            InputMethodManager close = (InputMethodManager)Activity.GetSystemService(Context.InputMethodService);
            close.HideSoftInputFromWindow(_linearLayout.WindowToken, 0);
        }

        public override void OnDestroyView()
        {

            InputMethodManager inputManager = (InputMethodManager)Activity.GetSystemService(Context.InputMethodService);
            var currentFocus = Activity.CurrentFocus;
            inputManager.HideSoftInputFromWindow(currentFocus.WindowToken, 0);
            base.OnDestroyView();
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Java.IO;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using TestProject.Core.ViewModels;
using TestProject.Droid.Views;

namespace TestProject.Droid.Fragments
{
    [MvxFragmentPresentation(
        typeof(MainViewModel),
        Resource.Id.content_frame,
        true)]
    [Register("testproject.droid.fragments.ProfileFragment")]
    public class ProfileFragment
        : BaseFragment<UserProfileViewModel>
    {
        private LinearLayout _linearLayout;
        private ImageView _imageView;
        private static readonly Int32 REQUEST_CAMERA = 0;
        private static readonly Int32 SELECT_FILE = 1;
        private Bitmap _bitmap;
        private Android.Net.Uri _uriImage;

        protected override int FragmentId => Resource.Layout.ProfileFragment;

        public Android.Net.Uri ImageUri
        {
            get
            {
                return _uriImage;
            }
            set
            {
                _uriImage = value;
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            _linearLayout = view.FindViewById<LinearLayout>(Resource.Id.profileLinearLayout);
            _imageView = view.FindViewById<ImageView>(Resource.Id.profileImage_view);

            ((MainActivity)ParentActivity).DrawerLayout.SetDrawerLockMode(DrawerLayout.LockModeLockedClosed);

            _imageView.Click += OnAddPhotoClicked;
            _linearLayout.Click += delegate { HideSoftKeyboard(); };

            if (ViewModel.Profile.ImagePath == null)
            {
                try
                {
                    _bitmap = BitmapFactory.DecodeResource(Context.Resources, Resource.Drawable.placeholder);
                    _imageView.SetImageBitmap(_bitmap);
                }
                catch (Java.Lang.OutOfMemoryError)
                {
                    System.GC.Collect();
                }
            }

            if (ViewModel.Profile.ImagePath != null)
            {
                BitmapFactory.Options options = new BitmapFactory.Options();
                options.InSampleSize = 3;
                try
                {
                    _bitmap = BitmapFactory.DecodeFile(ViewModel.Profile.ImagePath, options);
                    _imageView.SetImageBitmap(_bitmap);
                }
                catch (Java.Lang.OutOfMemoryError)
                {
                    System.GC.Collect();
                }
            }


            return view;
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);



            if (resultCode == -1 && requestCode == 0)
            {
                Bitmap bitmap = BitmapFactory.DecodeFile(ImageUri.Path);

                using (MemoryStream writer = new MemoryStream())
                {
                    bitmap.Compress(Bitmap.CompressFormat.Jpeg, 40, writer);
                    Java.IO.File resizeFile = new Java.IO.File(ImageUri.Path);
                    resizeFile.CreateNewFile();
                    FileOutputStream fos = new FileOutputStream(resizeFile);
                    fos.Write(writer.ToArray());
                    fos.Close();
                    BitmapFactory.Options options = new BitmapFactory.Options();
                    options.InSampleSize = 3;
                    Bitmap _bitmap = BitmapFactory.DecodeFile(ImageUri.Path, options);
                    _imageView.SetImageBitmap(_bitmap);
                    ViewModel.Profile.ImagePath = ImageUri.Path;
                }
            }
            if (resultCode == -1
                && requestCode == 1)
            {
                var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
                string name = "Test_Project_" + System.DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";
                var filePath = System.IO.Path.Combine(sdCardPath, name);
                var stream = new FileStream(filePath, FileMode.Create);

                Bitmap bitmap = BitmapFactory.DecodeFile(GetRealPathFromURI(data.Data));
                try
                {
                    using (MemoryStream writer = new MemoryStream())
                    {
                        bitmap.Compress(Bitmap.CompressFormat.Jpeg, 40, writer);
                        Java.IO.File resizeUri = GetPhotoFileUri("Test_Project_" + System.DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_resize" + ".jpg");
                        Java.IO.File resizeFile = new Java.IO.File(resizeUri.Path);
                        resizeFile.CreateNewFile();
                        FileOutputStream fos = new FileOutputStream(resizeFile);
                        fos.Write(writer.ToArray());
                        fos.Close();
                        BitmapFactory.Options options = new BitmapFactory.Options();
                        options.InSampleSize = 3;
                        Bitmap _bitmap = BitmapFactory.DecodeFile(resizeUri.Path, options);
                        _imageView.SetImageBitmap(_bitmap);
                        ViewModel.Profile.ImagePath = resizeUri.Path;
                    }
                }
                catch (Java.Lang.OutOfMemoryError)
                {
                    System.GC.Collect();
                }

            }
            if (resultCode == 0)
            {

            }
        }

        private Android.Net.Uri GetImageUri(Context context, Bitmap inImage)
        {
            String path = MediaStore.Images.Media.InsertImage(context.ContentResolver, inImage, "Title", null);
            return Android.Net.Uri.Parse(path);
        }


        public Java.IO.File GetPhotoFileUri(String fileName)
        {
            Java.IO.File mediaStorageDir = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath);

            Java.IO.File file = new Java.IO.File(mediaStorageDir.Path + Java.IO.File.Separator + fileName);

            return file;
        }

        public String GetRealPathFromURI(Android.Net.Uri contentUri)
        {
            String[] proj = { Android.Provider.MediaStore.Images.Media.InterfaceConsts.Data };
            var cursor = this.Activity.ContentResolver.Query(contentUri, proj, null, null, null);
            int column_index = cursor.GetColumnIndexOrThrow(Android.Provider.MediaStore.Images.Media.InterfaceConsts.Data);

            cursor.MoveToFirst();

            return cursor.GetString(column_index);
        }

        public void OnAddPhotoClicked(object sender, EventArgs e)
        {
            var popup = new Android.Support.V7.Widget.PopupMenu(Activity, _imageView);
            popup.Menu.Add("Camera");
            popup.Menu.Add("Gallery");
            popup.Menu.Add("Cancel");
            popup.MenuItemClick += OnMenuItemClicked;
            popup.Show();
        }

        private void OnMenuItemClicked(object sender, Android.Support.V7.Widget.PopupMenu.MenuItemClickEventArgs e)
        {
            var label = e.Item.TitleFormatted.ToString();

            if (label == "Camera")
            {
                var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
                string name = "Test_Project_" + System.DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";
                var filePath = System.IO.Path.Combine(sdCardPath, name);
                Java.IO.File image = new Java.IO.File(filePath);
                ImageUri = Android.Net.Uri.FromFile(image);
                var intent = new Intent(MediaStore.ActionImageCapture);
                intent.PutExtra(MediaStore.ExtraOutput, ImageUri);
                this.StartActivityForResult(intent, REQUEST_CAMERA);
            }
            if (label == "Gallery")
            {
                var intent = new Intent(Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri);
                intent.SetType("image/*");
                this.StartActivityForResult(Intent.CreateChooser(intent, "Select Picture"), SELECT_FILE);
            }
            if (label == "Cancel")
            {

            }
        }

        public void HideSoftKeyboard()
        {
            InputMethodManager close = (InputMethodManager)Activity.GetSystemService(Context.InputMethodService);
            close.HideSoftInputFromWindow(_linearLayout.WindowToken, 0);
        }
    }
}
using System;
using System.IO;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using TestProject.Core.ViewModels;
using TestProject.Droid.Views;
using File = Java.IO.File;
using Uri = Android.Net.Uri;

namespace TestProject.Droid.Fragments
{
    [MvxFragmentPresentation(
        typeof(MainViewModel),
        Resource.Id.content_frame,
        true)]
    [Register("testproject.droid.fragments.ProfileFragment")]
    public class ProfileFragment
        : BaseFragment<ProfileViewModel>
    {
        private LinearLayout _linearLayout;
        private ImageView _imageView;
        private static readonly int REQUEST_CAMERA = 0;
        private static readonly int SELECT_FILE = 1;
        private Bitmap _bitmap;

        protected override int FragmentId => Resource.Layout.ProfileFragment;

        public Uri ImageUri { get; set; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            _linearLayout = view.FindViewById<LinearLayout>(Resource.Id.profileLinearLayout);
            _imageView = view.FindViewById<ImageView>(Resource.Id.profileImage_view);

            ((MainActivity)ParentActivity).DrawerLayout.SetDrawerLockMode(DrawerLayout.LockModeLockedClosed);

            _imageView.Click += OnAddPhotoClicked;
            _linearLayout.Click += delegate { HideSoftKeyboard(); };

            return view;
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == -1 && requestCode == 0)
            {
                Bitmap bitmapImage = BitmapFactory.DecodeFile(ImageUri.Path);

                SaveImage(bitmapImage);
            }

            if (resultCode == -1
                && requestCode == 1)
            {
                Bitmap bitmapImage = BitmapFactory.DecodeFile(GetRealPathFromURI(data.Data));

                SaveImage(bitmapImage);
            }

            if (resultCode == 0)
            {

            }
        }

        private Uri GetImageUri(Context context, Bitmap inImage)
        {
            string path = MediaStore.Images.Media.InsertImage(context.ContentResolver, inImage, "Title", null);

            var imageUri = Uri.Parse(path);

            return imageUri;
        }


        public File GetPhotoFileUri(string fileName)
        {
            File mediaStorageDir = new File(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath);

            File file = new File(mediaStorageDir.Path + File.Separator + fileName);

            return file;
        }

        public string GetRealPathFromURI(Uri contentUri)
        {
            string[] proj = { MediaStore.Images.Media.InterfaceConsts.Data };

            var cursor = this?.Activity?.ContentResolver?.Query(contentUri, proj, null, null, null);
            int column_index = cursor.GetColumnIndexOrThrow(MediaStore.Images.Media.InterfaceConsts.Data);

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
                string name = "Test_Project_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";
                var filePath = System.IO.Path.Combine(sdCardPath, name);

                File image = new File(filePath);

                ImageUri = Uri.FromFile(image);

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

        public void SaveImage(Bitmap image)
        {
            try
            {
                using (MemoryStream writer = new MemoryStream())
                {
                    image.Compress(Bitmap.CompressFormat.Png, 40, writer);

                    byte[] byteArray = writer.ToArray();

                    string encodedImage = Base64.EncodeToString(byteArray, Base64Flags.Default);

                    ViewModel.Profile.ImagePath = encodedImage;
                }
            }
            catch (Java.Lang.OutOfMemoryError)
            {
                GC.Collect();
            }
        }

        public void HideSoftKeyboard()
        {
            InputMethodManager close = (InputMethodManager)Activity.GetSystemService(Context.InputMethodService);

            close.HideSoftInputFromWindow(_linearLayout.WindowToken, 0);
        }
    }
}
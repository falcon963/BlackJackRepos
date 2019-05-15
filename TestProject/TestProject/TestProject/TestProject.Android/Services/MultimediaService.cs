using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TestProject.Droid.Services.Interfaces;
using Path = System.IO.Path;
using File = Java.IO.File;
using Uri = Android.Net.Uri;
using TestProject.Droid.Fragments;
using Android.Graphics;
using Android.Util;
using TestProject.LanguageResources;
using MvvmCross;
using TestProject.Droid.Helpers.Interfaces;

namespace TestProject.Droid.Services
{
    public class MultimediaService<T> where T : BaseFragment
    {

        private const int REQUEST_CAMERA = 0;

        private const int REQUEST_GALLERY = 1;

        private const int RESULT_CODE_OK = -1;

        private const int RESULT_CODE_CANCEL = 0;

        private ImageView _imageView;

        private readonly T _fragment;

        private IImageHelper _imageHelper;
        private IUriHelper _uriHelper;

        private Action<string> SaveImageAction { get; set; }

        public Uri _imageUri;

        public MultimediaService(T fragment, ImageView imageView, Uri imageUri, Action<string> action)
        {
            SaveImageAction = action;
            _fragment = fragment;
            _imageView = imageView;
            _imageUri = imageUri;
            _uriHelper = Mvx.IoCProvider.Resolve<IUriHelper>();
            _imageHelper = Mvx.IoCProvider.Resolve<IImageHelper>();
        }

        public void OnAddPhotoClicked(object sender, EventArgs e)
        {
            var popup = new Android.Support.V7.Widget.PopupMenu(_fragment.Activity, _imageView);

            popup.Menu.Add(Strings.Camera);
            popup.Menu.Add(Strings.Gallery);
            popup.Menu.Add(Strings.Cancel);
            popup.MenuItemClick += OnMenuItemClicked;
            popup.Show();
        }

        private void OnMenuItemClicked(object sender, Android.Support.V7.Widget.PopupMenu.MenuItemClickEventArgs e)
        {
            var label = e.Item.TitleFormatted.ToString();

            if (label == Strings.Camera)
            {
                var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
                string time = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string name = $"Test_Project_{time}.jpg";
                var filePath = Path.Combine(sdCardPath, name);

                File image = new File(filePath);
                _imageUri = Uri.FromFile(image);

                var intent = new Intent(MediaStore.ActionImageCapture);
                intent.PutExtra(MediaStore.ExtraOutput, _imageUri);

                _fragment.StartActivityForResult(intent, REQUEST_CAMERA);
            }
            if (label == Strings.Gallery)
            {
                var intent = new Intent(Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri);
                intent.SetType("image/*");

                _fragment.StartActivityForResult(Intent.CreateChooser(intent, Strings.SelectPicture), REQUEST_GALLERY);
            }
            if (label == Strings.Cancel)
            {

            }
        }

        public void ActivityResult(int requestCode, int resultCode, Intent data, T fragment)
        {
            if (resultCode == RESULT_CODE_CANCEL)
            {
                return;
            }

            if (resultCode == RESULT_CODE_OK
                && requestCode == REQUEST_CAMERA)
            {
                Bitmap bitmapImage = BitmapFactory.DecodeFile(_imageUri.Path);

                SaveImage(bitmapImage);
            }

            if (resultCode == RESULT_CODE_OK
                && requestCode == REQUEST_GALLERY)
            {
                string realPath = _uriHelper.GetRealPathFromURI(data.Data, fragment);

                Bitmap bitmapImage = BitmapFactory.DecodeFile(realPath);

                SaveImage(bitmapImage);
            }
        }

        public void SaveImage(Bitmap image)
        {
            var encodedImage = _imageHelper.ImageEncoding(image);

            if (string.IsNullOrEmpty(encodedImage))
            {
                return;
            }

            SaveImageAction(encodedImage);
        }
    }
}
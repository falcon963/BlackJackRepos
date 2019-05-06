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

namespace TestProject.Droid.Services
{
    public class MultimediaService<T>
        : IMultimediaService where T : BaseFragment
    {
        private static readonly Int32 REQUEST_CAMERA = 0;
        private static readonly Int32 SELECT_FILE = 1;

        private ImageView _imageView;

        public Uri ImageUri { get; set; }

        private readonly T _fragment;

        public MultimediaService(T fragment, ImageView imageView, Uri imageUri)
        {
            _fragment = fragment;
            _imageView = imageView;
            ImageUri = imageUri;
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
                ImageUri = Uri.FromFile(image);

                var intent = new Intent(MediaStore.ActionImageCapture);
                intent.PutExtra(MediaStore.ExtraOutput, ImageUri);

                _fragment.StartActivityForResult(intent, REQUEST_CAMERA);
            }
            if (label == Strings.Gallery)
            {
                var intent = new Intent(Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri);
                intent.SetType("image/*");

                _fragment.StartActivityForResult(Intent.CreateChooser(intent, Strings.SelectPicture), SELECT_FILE);
            }
            if (label == Strings.Cancel)
            {

            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Uri = Android.Net.Uri;
using File = Java.IO.File;
using TestProject.Droid.Helpers.Interfaces;
using TestProject.Droid.Fragments;
using Android.Database;

namespace TestProject.Droid.Helpers
{
    public class UriHelper 
        : IUriHelper
    {

        private const string title = "Title";


        public Uri GetImageUri(Context context, Bitmap inImage)
        {
            string path = MediaStore.Images.Media.InsertImage(context.ContentResolver, inImage, title, null);

            var imageUri = Uri.Parse(path);

            return imageUri;
        }


        public File GetPhotoFileByUri(string fileName)
        {
            File mediaStorageDir = new File(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath);

            File file = new File($"{mediaStorageDir.Path}{File.Separator}{fileName}");

            return file;
        }

        public string GetRealPathFromURI(Uri contentUri, BaseFragment fragment)
        {
            string[] proj = { MediaStore.Images.Media.InterfaceConsts.Data };
            var cursor = fragment?.Activity?.ContentResolver?.Query(contentUri, proj, null, null, null);
            int column_index = cursor.GetColumnIndexOrThrow(MediaStore.Images.Media.InterfaceConsts.Data);

            cursor.MoveToFirst();

            return cursor.GetString(column_index);
        }
    }
}
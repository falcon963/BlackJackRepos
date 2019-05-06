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

namespace TestProject.Droid.Helpers
{
    public class UriHelper<T> 
        : IUriHelper<T> where T : BaseFragment
    {

        public Uri GetImageUri(Context context, Bitmap inImage)
        {
            String path = MediaStore.Images.Media.InsertImage(context.ContentResolver, inImage, "Title", null);

            var imageUri = Uri.Parse(path);

            return imageUri;
        }


        public File GetPhotoFileUri(String fileName)
        {
            File mediaStorageDir = new File(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath);

            File file = new File(mediaStorageDir.Path + File.Separator + fileName);

            return file;
        }

        public String GetRealPathFromURI(Uri contentUri, T fragment)
        {
            String[] proj = { MediaStore.Images.Media.InterfaceConsts.Data };
            var cursor = fragment?.Activity?.ContentResolver?.Query(contentUri, proj, null, null, null);
            int column_index = cursor.GetColumnIndexOrThrow(MediaStore.Images.Media.InterfaceConsts.Data);

            cursor.MoveToFirst();

            return cursor.GetString(column_index);
        }
    }
}
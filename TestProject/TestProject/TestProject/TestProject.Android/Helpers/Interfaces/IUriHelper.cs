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
using TestProject.Droid.Fragments;
using File = Java.IO.File;
using Uri = Android.Net.Uri;

namespace TestProject.Droid.Helpers.Interfaces
{
    public interface IUriHelper<T> where T: BaseFragment
    {
        Uri GetImageUri(Context context, Bitmap inImage);
        File GetPhotoFileUri(String fileName);
        String GetRealPathFromURI(Uri contentUri, T fragment);
    }
}
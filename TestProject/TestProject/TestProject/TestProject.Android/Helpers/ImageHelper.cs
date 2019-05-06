using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using TestProject.Droid.Helpers.Interfaces;

namespace TestProject.Droid.Helpers
{
    public class ImageHelper
        : IImageHelper
    {
        public string ImageEncoding(Bitmap image)
        {
            try
            {
                using (MemoryStream writer = new MemoryStream())
                {
                    image.Compress(Bitmap.CompressFormat.Png, 40, writer);

                    byte[] byteArray = writer.ToArray();
                    string encodedImage = Base64.EncodeToString(byteArray, Base64Flags.Default);

                    return encodedImage;
                }
            }
            catch (Java.Lang.OutOfMemoryError)
            {
                GC.Collect();
            }

            return string.Empty;
        }
    }
}
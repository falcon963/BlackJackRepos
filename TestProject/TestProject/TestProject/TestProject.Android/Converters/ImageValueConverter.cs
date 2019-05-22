using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross;
using MvvmCross.Converters;
using TestProject.Droid.Providers.Interfaces;

namespace TestProject.Droid.Converters
{
    public class ImageValueConverter
        : MvxValueConverter<string, Bitmap>
    {
        protected override Bitmap Convert(string encodedImage, Type targetType, object parameter, CultureInfo culture)
        {
            if (!string.IsNullOrEmpty(encodedImage))
            {
                byte[] decodedString = Base64.Decode(encodedImage, Base64Flags.Default);

                Bitmap decodedByte = BitmapFactory.DecodeByteArray(decodedString, 0, decodedString.Length);

                return decodedByte;
            }

            var contextHelper = Mvx.Resolve<IContextProvider>();
            var context = contextHelper.GetContext();

            var placeholder = BitmapFactory.DecodeResource(context.Resources , Resource.Drawable.placeholder);

            return placeholder;
        }
    }
}
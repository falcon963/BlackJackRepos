using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Foundation;
using MvvmCross.Converters;
using UIKit;

namespace TestProject.iOS.Converters
{
    public class ImageValueConverter
        : MvxValueConverter<String, UIImage>
    {
        protected override UIImage Convert(String value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
            {
                UIImage image = UIImage.FromFile("placeholder.png");
                return image;
            }
            var data = new NSData(value, NSDataBase64DecodingOptions.IgnoreUnknownCharacters);
            //Byte[] data1 = System.Convert.FromBase64String(value);
           //NSData r = NSData.FromArray(data1);
            return UIImage.LoadFromData(data);
        }
    }
}
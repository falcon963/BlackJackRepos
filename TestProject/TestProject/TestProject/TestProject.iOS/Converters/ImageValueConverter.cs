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
        : MvxValueConverter<string, UIImage>
    {
        protected override UIImage Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
            {
                UIImage image = UIImage.FromFile("placeholder.png");
                return image;
            }
            var data = new NSData(value, NSDataBase64DecodingOptions.IgnoreUnknownCharacters);
            return UIImage.LoadFromData(data);
        }
    }
}
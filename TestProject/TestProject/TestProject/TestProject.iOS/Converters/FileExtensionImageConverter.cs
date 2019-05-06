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
    public class FileExtensionImageConverter 
        : MvxValueConverter<String, UIImage>
    {
        protected override UIImage Convert(String value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(value))
            {
                return UIImage.FromBundle("Placeholder");
            }

            return UIImage.FromBundle(value);
        }
    }
}
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
        : MvxValueConverter<string, UIImage>
    {
        protected override UIImage Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(value))
            {
                return UIImage.FromBundle("Placeholder");
            }
            
            var extensionImage = UIImage.FromBundle(value);

            return extensionImage;
        }
    }
}
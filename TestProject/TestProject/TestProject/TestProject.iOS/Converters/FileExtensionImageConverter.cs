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
            if (value == "png")
            {
                UIImage image = UIImage.FromFile("png_24.png");
                return image;
            }
            if (value == "pdf")
            {
                UIImage image = UIImage.FromFile("pdf_24.png");
                return image;
            }
            if (value == "rtf")
            {
                UIImage image = UIImage.FromFile("rtf_24.png");
                return image;
            }
            if (value == "jpg")
            {
                UIImage image = UIImage.FromFile("jpg_24.png");
                return image;
            }
            if (value == "txt")
            {
                UIImage image = UIImage.FromFile("txt_24.png");
                return image;
            }
            return UIImage.FromFile("placeholder.png");
        }
    }
}
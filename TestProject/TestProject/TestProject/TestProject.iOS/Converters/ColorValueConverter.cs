using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Foundation;
using MvvmCross.Converters;
using MvvmCross.Platform.UI;
using UIKit;

namespace TestProject.iOS.Converters
{
    public class ColorValueConverter
        : MvxValueConverter<MvxColor, UIColor>
    {
        protected override UIColor Convert(MvxColor value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = UIColor.FromRGB(value.R, value.G, value.B);
            return color;
        }
    }
}
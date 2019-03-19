using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Foundation;
using MvvmCross.Converters;
using MvvmCross.Platform.UI;
using MvvmCross.Plugin.Color;
using MvvmCross.UI;
using UIKit;

namespace TestProject.iOS.Converters
{
    public class ColorValueConverter
        : MvxColorValueConverter<MvvmCross.UI.MvxColor>
    {

        protected override MvvmCross.UI.MvxColor Convert(MvvmCross.UI.MvxColor value, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
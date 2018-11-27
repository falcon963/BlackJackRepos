using System;
using System.Globalization;
using Android.Graphics;
using Android.Graphics.Drawables;
using MvvmCross.Converters;
using MvvmCross.Plugin.Color;
using MvvmCross.UI;

namespace TestProject.Droid.Converter
{
    public class ColorValueConverter
        : MvxValueConverter<MvxColor, ColorDrawable>
    {
        protected override ColorDrawable Convert(MvxColor value, Type targetType, object parameter, CultureInfo culture)
        { 
            var color = Color.Rgb(value.R, value.G, value.B);
            var drawable = new ColorDrawable(color);
            return drawable;
        }
    }
}
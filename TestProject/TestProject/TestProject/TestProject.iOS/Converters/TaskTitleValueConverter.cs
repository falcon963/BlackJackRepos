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
    public class TaskTitleValueConverter
        : MvxValueConverter<String, String>
    {
        protected override string Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
            {
                String title = "New Task";
                return title;
            }
            return value;
        }
    }
}
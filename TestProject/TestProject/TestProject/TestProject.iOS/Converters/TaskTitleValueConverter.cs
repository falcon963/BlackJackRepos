using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Foundation;
using MvvmCross.Converters;
using TestProject.LanguageResources;
using UIKit;

namespace TestProject.iOS.Converters
{
    public class TaskTitleValueConverter
        : MvxValueConverter<string, string>
    {
        protected override string Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(value))
            {
                string title = Strings.NewTask;
                return title;
            }
            return value;
        }
    }
}
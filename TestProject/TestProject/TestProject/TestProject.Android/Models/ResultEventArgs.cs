using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TestProject.Droid.Models
{
    public class ResultEventArgs
    {
        public int RequestCode { get; set; }
        public int ResultCode { get; set; }
        public Intent Data { get; set; }

        public ResultEventArgs(int requestCode, int resultCode, Intent data)
        {
            RequestCode = requestCode;
            ResultCode = resultCode;
            Data = data;
        }
    }
}
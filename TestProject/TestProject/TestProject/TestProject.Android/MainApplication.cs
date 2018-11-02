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
using TestProject.Core.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;
using TestProject.Core;

namespace TestProject.Droid
{
    [Application]
    public class MainApplication : MvxAppCompatApplication<Setup, TestProject.Core.App>
    {
        public MainApplication()
        {

        }

        public MainApplication(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }
    }
}
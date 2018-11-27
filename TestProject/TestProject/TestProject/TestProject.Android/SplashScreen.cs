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
using Android.Content.PM;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using Android;

namespace TestProject.Droid
{
    [MvxActivityPresentation]
    [Activity(MainLauncher = true,
        Icon = "@mipmap/icon",
        Theme = "@style/Theme.Splash",
        NoHistory = true,
        ScreenOrientation = ScreenOrientation.Portrait)]

    public class SplashScreen 
        : MvxSplashScreenActivity
    {
        public SplashScreen()
             : base(Resource.Layout.SplashScreen)
        {
        }
    }
}
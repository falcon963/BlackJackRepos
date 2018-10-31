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
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using TestProject.Core.ViewModels;
using Android.Support.V4.Widget;
using Android.Support.V4.View;
using Android.Views.InputMethods;
using Acr.UserDialogs;

namespace TestProject.Droid.Views
{
    [MvxActivityPresentation]
    [Activity
        (ScreenOrientation = ScreenOrientation.Portrait,
        Label = "Login TaskList Project",
        Theme = "@style/AppTheme",
        LaunchMode = LaunchMode.SingleTop,
        Name = "testProject.droid.views.LoginActivity"
        )]
    public class LoginActivity : MvxAppCompatActivity<LoginViewModel>
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            UserDialogs.Init(this);

            SetContentView(Resource.Layout.LoginActivity);
        }

        protected override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.LoginActivity);
            base.OnViewModelSet();
        }
    }
}
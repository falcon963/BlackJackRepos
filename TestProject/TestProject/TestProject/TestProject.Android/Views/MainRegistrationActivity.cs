using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using TestProject.Core.ViewModels;
using Xamarin.Facebook;

namespace TestProject.Droid.Views
{
    [MvxActivityPresentation]
    [Activity(WindowSoftInputMode = SoftInput.AdjustPan,
        ScreenOrientation = ScreenOrientation.Portrait,
        Theme = "@style/AppTheme",
        Name = "testProject.droid.views.MainRegistrationActivity")]
    public class MainRegistrationActivity 
        : MvxAppCompatActivity<MainRegistrationViewModel>
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            UserDialogs.Init(this);

            Window.SetSoftInputMode(SoftInput.AdjustResize);

            SetContentView(Resource.Layout.MainRegistrationActivity);

            FacebookSdk.SdkInitialize(ApplicationContext);

            if (bundle == null)
            {
                ViewModel?.ShowLoginPageCommand?.Execute();
            }
        }

        public void HideSoftKeyboard()
        {
            if (CurrentFocus == null)
            {
                return;
            }

            InputMethodManager inputMethodManager = (InputMethodManager)GetSystemService(InputMethodService);
            inputMethodManager.HideSoftInputFromWindow(CurrentFocus.WindowToken, 0);

            CurrentFocus.ClearFocus();
        }

        public override void OnBackPressed()
        {
            return;
        }
    }
}
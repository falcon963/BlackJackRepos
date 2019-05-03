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
using Android.Graphics;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace TestProject.Droid.Views
{
    [MvxActivityPresentation]
    [Activity(WindowSoftInputMode = SoftInput.AdjustPan,
        ScreenOrientation = ScreenOrientation.Portrait,
        Label = "TaskList Project",
        Theme = "@style/AppTheme",
        Name = "testProject.droid.views.MainActivity")]
    public class MainActivity : MvxAppCompatActivity<MainViewModel>
    {
        public DrawerLayout DrawerLayout { get; set; }

        ActionBarDrawerToggle DrawerToggle { get; set; }

        Toolbar Toolbar { get; set; }

        private bool _toolBarNavigationListenerIsRegistered = false;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            UserDialogs.Init(this);

            Window.SetSoftInputMode(SoftInput.AdjustResize);

            SetContentView(Resource.Layout.MainActivity);

            DrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            if(bundle == null)
            {
                ViewModel?.ShowMenuCommand?.Execute(null);
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    DrawerLayout.OpenDrawer(GravityCompat.Start);
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void ShowBackButton()
        {

            DrawerLayout.SetDrawerLockMode(DrawerLayout.LockModeLockedClosed);
        }

        private void ShowHamburguerMenu()
        {
            DrawerLayout.SetDrawerLockMode(DrawerLayout.LockModeUnlocked);
        }

        public override void OnBackPressed()
        {
            if (DrawerLayout != null && DrawerLayout.IsDrawerOpen(GravityCompat.Start))
            {
                DrawerLayout.CloseDrawers();
            }

            return;
        }

        public void HideSoftKeyboard()
        {
            if (CurrentFocus == null) return;

            InputMethodManager inputMethodManager = (InputMethodManager)GetSystemService(InputMethodService);
            inputMethodManager.HideSoftInputFromWindow(CurrentFocus.WindowToken, 0);

            CurrentFocus.ClearFocus();
        }
    }
}
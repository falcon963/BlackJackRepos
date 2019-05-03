using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using MvvmCross.ViewModels;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Support.V4;
using Android.Content.Res;
using Android.Support.V7.Widget;
using TestProject.Droid.Views;

namespace TestProject.Droid.Fragments
{
    public abstract class BaseFragment
        : MvxFragment
    {
        protected Toolbar Toolbar { get; private set; }

        protected MvxActionBarDrawerToggle DrawerToggle { get; private set; }

        protected abstract int FragmentId { get; }

        protected bool ShowHumburgerMenu { get; set; } = false;

        public BaseFragment()
        {
            RetainInstance = true;
        }

        public MvxAppCompatActivity ParentActivity
        {
            get
            {
                return (MvxAppCompatActivity)Activity;
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(FragmentId, null);

            Toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar);

            if (Toolbar != null)
            {
                ParentActivity.SetSupportActionBar(Toolbar);
                if (ShowHumburgerMenu)
                {
                    ParentActivity.SupportActionBar?.SetDisplayHomeAsUpEnabled(true);

                    DrawerToggle = new MvxActionBarDrawerToggle(
                        Activity,
                        ((MainActivity)ParentActivity).DrawerLayout,
                        Toolbar,
                        Resource.String.drawer_open,
                        Resource.String.drawer_close
                    );

                    DrawerToggle.DrawerOpened += (object sender, ActionBarDrawerEventArgs e) => ((MainActivity)Activity)?.HideSoftKeyboard();

                    ((MainActivity)ParentActivity).DrawerLayout.AddDrawerListener(DrawerToggle);
                }
            }

            return view;
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);

            if (Toolbar != null)
            {
                DrawerToggle?.OnConfigurationChanged(newConfig);
            }
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            if (Toolbar != null)
            {
                DrawerToggle?.SyncState();
            }
        }

    }

    public  abstract class BaseFragment<TViewModel> 
        : BaseFragment where TViewModel : class, IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}
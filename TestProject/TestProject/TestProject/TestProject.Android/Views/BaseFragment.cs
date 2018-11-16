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

namespace TestProject.Droid.Views
{
    public abstract class BaseFragment : MvxFragment
    {
       private Toolbar _toolbar;

       private MvxActionBarDrawerToggle _drawerToggle;

        protected abstract Int32 FragmentId { get; }

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
            _toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar);
            if (_toolbar != null)
            {
                ParentActivity.SetSupportActionBar(_toolbar);
                ParentActivity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                _drawerToggle = new MvxActionBarDrawerToggle(
                    Activity,                               
                    ((MainActivity)ParentActivity).DrawerLayout,  
                    _toolbar,                              
                    Resource.String.drawer_open,            
                    Resource.String.drawer_close            
                );
                _drawerToggle.DrawerOpened += (object sender, ActionBarDrawerEventArgs e) => ((MainActivity)Activity)?.HideSoftKeyboard();
                ((MainActivity)ParentActivity).DrawerLayout.AddDrawerListener(_drawerToggle);
            }

            return view;
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            if (_toolbar != null)
                _drawerToggle.OnConfigurationChanged(newConfig);
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            if (_toolbar != null)
                _drawerToggle.SyncState();
        }

    }

    public  abstract class BaseFragment<TViewModel> : BaseFragment where TViewModel : class, IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}
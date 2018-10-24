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
       private Android.Widget.Toolbar _toolbar;

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
            _toolbar = view.FindViewById<Android.Widget.Toolbar>(Resource.Id.toolbar);

            //if(_toolbar != null)
            //{
            //    ParentActivity.SetSupportActionBar(_toolbar);
            //    ParentActivity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            //}

            return view;
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.Graphics.Drawable;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.Views;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using TestProject.Core.Models;
using TestProject.Core.ViewModels;
using TestProject.Droid.Controls;
using TestProject.Droid.Views;

namespace TestProject.Droid.Fragments
{
    [MvxFragmentPresentation(
        typeof(MainViewModel),
        Resource.Id.navigation_frame)]
    public class MenuFragment 
        : BaseFragment<MenuViewModel>
    {
        private MvxListView _navigationView;
        private CircleImageView _avatar;
        private Bitmap _bitmap;

        protected override int FragmentId => Resource.Layout.MenuFragment;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            _navigationView = view.FindViewById<MvxListView>(Resource.Id.mvxListView_menuItem);

            _navigationView.DividerHeight = 0;

            _navigationView.ItemClick.CanExecuteChanged += (sender, e) => { ((MainActivity)Activity).DrawerLayout.CloseDrawers();  };

            return view;
        }
    }
}
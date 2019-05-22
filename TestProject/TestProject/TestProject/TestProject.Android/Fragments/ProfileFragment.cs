using System;
using System.IO;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using MvvmCross;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using TestProject.Core.Services.Interfaces;
using TestProject.Core.ViewModels;
using TestProject.Droid.Fragments.Interfaces;
using TestProject.Droid.Helpers.Interfaces;
using TestProject.Droid.Models;
using TestProject.Droid.Services;
using TestProject.Droid.Views;
using File = Java.IO.File;
using Uri = Android.Net.Uri;

namespace TestProject.Droid.Fragments
{
    [MvxFragmentPresentation(
        typeof(MainViewModel),
        Resource.Id.content_frame,
        true)]
    public class ProfileFragment
        : BaseFragment<ProfileViewModel>, IFragmentLifecycle
    {
        private ImageView _imageView;

        public event EventHandler<ResultEventArgs> SubscribeOnResult;

        protected override int _fragmentId => Resource.Layout.ProfileFragment;

        public ProfileFragment()
        {
            
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            _linearLayout = view.FindViewById<LinearLayout>(Resource.Id.profileLinearLayout);
            _imageView = view.FindViewById<ImageView>(Resource.Id.profileImage_view);

            Mvx.IoCProvider.RegisterSingleton<IImagePickerPlatformService>(new MultimediaService<ProfileFragment>(this, _imageView));

            ((MainActivity)ParentActivity).DrawerLayout.SetDrawerLockMode(DrawerLayout.LockModeLockedClosed);

            _imageView.Click += (sender, e) => { ViewModel?.PickPhotoCommand?.Execute(); };

            return view;
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            SubscribeOnResult?.Invoke(this, new ResultEventArgs(requestCode, resultCode, data));
        }
    }
}
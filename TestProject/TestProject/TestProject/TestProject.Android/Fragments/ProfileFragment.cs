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
using MvvmCross.Platforms.Android.Presenters.Attributes;
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

        private readonly MultimediaService<ProfileFragment> _multimediaService;
        private readonly IImageHelper _imageHelper;
        private readonly IUriHelper _uriHelper;

        public event EventHandler<ResultEventArgs> SubscribeOnResult;

        public Action<string> SaveImage { get; set; }

        protected override int _fragmentId => Resource.Layout.ProfileFragment;

        public Uri ImageUri { get; set; }

        public ProfileFragment(IUriHelper uriHelper, IImageHelper imageHelper)
        {
            _imageHelper = imageHelper;
            _uriHelper = uriHelper;
            SaveImage = SaveEncodedImage;
            _multimediaService = new MultimediaService<ProfileFragment>(this, _imageView);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            _linearLayout = view.FindViewById<LinearLayout>(Resource.Id.profileLinearLayout);
            _imageView = view.FindViewById<ImageView>(Resource.Id.profileImage_view);

            ((MainActivity)ParentActivity).DrawerLayout.SetDrawerLockMode(DrawerLayout.LockModeLockedClosed);

            _imageView.Click += (sender, e) => { ViewModel?.PickPhotoCommand?.Execute(); };

            return view;
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            SubscribeOnResult?.Invoke(this, new ResultEventArgs(requestCode, resultCode, data));
        }

        public void SaveEncodedImage(string encodedImage)
        {
            ViewModel.Profile.ImagePath = encodedImage;
        }
    }
}
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
using TestProject.Droid.Helpers.Interfaces;
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
        : BaseFragment<ProfileViewModel>
    {
        private ImageView _imageView;
        private Bitmap _bitmap;

        private readonly MultimediaService<ProfileFragment> _multimediaService;
        private readonly IImageHelper _imageHelper;
        private readonly IUriHelper _uriHelper;

        public Action<string> SaveImage { get; set; }

        protected override int _fragmentId => Resource.Layout.ProfileFragment;

        public Uri ImageUri { get; set; }

        public ProfileFragment(IUriHelper uriHelper, IImageHelper imageHelper)
        {
            _imageHelper = imageHelper;
            _uriHelper = uriHelper;
            SaveImage = SaveEncodedImage;
            _multimediaService = new MultimediaService<ProfileFragment>(this, _imageView, ImageUri, SaveImage);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            _linearLayout = view.FindViewById<LinearLayout>(Resource.Id.profileLinearLayout);
            _imageView = view.FindViewById<ImageView>(Resource.Id.profileImage_view);

            ((MainActivity)ParentActivity).DrawerLayout.SetDrawerLockMode(DrawerLayout.LockModeLockedClosed);

            _imageView.Click += _multimediaService.OnAddPhotoClicked;

            return view;
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            _multimediaService.ActivityResult(requestCode, resultCode, data, this);
        }

        public void SaveEncodedImage(string encodedImage)
        {
            ViewModel.Profile.ImagePath = encodedImage;
        }
    }
}
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
        private readonly IUriHelper<ProfileFragment> _uriHelper;

        protected override int FragmentId => Resource.Layout.ProfileFragment;

        public Uri ImageUri { get; set; }

        public ProfileFragment(IUriHelper<ProfileFragment> uriHelper, IImageHelper imageHelper)
        {
            _imageHelper = imageHelper;
            _uriHelper = uriHelper;
            _multimediaService = new MultimediaService<ProfileFragment>(this, _imageView, ImageUri);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            LinearLayout = view.FindViewById<LinearLayout>(Resource.Id.profileLinearLayout);
            _imageView = view.FindViewById<ImageView>(Resource.Id.profileImage_view);

            ((MainActivity)ParentActivity).DrawerLayout.SetDrawerLockMode(DrawerLayout.LockModeLockedClosed);

            _imageView.Click += _multimediaService.OnAddPhotoClicked;

            return view;
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == -1 && requestCode == 0)
            {
                Bitmap bitmapImage = BitmapFactory.DecodeFile(ImageUri.Path);

                SaveImage(bitmapImage);
            }

            if (resultCode == -1
                && requestCode == 1)
            {
                string realPath = _uriHelper.GetRealPathFromURI(data.Data, this);

                Bitmap bitmapImage = BitmapFactory.DecodeFile(realPath);

                SaveImage(bitmapImage);
            }

            if (resultCode == 0)
            {

            }
        }

        public void SaveImage(Bitmap image)
        {
            var encodedImage = _imageHelper.ImageEncoding(image);

            if (string.IsNullOrEmpty(encodedImage))
            {
                return;
            }

            ViewModel.Profile.ImagePath = encodedImage;
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Net;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Java.IO;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.Views;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using TestProject.Core.ViewModels;
using Android.Support.V7.Widget;
using Android;
using TestProject.Droid.Views;
using Android.Support.V4.Widget;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Uri = Android.Net.Uri;
using Path = System.IO.Path;
using File = Java.IO.File;
using Android.Util;
using TestProject.Droid.Services;
using TestProject.Droid.Helpers.Interfaces;
using TestProject.Droid.Models;
using TestProject.Droid.Fragments.Interfaces;
using MvvmCross.Binding.BindingContext;

namespace TestProject.Droid.Fragments
{
    [MvxFragmentPresentation(
        typeof(MainViewModel),
        Resource.Id.content_frame,
        true)]
    public class TaskFragment
        : BaseFragment<TaskViewModel>, IFragmentLifecycle
    {

        private Toolbar _toolbar;
        private ImageView _imageView;

        private readonly MultimediaService<TaskFragment> _multimediaService;
        private readonly IImageHelper _imageHelper;
        private readonly IUriHelper _uriHelper;

        public event EventHandler<ResultEventArgs> SubscribeOnResult;

        public Action<string> SaveImage { get; set; }

        protected override int _fragmentId => Resource.Layout.TaskFragment;

        public Uri ImageUri { get; set; }

        public TaskFragment(IUriHelper uriHelper, IImageHelper imageHelper)
        {
            _imageHelper = imageHelper;
            _uriHelper = uriHelper;
            SaveImage = SaveEncodedImage;
            _multimediaService = new MultimediaService<TaskFragment>(this, _imageView);

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            _linearLayout = view.FindViewById<LinearLayout>(Resource.Id.task_linearlayout);
            _toolbar = view.FindViewById<Toolbar>(Resource.Id.task_toolbar);
            _imageView = view.FindViewById<ImageView>(Resource.Id.image_view);

            _imageView.Click += (sender, e) => { ViewModel?.PickPhotoCommand?.Execute(); };
            _toolbar.Click += (sender, e) => { HideSoftKeyboard(); };

            ((MainActivity)ParentActivity).DrawerLayout.SetDrawerLockMode(DrawerLayout.LockModeLockedClosed);

            return view;
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            SubscribeOnResult?.Invoke(this, new ResultEventArgs(requestCode, resultCode, data));
        }



        private void UnbindDrawables(View view)
        {
            if (view.Background != null)
            {
                view.Background.SetCallback(null);
            }
            if (view is ViewGroup)
            {
                for (int i = 0; i < ((ViewGroup)view).ChildCount; i++)
                {
                    UnbindDrawables(((ViewGroup)view).GetChildAt(i));
                }

                ((ViewGroup)view).RemoveAllViews();
            }
        }

        public void SaveEncodedImage(string encodedImage)
        {
            ViewModel.UserTask.Changes.ImagePath = encodedImage;
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();

            UnbindDrawables(this.View);

            GC.SuppressFinalize(this);
        }
    }
}
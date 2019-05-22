﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using TestProject.Core.ViewModels;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace TestProject.Droid.Fragments
{
    [MvxFragmentPresentation(
        typeof(MainRegistrationViewModel),
        Resource.Id.login_frame, true)]
    public class RegistrationFragment
        : BaseFragment<RegistrationViewModel>
    {
        protected override int _fragmentId => Resource.Layout.RegistrationFragment;


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            _linearLayout = view.FindViewById<LinearLayout>(Resource.Id.registration_linearlayout);

            _toolbar = view.FindViewById<Toolbar>(Resource.Id.registration_toolbar);

            _linearLayout.Click += HideSoftKeyboard;

            _toolbar.Click += HideSoftKeyboard;

            return view;
        }

    
    }
}
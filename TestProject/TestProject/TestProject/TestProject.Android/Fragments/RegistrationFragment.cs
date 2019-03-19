using System;
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

namespace TestProject.Droid.Fragments
{
    [MvxFragmentPresentation(
        typeof(MainRegistrationViewModel),
        Resource.Id.login_frame, true)]
    [Register("testproject.droid.fragments.RegistrationFragment")]
    public class RegistrationFragment
        : BaseFragment<RegistrationViewModel>
    {
        protected override int FragmentId => Resource.Layout.RegistrationFragment;

        private LinearLayout _linearLayout;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            _linearLayout = view.FindViewById<LinearLayout>(Resource.Id.registration_linearlayout);

            _linearLayout.Click += delegate { HideSoftKeyboard(); };


            return view;
        }

        public void HideSoftKeyboard()
        {
            InputMethodManager close = (InputMethodManager)Activity.GetSystemService(Context.InputMethodService);
            close.HideSoftInputFromWindow(_linearLayout.WindowToken, 0);
        }

        public override void OnDestroyView()
        {
            InputMethodManager inputManager = (InputMethodManager)Activity.GetSystemService(Context.InputMethodService);
            var currentFocus = Activity.CurrentFocus;
            inputManager.HideSoftInputFromWindow(currentFocus.WindowToken, 0);
            base.OnDestroyView();
        }
    
    }
}
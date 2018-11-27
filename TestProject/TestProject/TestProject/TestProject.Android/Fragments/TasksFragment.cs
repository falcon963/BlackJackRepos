using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.Views;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using TestProject.Core.ViewModels;
using TestProject.Droid.Views;
using Android.Support.V7.Widget;
using System.ComponentModel;

namespace TestProject.Droid.Fragments
{
    [MvxFragmentPresentation(
        typeof(MainViewModel),
        Resource.Id.content_frame,
        false)]
    [Register("testProject.droid.fragments.TasksFragment")]
    public class TasksFragment
        : BaseFragment<TaskListViewModel>
    {
        protected override int FragmentId => Resource.Layout.TasksFragmentLayout;

        MvxListView listView;
        Android.Support.V7.Widget.Toolbar _toolbar;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            var view = base.OnCreateView(inflater, container, savedInstanceState);

            listView = view.FindViewById<MvxListView>(Resource.Id.task_recycler_view);
            _toolbar = view.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            Activity.FindViewById<Android.Support.V4.Widget.DrawerLayout>(Resource.Id.drawer_layout).CloseDrawers();
            ImageAdapter adapter = new ImageAdapter(this.Activity, (MvxAndroidBindingContext)BindingContext, listView);
            listView.Adapter = adapter;
            ViewModel.ShowMenuCommand.Execute(null);

            return view;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ListOfTasks")
            {
                ImageAdapter adapter = new ImageAdapter(this.Activity, (MvxAndroidBindingContext)BindingContext, listView);
                listView.Adapter = adapter;
            }
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
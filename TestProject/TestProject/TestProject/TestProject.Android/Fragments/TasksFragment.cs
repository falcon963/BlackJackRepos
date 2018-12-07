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
using Android.Support.V7.Widget.Helper;
using TestProject.Droid.Adapter;
using TestProject.Core.Models;

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

        private RecyclerView _recyclerView;
        private RecyclerImageAdapter _imageAdapter;


        Android.Support.V7.Widget.Toolbar _toolbar;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            ViewModel.ListOfTasks.CollectionChanged += ViewModel_CollectionChanged;

            var view = base.OnCreateView(inflater, container, savedInstanceState);


            _recyclerView = view.FindViewById<RecyclerView>(Resource.Id.task_recycler_view);
            _toolbar = view.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);

            Activity.FindViewById<Android.Support.V4.Widget.DrawerLayout>(Resource.Id.drawer_layout).CloseDrawers();

            _imageAdapter = new RecyclerImageAdapter(this);

            SetupRecyclerView();

            ViewModel.ShowMenuCommand.Execute(null);

            return view;
        }

        private void ViewModel_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                _imageAdapter = new RecyclerImageAdapter(this);

                SetupRecyclerView();
            }
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                _imageAdapter = new RecyclerImageAdapter(this);

                SetupRecyclerView();
            }
        }

        public override void OnDestroyView()
        {
            InputMethodManager inputManager = (InputMethodManager)Activity.GetSystemService(Context.InputMethodService);
            var currentFocus = Activity.CurrentFocus;
            inputManager.HideSoftInputFromWindow(currentFocus.WindowToken, 0);
            base.OnDestroyView();
        }

        void OnSwipe(object sender, int position)
        {
            UserTask task = ViewModel.ListOfTasks[position];
            ViewModel.DeleteTaskCommand.Execute(task);
            ViewModel.ListOfTasks.Remove(task);
        }


        void OnItemClick(object sender, int position)
        {
            ViewModel.ItemSelectedCommand.Execute(ViewModel.ListOfTasks[position]);
        }

        private void SetupRecyclerView()
        {
            _recyclerView.SetLayoutManager(new LinearLayoutManager(Context));
       
            var callback = new MyItemTouchHelper(this, _imageAdapter);
            ItemTouchHelper itemTouchHelper = new ItemTouchHelper(callback);
            itemTouchHelper.AttachToRecyclerView(_recyclerView);
            AnimationDecoratorHelper animationDecorator = new AnimationDecoratorHelper();
            _recyclerView.AddItemDecoration(animationDecorator);

           _recyclerView.SetAdapter(_imageAdapter);
        }
    }
}
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
using TestProject.Core.Models;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using TestProject.Droid.Adapters;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using TestProject.Droid.Helpers;
using MvvmCross.Droid.Support.V7.RecyclerView;

namespace TestProject.Droid.Fragments
{
    [MvxFragmentPresentation(
        typeof(MainViewModel),
        Resource.Id.content_frame)]
    public class TasksListFragment
        : BaseFragment<TasksListViewModel>
    {
        protected override int _fragmentId => Resource.Layout.TasksListFragment;

        private MvxRecyclerView _recyclerView;
        private RecyclerImageAdapter _imageAdapter;
        private Toolbar _toolbar;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            ViewModel.Tasks.CollectionChanged += ViewModel_CollectionChanged;

            _showHumburgerMenu = true;

            var view = base.OnCreateView(inflater, container, savedInstanceState);

            _recyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.task_recycler_view);

            _toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar);

            var fabButton = view.FindViewById<FloatingActionButton>(Resource.Id.fab);

            _imageAdapter = new RecyclerImageAdapter(this, (IMvxAndroidBindingContext)this.BindingContext);

            _recyclerView.Adapter = _imageAdapter;

            //var adapter = new TestAdapter((IMvxAndroidBindingContext)this.BindingContext);
            //_recyclerView.Adapter = adapter;

            var set = this.CreateBindingSet<TasksListFragment, TasksListViewModel>();
            set.Bind(_recyclerView).For(x => x.ItemsSource).To(x => x.Tasks);
            set.Apply();


            //set.Bind(_recyclerView).For(v => v.ItemsSource).To(vm => vm.Tasks);
            //set.Apply();


            SetupRecyclerView();

            ((MainActivity)ParentActivity).DrawerLayout.SetDrawerLockMode(DrawerLayout.LockModeUnlocked);

            return view;
        }

        private void ViewModel_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _imageAdapter.Tasks = ViewModel.Tasks.ToList();

            SetupRecyclerView();

            _imageAdapter.NotifyDataSetChanged();

            _recyclerView.Invalidate();
        }

        private void SetupRecyclerView()
        {
            _recyclerView.SetLayoutManager(new LinearLayoutManager(Context));
            _recyclerView.HasFixedSize = true;
            _recyclerView.SetAdapter(_imageAdapter);

            ItemTouchHelper.Callback callback = new MyItemTouchHelper(this, _imageAdapter);
            ItemTouchHelper itemTouchHelper = new ItemTouchHelper(callback);

            itemTouchHelper.AttachToRecyclerView(_recyclerView);

            AnimationDecoratorHelper animationDecorator = new AnimationDecoratorHelper();

            _recyclerView.AddItemDecoration(animationDecorator);
        }
    }
}
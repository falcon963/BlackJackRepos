using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.Views;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using TestProject.Core.ViewModels;

namespace TestProject.Droid.Views
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, true)]
    [Register("testproject.droid.views.TaskFragment")]
    public class TaskFragment : BaseFragment<TaskViewModel>
    {
        protected override int FragmentId => Resource.Layout.TaskFragment;

        private Toolbar _toolbar;

        private Button _buttonSave;
        private Button _buttonDelete;
        private Button _buttonBack;
        private Android.Support.V7.Widget.SwitchCompat @switchStatus;
        private EditText _textTitle;
        private EditText _textNote;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);
            _toolbar = view.FindViewById<Toolbar>(Resource.Id.fragment_toolbar);
            _buttonSave = view.FindViewById<Button>(Resource.Id.button_save);
            _buttonDelete = view.FindViewById<Button>(Resource.Id.button_delete);
            _buttonBack = view.FindViewById<Button>(Resource.Id.fragment_button_menu);
            switchStatus = view.FindViewById<Android.Support.V7.Widget.SwitchCompat>(Resource.Id.switch_status);
            _textTitle = view.FindViewById<EditText>(Resource.Id.text_title);
            _textNote = view.FindViewById<EditText>(Resource.Id.text_note);

            return view;
        }

    }
}
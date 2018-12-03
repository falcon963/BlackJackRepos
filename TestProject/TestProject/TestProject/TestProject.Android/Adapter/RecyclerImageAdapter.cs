using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using DE.Hdodenhof.CircleImageView;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.Views;
using TestProject.Core.Models;
using TestProject.Core.ViewModels;

namespace TestProject.Droid.Adapter
{
    public class RecyclerImageAdapter : RecyclerView.Adapter
    {
        public UserTask[] _tasksList;

        public RecyclerImageAdapter(UserTask[] )
    }
}
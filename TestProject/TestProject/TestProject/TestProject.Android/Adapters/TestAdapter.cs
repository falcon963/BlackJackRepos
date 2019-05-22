using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using TestProject.Core.Models;

namespace TestProject.Droid.Adapters
{
    public class TestAdapter
        : MvxRecyclerAdapter
    {
       public TestAdapter(IMvxAndroidBindingContext bindingContext)
          : base(bindingContext)
       {
       }

       public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        { 
                var itemBindingContext = new MvxAndroidBindingContext(parent.Context, this.BindingContext.LayoutInflaterHolder);
                var view = this.InflateViewForHolder(parent, Resource.Layout.TasksListItem, itemBindingContext);

                return new MyViewHolder(view, itemBindingContext);
        }
    }


    public class MyViewHolder : MvxRecyclerViewHolder
    {
        private readonly TextView Text;
        private readonly CheckBox CheckBox;

        public MyViewHolder(View itemView, IMvxAndroidBindingContext context)
            : base(itemView, context)
        {
            Text = itemView.FindViewById<TextView>(Resource.Id.task_name);
            //CheckBox = itemView.FindViewById<CheckBox>(Resource.Id.list_checkbox);

            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<MyViewHolder, UserTask>();
                set.Bind(this.Text).To(x => x.Title);
                set.Bind(this.CheckBox).To(x => x.Status);
                set.Apply();
            });
        }
    }
}
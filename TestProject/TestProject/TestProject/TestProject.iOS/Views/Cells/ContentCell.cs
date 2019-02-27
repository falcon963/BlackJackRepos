using Foundation;
using MvvmCross.Binding.BindingContext;
using System;
using System.Collections.Generic;
using TestProject.Core.Models;
using UIKit;

namespace TestProject.iOS.Views.Cells
{
    public partial class ContentCell : BaseTableViewCell
    {

        public static readonly NSString Identifier = new NSString("TaskCell");


        public ContentCell(IntPtr handle) : base(handle)
        {
        }


        public void UpdateCell(UserTask item)
        {
            //TaskImage.Image = UIImage.LoadFromData(item.ImagePath);
            //TaskName.Text = item.Title;
            //TaskStatus.On = item.Status;
        }

        protected override void CreateView()
        {
            base.CreateView();

            this.DelayBind(() =>
            {
               // this.AddBindings(TaskName, "Text Title");
                //this.AddBindings(CheckBox, "Checked Status");
            });

        }
    }
}
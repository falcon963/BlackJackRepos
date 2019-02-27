using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using TestProject.Core.Models;
using UIKit;

namespace TestProject.iOS.Views.Cells
{
    public partial class ContentTasksCell : BaseTableViewCell
    {
        public static readonly NSString Key = new NSString("ContentTasksCell");
        public static readonly UINib Nib;

        static ContentTasksCell()
        {
            Nib = UINib.FromName("ContentTasksCell", NSBundle.MainBundle);
        }

        protected ContentTasksCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public void UpdateCell(UserTask item)
        {
            TaskImage.Image = UIImage.LoadFromData(item.ImagePath);
            TaskName.Text = item.Title;
            TaskStatus.On = item.Status;
        }

        //protected override void CreateView()
        //{
        //    base.CreateView();

        //    this.DelayBind(() =>
        //    {
        //        // this.AddBindings(TaskName, "Text Title");
        //        //this.AddBindings(CheckBox, "Checked Status");
        //    });

        //}

    }
}

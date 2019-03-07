using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using TestProject.Core.Models;
using UIKit;

namespace TestProject.iOS.Views.Cells
{
    public partial class ContentTasksCell 
        : BaseTableViewCell
    {

        protected ContentTasksCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public void UpdateCell(UserTask item)
        {
            if (item.ImagePath != null)
            {
                TaskImage.Image = UIImage.LoadFromData(item.ImagePath);
            }
            if(item.ImagePath == null)
            {
                TaskImage.Image = UIImage.FromFile("placeholder.png");
            }
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

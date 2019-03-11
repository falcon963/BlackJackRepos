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
            TaskStatus.Enabled = false;

            TaskImage.Layer.BorderWidth = 1;
            TaskImage.Layer.BackgroundColor = UIColor.Black.CGColor;
            TaskImage.Layer.MasksToBounds = true;
            TaskImage.Layer.CornerRadius = TaskImage.Frame.Size.Width/2;
            TaskImage.ClipsToBounds = true;
        }

    }
}

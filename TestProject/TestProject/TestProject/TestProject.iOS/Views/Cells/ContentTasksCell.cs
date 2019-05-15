using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using TestProject.Core.Models;
using TestProject.iOS.Converters;
using UIKit;

namespace TestProject.iOS.Views.Cells
{
    public partial class ContentTasksCell 
        : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString(nameof(ContentNavigateCell));
        public static readonly UINib Nib;

        static ContentTasksCell()
        {
            Nib = UINib.FromName(Key, NSBundle.MainBundle);
        }

        protected ContentTasksCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<ContentTasksCell, UserTask>();
                set.Bind(TaskName).To(m => m.Title);
                set.Bind(TaskStatus).To(m => m.Status);
                set.Bind(TaskImage).To(vm => vm.ImagePath).WithConversion(new ImageValueConverter());
                set.Apply();
            });
        }

        public void UpdateCell(UserTask item)
        {
            TaskStatus.Enabled = false;

            TaskImage.Layer.BorderWidth = 1;
            TaskImage.Layer.BackgroundColor = UIColor.Black.CGColor;
            TaskImage.Layer.MasksToBounds = true;
            TaskImage.Layer.CornerRadius = TaskImage.Frame.Size.Width / 2;
        }
    }
}

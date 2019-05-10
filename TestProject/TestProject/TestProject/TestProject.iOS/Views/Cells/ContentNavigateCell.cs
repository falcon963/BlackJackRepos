
using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using TestProject.Core.Models;
using UIKit;

namespace TestProject.iOS.Views.Cells
{
    public partial class ContentNavigateCell 
        : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("ContentNavigateCell");
        public static readonly UINib Nib;

        static ContentNavigateCell()
        {
            Nib = UINib.FromName(nameof(ContentNavigateCell), NSBundle.MainBundle);
        }

        protected ContentNavigateCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<ContentNavigateCell, MenuItem>();
                set.Bind(NavigatePageName).To(m => m.ItemTitle);
                set.Apply();
            });
        }

        public void UpdateCell(MenuItem item)
        {
            
        }

    }
}

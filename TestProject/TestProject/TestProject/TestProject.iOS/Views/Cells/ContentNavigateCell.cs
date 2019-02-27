
using System;

using Foundation;
using TestProject.Core.Models;
using UIKit;

namespace TestProject.iOS.Views.Cells
{
    public partial class ContentNavigateCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("ContentNavigateCell");
        public static readonly UINib Nib;

        static ContentNavigateCell()
        {
            Nib = UINib.FromName("ContentNavigateCell", NSBundle.MainBundle);
        }

        protected ContentNavigateCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public void UpdateCell(MenuItem item)
        {
            NavigatePageName.Text = item.ItemTitle;
        }
    }
}

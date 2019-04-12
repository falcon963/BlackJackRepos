using System;

using Foundation;
using UIKit;

namespace TestProject.iOS.Views.Cells
{
    public partial class FileCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("FileCell");
        public static readonly UINib Nib;

        static FileCell()
        {
            Nib = UINib.FromName("FileCell", NSBundle.MainBundle);
        }

        protected FileCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
    }
}

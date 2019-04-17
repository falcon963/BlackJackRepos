using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using UIKit;

namespace TestProject.iOS.CustomControls
{
    [Register("PlaceholderTextView")]
    public class PlaceholderTextView : UITextView
    {
        public string Placeholder { get; set; }

        public PlaceholderTextView()
        {
            Initialize();
        }

        public PlaceholderTextView(CGRect frame)
            : base(frame)
        {
            Initialize();
        }

        public PlaceholderTextView(IntPtr handle)
            : base(handle)
        {
            Initialize();
        }

        void Initialize()
        {
            Placeholder = "Please enter text";

            ShouldBeginEditing = t => {
                if (Text == Placeholder)
                    Text = string.Empty;

                return true;
            };
            ShouldEndEditing = t => {
                if (string.IsNullOrEmpty(Text))
                    Text = Placeholder;

                return true;
            };
        }
    }
}
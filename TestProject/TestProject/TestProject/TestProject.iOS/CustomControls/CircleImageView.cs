using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace TestProject.iOS.CustomControls
{
    public class CircleImageView
        :UIViewController
    {
        public UIImageView _imageView;

        public CircleImageView()
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _imageView.Layer.BorderWidth = 1;
            _imageView.Layer.MasksToBounds = false;
            _imageView.Layer.BorderColor = UIColor.Black.CGColor;
            _imageView.Layer.CornerRadius = _imageView.Frame.Width / 2;
            _imageView.ClipsToBounds = true;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;

namespace TestProject.iOS.Loader
{
    public class LoadingOverlay : UIView
    {
        private CABasicAnimation rotationAnimation;
        private UIImageView spinner;

        public LoadingOverlay(CGSize screen)
        {

            BackgroundColor = UIColor.Gray;
            Alpha = 0.5f;
            Frame = new CGRect(0, 0, screen.Width, screen.Height);
            var loaderImage = UIImage.FromBundle("loaderSet");
            spinner = new UIImageView
            {
                Frame = new CGRect(this.Center, new CGSize(36, 36)),
                Image = loaderImage
            };
            spinner.Center = this.Center;

            AddSubview(spinner);
            RotateAnimation();
        }

        #region AnimateSpinner

        public void RotateAnimation()
        {
            rotationAnimation = new CABasicAnimation
            {
                KeyPath = "transform.rotation.z",
                To = new NSNumber(Math.PI * 2),
                Duration = 0.7,
                Cumulative = true,
                RepeatCount = float.MaxValue
            };
            spinner.Layer.AddAnimation(rotationAnimation, "rotationAnimation");
        }

        #endregion
    }
}
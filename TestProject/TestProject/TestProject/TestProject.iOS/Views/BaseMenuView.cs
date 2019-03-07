using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.ViewModels;
using UIKit;

namespace TestProject.iOS.Views
{
    public abstract class BaseMenuView<TParam>
        :MvxViewController<TParam> where TParam : MvxViewModel
    {
        public virtual UIScrollView ScrollView { get; set; }

        protected UIWindow Window
        {
            get
            {
                return (UIApplication.SharedApplication.Delegate as AppDelegate).Window;
            }
            set
            {
                (UIApplication.SharedApplication.Delegate as AppDelegate).Window = value;
            }
        }

        public BaseMenuView(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
        }

        public virtual void HandleKeyboardDidShow(NSNotification obj)
        {
            ScrollView.ContentSize = new CoreGraphics.CGSize(View.Frame.Width, View.Frame.Height + UIKeyboard.FrameBeginFromNotification(obj).Height / 2);
        }

        public virtual void HandleKeyboardDidHide(NSNotification obj)
        {
            ScrollView.ContentSize = new CoreGraphics.CGSize(View.Frame.Width, View.Frame.Height - UIKeyboard.FrameBeginFromNotification(obj).Height / 2);
        }

        public void HideKeyboard(UITapGestureRecognizer tap)
        {
            tap = new UITapGestureRecognizer();
            tap.AddTarget(() =>
            {
                View.EndEditing(true);
            });
            tap.CancelsTouchesInView = false;
            View.AddGestureRecognizer(tap);
        }

        public void AddShadow(UIView view)
        {
            view.Layer.MasksToBounds = false;
            view.Layer.ShadowRadius = 3;
            view.Layer.ShadowColor = UIColor.Black.CGColor;
            view.Layer.ShadowOffset = new CGSize(1.0, 1.0);
            view.Layer.ShadowOpacity = 1;
        }
    }
}
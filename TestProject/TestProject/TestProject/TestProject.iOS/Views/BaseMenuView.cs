using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreAnimation;
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

        private CGSize SizeScroll { get; set; }

        private Boolean _scrollEnable;

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
            //if(ScrollView.ContentSize.Height > ScrollView.VisibleSize.Height)
            //{
            //    SizeScroll = ScrollView.ContentSize;
            //    _scrollEnable = true;
            //}
            SizeScroll = ScrollView.ContentSize;
            ScrollView.ContentSize = new CoreGraphics.CGSize(View.Frame.Width, View.Frame.Height + UIKeyboard.FrameBeginFromNotification(obj).Height / 2);
        }

        public virtual void HandleKeyboardDidHide(NSNotification obj)
        {
            //if (_scrollEnable)
            //{
            //    ScrollView.ContentSize = SizeScroll;
            //}
            //if (!_scrollEnable)
            //{
            //ScrollView.ContentSize = new CoreGraphics.CGSize(View.Frame.Width, View.Frame.Height - UIKeyboard.FrameBeginFromNotification(obj).Height / 2);
            //}
            ScrollView.ContentSize = SizeScroll;
            //_scrollEnable = false;
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

        public void AddShadowTextField(UITextField field)
        {
            field.BorderStyle = UITextBorderStyle.None;
            field.BackgroundColor = UIColor.GroupTableViewBackgroundColor;

            //field.Layer.CornerRadius = field.Frame.Size.Height / 2;
            field.Layer.BorderWidth = 0.25f;
            field.Layer.BorderColor = UIColor.White.CGColor;

            field.Layer.ShadowOpacity = 1f;
            field.Layer.ShadowRadius = 3f;
            field.Layer.BorderColor = UIColor.Black.ColorWithAlpha(0.25f).CGColor;
            field.Layer.ShadowOffset = new CGSize (0f, 3f);
            field.Layer.ShadowColor = UIColor.Black.CGColor;
        }

        public void CreateShadow(UITextField field)
        {
            var height = field.Frame.Size.Height;
            var width = field.Frame.Size.Width;
            Single shadowOffsetX = 2000;
            var shadowPath = new UIBezierPath();
            shadowPath.MoveTo(new CGPoint(0, height));
            shadowPath.AddLineTo(new CGPoint(width, height));
            shadowPath.AddLineTo(new CGPoint(width + shadowOffsetX, 2000));
            shadowPath.AddLineTo(new CGPoint(shadowOffsetX, 2000));
            field.Layer.ShadowPath = shadowPath.CGPath;

            field.Layer.ShadowRadius = 0;
            field.Layer.ShadowOffset = CGSize.Empty;
            field.Layer.ShadowOpacity = 0.2f;

            //Single shadowOffsetX = 100;
            //var shadowPath = new UIBezierPath();
            //shadowPath.MoveTo(new CGPoint(0, height));
            //shadowPath.AddLineTo(new CGPoint(width, height));

            //shadowPath.AddLineTo(new CGPoint(width + shadowOffsetX, 100));
            //shadowPath.AddLineTo(new CGPoint(shadowOffsetX, 100));
            field.Layer.ShadowPath = shadowPath.CGPath;
        }
    }
}
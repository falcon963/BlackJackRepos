using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.ViewModels;
using TestProject.Core.ViewModels;
using TestProject.iOS.Loader;
using TestProject.iOS.Views.Interfaces;
using UIKit;

namespace TestProject.iOS.Views
{
    public abstract class BaseView
        : MvxViewController
    {
        protected virtual UIScrollView ScrollView { get; set; }

        private CGSize SizeScroll { get; set; }

        private bool _scrollEnable;

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

        public AppDelegate ThisApp
        {
            get { return (AppDelegate)UIApplication.SharedApplication.Delegate; }
        }

        public BaseView(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
        }



        public virtual void HandleKeyboardDidShow(NSNotification obj)
        {

            SizeScroll = ScrollView.ContentSize;
            ScrollView.ContentSize = new CGSize(View.Frame.Width, View.Frame.Height + UIKeyboard.FrameBeginFromNotification(obj).Height / 2);
        }

        public virtual void HandleKeyboardDidHide(NSNotification obj)
        {

            ScrollView.ContentSize = SizeScroll;
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
            float shadowOffsetX = 2000;
            var shadowPath = new UIBezierPath();
            shadowPath.MoveTo(new CGPoint(0, height));
            shadowPath.AddLineTo(new CGPoint(width, height));
            shadowPath.AddLineTo(new CGPoint(width + shadowOffsetX, 2000));
            shadowPath.AddLineTo(new CGPoint(shadowOffsetX, 2000));
            field.Layer.ShadowPath = shadowPath.CGPath;

            field.Layer.ShadowRadius = 0;
            field.Layer.ShadowOffset = CGSize.Empty;
            field.Layer.ShadowOpacity = 0.2f;

            field.Layer.ShadowPath = shadowPath.CGPath;
        }

        public void ShadowCreate(UIView inputView, UIView shadowView)
        {
            var shadowOffsetX = inputView.Bounds.Height;
            if (inputView.Bounds.Height > 30)
            {
                shadowOffsetX = 30;
            }

            shadowView.TranslatesAutoresizingMaskIntoConstraints = false;

            var shadowPath = new UIBezierPath();

            var frame = inputView.Bounds;
            var frame1 = inputView.Frame.Size;

            shadowPath.MoveTo(new CGPoint(frame.GetMinX(), frame.GetMinY()));
            shadowPath.AddLineTo(new CGPoint(frame.GetMaxX() * 0.77, frame.GetMinY()));
            shadowPath.AddLineTo(new CGPoint(frame.GetMaxX() * 0.77 + shadowOffsetX, frame.GetMaxY()));
            shadowPath.AddLineTo(new CGPoint(frame.GetMinX() + shadowOffsetX, frame.GetMaxY()));
            shadowPath.ClosePath();
            shadowPath.Fill();


            var shadowLayer = new CAShapeLayer();
            shadowLayer.Frame = new CGRect(0, 0, shadowView.Frame.Width, shadowView.Frame.Height);
            shadowLayer.Path = shadowPath.CGPath;
            shadowLayer.FillRule = CAShapeLayer.FillRuleEvenOdd;

            shadowView.Layer.MasksToBounds = false;
            var shadowImage = new UIImageView();
            shadowImage.Image = UIImage.FromBundle("shadow");
            shadowImage.Frame = new CGRect(0, 0, inputView.Frame.Width, inputView.Frame.Height);
            shadowImage.Bounds = shadowView.Bounds;
            shadowView.AddSubview(shadowImage);
            shadowView.Layer.Mask = shadowLayer;
        }
    }

    public class BaseView<TView, TViewModel> : MvxViewController<TViewModel>, IBaseView, IMvxBindingContextOwner where TViewModel 
          : class, IMvxViewModel, IMvxNotifyPropertyChanged where TView : MvxViewController, IBaseView, new()
    {

        public nfloat KeyboardHeight { get; set; }
        protected LoadingOverlay LoaderOverlay;
        protected BaseViewModel BaseViewModel => ViewModel as BaseViewModel;
        protected MvxFluentBindingDescriptionSet<TView, TViewModel> BindingSet;
        protected UITapGestureRecognizer hideKeyboardRecognizer;

        protected virtual UIScrollView ScrollView { get; set; }

        private CGSize SizeScroll { get; set; }

        public virtual void HandleKeyboardDidShow(NSNotification obj)
        {
            SizeScroll = ScrollView.ContentSize;
            ScrollView.ContentSize = new CGSize(View.Frame.Width, View.Frame.Height + (obj.UserInfo[UIKeyboard.FrameEndUserInfoKey] as NSValue).CGRectValue.Height/2);
        }

        public virtual void HandleKeyboardDidHide(NSNotification obj)
        {

            ScrollView.ContentSize = SizeScroll;
        }

        public AppDelegate ThisApp
        {
            get { return (AppDelegate)UIApplication.SharedApplication.Delegate; }
        }

        protected UIWindow Window
        {
            get
            {
                return UIApplication.SharedApplication.KeyWindow; ;
            }
            set
            {
                (UIApplication.SharedApplication.Delegate as AppDelegate).Window = value;
            }
        }

        #region Lifecycle

        public BaseView(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
            try
            {
                LoaderOverlay = new LoadingOverlay(UIScreen.MainScreen.Bounds.Size)
                {
                    Hidden = true
                };

                HideKeyboard();

                View.AddSubview(LoaderOverlay);
            }
            catch (Exception ex)
            {
                ;
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            CreateBindnigSet();
            SetupBindings();
            SetupEvents();
            CustomizeViews();
        }

        public void HideKeyboard()
        {
            var tap = new UITapGestureRecognizer();
            tap.AddTarget(() =>
            {
                View.EndEditing(true);
            });
            tap.CancelsTouchesInView = false;
            View.AddGestureRecognizer(tap);
        }

        public override void ViewDidLayoutSubviews() => base.ViewDidLayoutSubviews();

        public override void ViewWillAppear(bool animated)
        {

            try
            {
                base.ViewWillAppear(animated);

            }
            catch (Exception)
            {
                ;
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            try
            {
                base.ViewWillDisappear(animated);

            }
            catch (Exception)
            {
                ;
            }
        }

        public override void ViewWillUnload()
        {
            base.ViewWillUnload();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);


            LoaderOverlay.RotateAnimation();
            View.BringSubviewToFront(LoaderOverlay);
        }
        #endregion

        protected bool CreateBindnigSet()
        {
            BindingSet = (this as TView).CreateBindingSet<TView, TViewModel>();

            return true;
        }

        public virtual bool SetupBindings()
        {
            BindingSet.Apply();
            return true;
        }

        public virtual bool SetupEvents()
        {
            return true;
        }

        public virtual bool CustomizeViews()
        {
            return true;
        }

        #region Events

        public override void TouchesBegan(NSSet touches, UIEvent evt) => View.EndEditing(true);

        public override void TouchesEnded(NSSet touches, UIEvent evt) => View.EndEditing(true);

        protected virtual void OnKeyboardWillShow(NSNotification notification)
        {
            KeyboardHeight = (notification.UserInfo[UIKeyboard.FrameEndUserInfoKey] as NSValue).CGRectValue.Height;
            var duration = (notification.UserInfo[UIKeyboard.AnimationDurationUserInfoKey] as NSNumber).DoubleValue;
            UIView.Animate(duration, () => { View.LayoutIfNeeded(); });
        }

        protected virtual void OnKeyboardWillHide(NSNotification notification)
        {
            var duration = (notification.UserInfo[UIKeyboard.AnimationDurationUserInfoKey] as NSNumber).DoubleValue;
            UIView.Animate(duration, () => { View.LayoutIfNeeded(); });
        }

        private void OnScrollDismissKeyboard() => View.EndEditing(true);

        #endregion

        #region shadowCreate
        public void AddShadow(UIView view)
        {
            view.Layer.MasksToBounds = false;
            view.Layer.ShadowRadius = 3;
            view.Layer.ShadowColor = UIColor.Black.CGColor;
            view.Layer.ShadowOffset = new CGSize(1.0, 1.0);
            view.Layer.ShadowOpacity = 1;
        }

        public void ShadowCreate(UIView inputView, UIView shadowView)
        {
            var shadowOffsetX = inputView.Bounds.Height;
            if (inputView.Bounds.Height > 30)
            {
                shadowOffsetX = 30;
            }

            shadowView.TranslatesAutoresizingMaskIntoConstraints = false;

            var shadowPath = new UIBezierPath();

            var frame = inputView.Bounds;
            var frame1 = inputView.Frame.Size;

            shadowPath.MoveTo(new CGPoint(frame.GetMinX(), frame.GetMinY()));
            shadowPath.AddLineTo(new CGPoint(frame.GetMaxX() * 0.77, frame.GetMinY()));
            shadowPath.AddLineTo(new CGPoint(frame.GetMaxX() * 0.77 + shadowOffsetX, frame.GetMaxY()));
            shadowPath.AddLineTo(new CGPoint(frame.GetMinX() + shadowOffsetX, frame.GetMaxY()));
            shadowPath.ClosePath();
            shadowPath.Fill();


            var shadowLayer = new CAShapeLayer();
            shadowLayer.Frame = new CGRect(0, 0, shadowView.Frame.Width, shadowView.Frame.Height);
            shadowLayer.Path = shadowPath.CGPath;
            shadowLayer.FillRule = CAShapeLayer.FillRuleEvenOdd;

            shadowView.Layer.MasksToBounds = false;
            var shadowImage = new UIImageView();
            shadowImage.Image = UIImage.FromBundle("shadow");
            shadowImage.Frame = new CGRect(0, 0, inputView.Frame.Width, inputView.Frame.Height);
            shadowImage.Bounds = shadowView.Bounds;
            shadowView.AddSubview(shadowImage);
            shadowView.Layer.Mask = shadowLayer;
        }
        #endregion
    }
}
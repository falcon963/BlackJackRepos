using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Views;
using System;
using TestProject.Core.ViewModels;
using UIKit;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using System.Drawing;
using CoreGraphics;
using TestProject.iOS.Converters;
using Xamarin.Auth;
using System.Json;
using TestProject.Core.Interfaces.SocialService.Facebook;
using TestProject.Core.Interfaces.SocialService.Google;
using TestProject.Core.Authentication;
using TestProject.Core.Constant;
using TestProject.Core.Models;
using SafariServices;
using CoreAnimation;

namespace TestProject.iOS.Views
{
    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class LoginView
        : BaseMenuView<LoginViewModel>, IFacebookAuthentication, IGoogleAuthenticationDelegate
    {
        private UITapGestureRecognizer _tap;

        private FacebookAuthenticator _authFaceBook;

        public static GoogleAuthenticator GoogleAuth { get; private set; } = null;

        private CAShapeLayer _shadowLayer;

        public override UIScrollView ScrollView { get => base.ScrollView; set => base.ScrollView = value; }

        public LoginView() : base("LoginView", null)
        {
            HideKeyboard(_tap);
        }



        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            InitializeGoogleAuth();
            InitializeFacebokAuth();

            ScrollView = LoginScrollView;

            UITapGestureRecognizer googleButton = new UITapGestureRecognizer(GoogleLogin);
            UITapGestureRecognizer facebookButton = new UITapGestureRecognizer(FacebookLogin);
            LoginGoogleButton.AddGestureRecognizer(googleButton);
            LoginFacebookButton.AddGestureRecognizer(facebookButton);

            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidHideNotification, HandleKeyboardDidHide);

            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, HandleKeyboardDidShow);

            var set = this.CreateBindingSet<LoginView, LoginViewModel>();
            set.Bind(RegistrationButton).To(vm => vm.GoRegistrationPageCommand);
            set.Bind(LoginButton).To(vm => vm.LoginCommand);
            set.Bind(LoginField).To(vm => vm.Login);
            set.Bind(PasswordField).To(vm => vm.Password);
            set.Bind(RememberSwitch).To(vm => vm.RememberMeStatus);
            set.Bind(LoginScrollView).For(v => v.BackgroundColor).To(vm => vm.LoginColor).WithConversion(new ColorValueConverter());
            set.Apply();

            var shadowOffsetX = 30;

            var shadowPath = new UIBezierPath();

            var frame = LoginField.Bounds;

            shadowPath.MoveTo(new CGPoint(frame.GetMinX(), frame.GetMinY()));
            shadowPath.AddLineTo(new CGPoint(260, frame.GetMinY()));
            shadowPath.AddLineTo(new CGPoint(260 + shadowOffsetX, frame.GetMaxY()));
            shadowPath.AddLineTo(new CGPoint(frame.GetMinX() + shadowOffsetX, frame.GetMaxY()));
            shadowPath.ClosePath();
            shadowPath.Fill();

            //shadowPath.MoveTo(new CGPoint(frame.Location.X, 0));
            //shadowPath.AddLineTo(new CGPoint(430, 0));
            //shadowPath.AddLineTo(new CGPoint(430 + 40, 40));
            //shadowPath.AddLineTo(new CGPoint(frame.Location.X + 40, 40));
            //shadowPath.ClosePath();
            //shadowPath.Fill();

            _shadowLayer = new CAShapeLayer();
            _shadowLayer.Frame = new CGRect(LoginField.Frame.GetMinX(), 0, LoginField.Frame.Width, LoginField.Frame.Height);
            _shadowLayer.Bounds = LoginField.Bounds;
            _shadowLayer.Path = shadowPath.CGPath;
            ShadowView.Layer.Mask = _shadowLayer;

            var shadowGradColor = new CAGradientLayer();
            shadowGradColor.Frame = new CGRect(ShadowView.Frame.GetMinX(), 0, ShadowView.Frame.Width, ShadowView.Frame.Height);
            shadowGradColor.Colors = new[] { UIColor.Black.CGColor, View.BackgroundColor.CGColor };
            shadowGradColor.Locations = new NSNumber[] { 0, 1 };
            shadowGradColor.StartPoint = new CGPoint(0.6, 0);
            shadowGradColor.EndPoint = new CGPoint(1, 1);

            ShadowView.Layer.AddSublayer(shadowGradColor);
            ShadowView.Layer.MasksToBounds = true;
            ShadowView.Layer.MaskedCorners = CACornerMask.MaxXMaxYCorner;
            ShadowView.Layer.MaskedCorners = CACornerMask.MaxXMinYCorner;
            ShadowView.Layer.MaskedCorners = CACornerMask.MinXMaxYCorner;
            ShadowView.Layer.MaskedCorners = CACornerMask.MinXMinYCorner;

            CALayer layer = ShadowView.Layer;
            layer.ShadowOpacity = 5f;
            layer.ShadowOffset = new CGSize(0, 0);
            layer.ShadowRadius = 10f;
            layer.ShadowColor = UIColor.Black.CGColor;
            //ShadowView.BackgroundColor = UIColor.Black;










            //CreateShadow(LoginField);
            AddShadowTextField(PasswordField);
            AddShadow(LoginButton);
            AddShadow(RegistrationButton);
            AddShadow(RememberSwitch);
            AddShadow(LoginFacebookButton);
            AddShadow(LoginGoogleButton);
        }

        void FacebookLogin()
        {
            var authenticator = _authFaceBook.GetAuthenticator();
            var view = authenticator.GetUI();
            PresentViewController(view, true, () => { });
        }

        void GoogleLogin()
        {
            var authenticator = GoogleAuth.GetAuthenticator();
            var viewController = authenticator.GetUI();
            PresentViewController(viewController, true, null);
        }

        public override void HandleKeyboardDidHide(NSNotification obj)
        {
            base.HandleKeyboardDidHide(obj);
        }

        public override void HandleKeyboardDidShow(NSNotification obj)
        {
            base.HandleKeyboardDidShow(obj);
        }

        //public void CreateShadow()
        //{
        //    var shadowOffsetX = LoginField.Frame.Height;
        //    var width = LoginField.Frame.Width;
        //    var height = LoginField.Frame.Height;
        //    var path = new UIBezierPath();
        //    path.MoveTo(new CGPoint(0, LoginField.Frame.Height));
        //    path.AddLineTo(new CGPoint(width, 0));
        //    path.AddLineTo(new CGPoint(width + shadowOffsetX, height));
        //    path.AddLineTo(new CGPoint(shadowOffsetX, 50));
        //    path.AddLineTo(new CGPoint(0, 0));
        //    path.ClosePath();
        //}

        #region SocialMedia

        private void InitializeFacebokAuth() => _authFaceBook = new FacebookAuthenticator(SocialConstant.ClientIdiOSFacebook, SocialConstant.Scope, this);
        private void InitializeGoogleAuth()
        {
            var googleServiceDictionary = NSDictionary.FromFile("credentials.plist");
            //var clientId = googleServiceDictionary["CLIENT_ID"].ToString();
            //var redirect = googleServiceDictionary["REVERSED_CLIENT_ID"].ToString();
            GoogleAuth = new GoogleAuthenticator("70862177039-jm46ae5e77822hk8qllegch1fqler0a4.apps.googleusercontent.com", "email", new Uri("com.googleusercontent.apps.70862177039-jm46ae5e77822hk8qllegch1fqler0a4:/oauth2redirect"), this);
        }

        void IGoogleAuthenticationDelegate.OnAuthenticationCompleted(String token)
        {
            ViewModel.SaveGoogleUserCommand.Execute();
            DismissViewController(true, null);
        }
        void IGoogleAuthenticationDelegate.OnAuthenticationFailed(string message, Exception exception)
        {
            DismissViewController(true, null);
        }
        void IGoogleAuthenticationDelegate.OnAuthenticationCanceled()
        {
            //DismissViewController(true, null);
        }

        void IFacebookAuthentication.OnAuthenticationCompleted(String token)
        {
            ViewModel.SaveFacebookUserCommand.Execute();
            DismissViewController(true, null);
        }
        void IFacebookAuthentication.OnAuthenticationFailed(string message, Exception exception)
        {
            DismissViewController(true, null);
        }
        void IFacebookAuthentication.OnAuthenticationCanceled()
        {
            //DismissViewController(true, null);
            InitializeFacebokAuth();
        }
        #endregion
    }
}

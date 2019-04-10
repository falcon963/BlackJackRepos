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


            ShadowCreate(LoginField, ShadowView);
            ShadowCreate(PasswordField, ShadowViewPasswordField);

            AddShadow(LoginButton);
            AddShadow(RegistrationButton);
            AddShadow(RememberSwitch);
            AddShadow(LoginFacebookButton);
            AddShadow(LoginGoogleButton);
        }

        void ShadowCreate(UIView inputView, UIView shadowView)
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
            shadowPath.AddLineTo(new CGPoint(frame.GetMaxX()*0.77, frame.GetMinY()));
            shadowPath.AddLineTo(new CGPoint(frame.GetMaxX()*0.77 + shadowOffsetX, frame.GetMaxY()));
            shadowPath.AddLineTo(new CGPoint(frame.GetMinX() + shadowOffsetX, frame.GetMaxY()));
            shadowPath.ClosePath();
            shadowPath.Fill();


            _shadowLayer = new CAShapeLayer();
            _shadowLayer.Frame = new CGRect(shadowView.Frame.X, 0, shadowView.Frame.Width, shadowView.Frame.Height); ;
            _shadowLayer.Path = shadowPath.CGPath;
            _shadowLayer.FillRule = CAShapeLayer.FillRuleEvenOdd;

            //var shadowGradColor = new CAGradientLayer();
            //shadowGradColor.Frame = new CGRect(shadowView.Frame.GetMinX(), 0, shadowView.Frame.Width, shadowView.Frame.Height);
            //shadowGradColor.Colors = new[] { UIColor.Black.CGColor, View.BackgroundColor.CGColor };
            //shadowGradColor.Locations = new NSNumber[] { 0, 1 };
            //shadowGradColor.StartPoint = new CGPoint(0.6, 0);
            //shadowGradColor.EndPoint = new CGPoint(1, 1);

            //ShadowView.Layer.AddSublayer(shadowGradColor);

            shadowView.Layer.MasksToBounds = false;
            var shadowImage = new UIImageView();
            shadowImage.Image = UIImage.FromFile("shadow_backinput_1444.png");
            shadowImage.Frame = new CGRect(inputView.Frame.GetMinX(), 0, inputView.Frame.Width, inputView.Frame.Height);
            shadowImage.Bounds = shadowView.Bounds;
            shadowView.AddSubview(shadowImage);
            shadowView.Layer.Mask = _shadowLayer;
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
        void IGoogleAuthenticationDelegate.OnAuthenticationFailed(String message, Exception exception)
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
        void IFacebookAuthentication.OnAuthenticationFailed(String message, Exception exception)
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

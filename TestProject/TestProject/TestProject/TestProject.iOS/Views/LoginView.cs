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
using TestProject.Core.Authentication;
using TestProject.Core.Models;
using SafariServices;
using CoreAnimation;
using TestProject.Core.Authentication.Interfaces;
using TestProject.Core.Constants;

namespace TestProject.iOS.Views
{
    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class LoginView
        : BaseMenuView<LoginViewModel>, IFacebookAuthentication, IGoogleAuthenticationDelegate
    {

        #region Fields

        private UITapGestureRecognizer _tap;

        private FacebookAuthenticator _authFaceBook;

        #endregion

        #region Propertys

        public static GoogleAuthenticator GoogleAuth { get; private set; } = null;

        public override UIScrollView ScrollView { get => base.ScrollView; set => base.ScrollView = value; }

        #endregion

        #region ctor

        public LoginView() : base("LoginView", null)
        {
            HideKeyboard(_tap);
        }

        #endregion

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            InitializeFacebokAuth();
            InitializeGoogleAuth();

            ScrollView = LoginScrollView;

            UITapGestureRecognizer facebookButton = new UITapGestureRecognizer(FacebookLogin);
            UITapGestureRecognizer googleButton = new UITapGestureRecognizer(GoogleLogin);

            LoginFacebookButton.AddGestureRecognizer(facebookButton);
            LoginGoogleButton.AddGestureRecognizer(googleButton);

            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidHideNotification, HandleKeyboardDidHide);

            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, HandleKeyboardDidShow);

            var set = this.CreateBindingSet<LoginView, LoginViewModel>();
            set.Bind(LoginScrollView).For(v => v.BackgroundColor).To(vm => vm.LoginColor).WithConversion(new ColorValueConverter());
            set.Bind(RegistrationButton).To(vm => vm.GoRegistrationPageCommand);
            set.Bind(RememberSwitch).To(vm => vm.IsRememberMeStatus);
            set.Bind(LoginButton).To(vm => vm.LoginCommand);
            set.Bind(PasswordField).To(vm => vm.Password);
            set.Bind(LoginField).To(vm => vm.Login);
            set.Apply();

            ShadowCreate(PasswordField, ShadowViewPasswordField);
            ShadowCreate(LoginField, ShadowView);

            AddShadow(LoginFacebookButton);
            AddShadow(RegistrationButton);
            AddShadow(LoginGoogleButton);
            AddShadow(RememberSwitch);
            AddShadow(LoginButton);
        }

        #region Login

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

        #endregion

        #region Keyboard

        public override void HandleKeyboardDidHide(NSNotification obj)
        {
            base.HandleKeyboardDidHide(obj);
        }

        public override void HandleKeyboardDidShow(NSNotification obj)
        {
            base.HandleKeyboardDidShow(obj);
        }

        #endregion

        #region SocialMedia

        private void InitializeFacebokAuth() => _authFaceBook = new FacebookAuthenticator(SocialConstants.ClientIdiOSFacebook, SocialConstants.Scope, this);
        private void InitializeGoogleAuth()
        {
            GoogleAuth = new GoogleAuthenticator("70862177039-jm46ae5e77822hk8qllegch1fqler0a4.apps.googleusercontent.com", "email",
                new Uri("com.googleusercontent.apps.70862177039-jm46ae5e77822hk8qllegch1fqler0a4:/oauth2redirect"), this);
        }

        void IGoogleAuthenticationDelegate.OnAuthenticationCompleted(String token)
        {
            ViewModel.SignInWithFacebookCommand.Execute();
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
            ViewModel.SignInWithFacebookCommand.Execute();
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

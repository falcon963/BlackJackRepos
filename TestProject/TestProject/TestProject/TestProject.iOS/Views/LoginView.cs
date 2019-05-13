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
using TestProject.iOS.Services;
using TestProject.iOS.Models;

namespace TestProject.iOS.Views
{
    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class LoginView
        : BaseView<LoginView ,LoginViewModel>
    {

        #region Fields

        private UITapGestureRecognizer _tap;

        private FacebookAuthenticator _authFaceBook;

        private AuthenticationService _authenticationService;

        #endregion

        #region Propertys

        public static GoogleAuthenticator GoogleAuth { get; private set; } = null;

        #endregion

        #region ctor

        public LoginView() : base("LoginView", null)
        {
            var authenticationModel = new AuthenticationModel
            {
                DissmisController = () => DismissViewController(true, null),
                FacebookInitialize = () => InitializeFacebokAuth(),
                GoogleInitialize = () => InitializeGoogleAuth(),
                FacebookOnAuthenticationCompleted = () => ViewModel.SignInWithFacebookCommand.Execute(),
                GoogleOnAuthenticationCompleted = () => ViewModel.SignInWithGoogleCommand.Execute()
            };
            _authenticationService = new AuthenticationService(authenticationModel);
        }

        #endregion

        public override bool SetupBindings()
        {
            BindingSet.Bind(LoginScrollView).For(v => v.BackgroundColor).To(vm => vm.LoginColor).WithConversion("NativeColor");
            BindingSet.Bind(LoginScrollView).For(v => v.BackgroundColor).To(vm => vm.LoginColor).WithConversion("NativeColor"); ;
            BindingSet.Bind(RegistrationButton).To(vm => vm.GoRegistrationPageCommand);
            BindingSet.Bind(RememberSwitch).To(vm => vm.IsRememberMeStatus);
            BindingSet.Bind(LoginButton).To(vm => vm.LoginCommand);
            BindingSet.Bind(PasswordField).To(vm => vm.Password);
            BindingSet.Bind(LoginField).To(vm => vm.Login);

            return base.SetupBindings();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            InitializeFacebokAuth();
            InitializeGoogleAuth();

            UITapGestureRecognizer facebookButton = new UITapGestureRecognizer(FacebookLogin);
            UITapGestureRecognizer googleButton = new UITapGestureRecognizer(GoogleLogin);

            LoginFacebookButton.AddGestureRecognizer(facebookButton);
            LoginGoogleButton.AddGestureRecognizer(googleButton);

            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidHideNotification, OnKeyboardWillHide);

            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, OnKeyboardWillShow);

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

        #region SocialMedia

        private void InitializeFacebokAuth() => _authFaceBook = new FacebookAuthenticator(SocialConstants.ClientIdiOSFacebook, SocialConstants.Scope, _authenticationService);
        private void InitializeGoogleAuth()
        {
            GoogleAuth = new GoogleAuthenticator("70862177039-jm46ae5e77822hk8qllegch1fqler0a4.apps.googleusercontent.com", "email",
                new Uri("com.googleusercontent.apps.70862177039-jm46ae5e77822hk8qllegch1fqler0a4:/oauth2redirect"), _authenticationService);
        }

        #endregion
    }
}

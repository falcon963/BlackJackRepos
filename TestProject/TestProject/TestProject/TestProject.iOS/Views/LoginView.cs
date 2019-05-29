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
using TestProject.iOS.Services.Interfaces;
using MvvmCross;
using ObjCRuntime;

namespace TestProject.iOS.Views
{
    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class LoginView
        : BaseView<LoginView ,LoginViewModel>
    {

        #region Fields

        private IAuthenticationGoogleService _googleAuthenticationService;

        private IAuthenticationFacebookService _facebookAuthenticationService;

        #endregion

        #region ctor

        public LoginView() : base(nameof(LoginView), null)
        {
        }

        #endregion

        public override bool SetupEvents()
        {
            _googleAuthenticationService.OnAuthenticationCompleted += OnGoogleAuthenticationServiceAuthenticationCompleted;

            _googleAuthenticationService.OnAuthenticationFailed += OnAuthenticationServiceAuthenticationFailed;

            _facebookAuthenticationService.OnAuthenticationCompleted += OnFacebookAuthenticationServiceAuthenticationCompleted;

            _facebookAuthenticationService.OnAuthenticationFailed += OnAuthenticationServiceAuthenticationFailed;

            return base.SetupEvents();
        }

        #region Handlers

        private void OnAuthenticationServiceAuthenticationFailed(object sender, EventArgs e)
        {
            DismissViewController(true, null);
        }

        private void OnFacebookAuthenticationServiceAuthenticationCompleted(object sender, EventArgs e)
        {
            Unsubscribe();
            DismissViewController(true, null);
            ViewModel.SignInWithFacebookCommand.Execute();
        }

        private void OnGoogleAuthenticationServiceAuthenticationCompleted(object sender, EventArgs e)
        {
            Unsubscribe();
            DismissViewController(true, null);
            ViewModel?.SignInWithGoogleCommand?.Execute();
        }

        #endregion

        public override bool SetupBindings()
        {
            BindingSet.Bind(LoginScrollView).For(v => v.BackgroundColor).To(vm => vm.LoginColor).WithConversion("NativeColor");
            BindingSet.Bind(RegistrationButton).To(vm => vm.GoRegistrationPageCommand);
            BindingSet.Bind(RememberSwitch).To(vm => vm.IsRememberMeStatus);
            BindingSet.Bind(LoginButton).To(vm => vm.LoginCommand);
            BindingSet.Bind(PasswordField).To(vm => vm.Password);
            BindingSet.Bind(LoginField).To(vm => vm.Login);

            return base.SetupBindings();
        }

        public override void ViewDidLoad()
        {
            ScrollView = LoginScrollView;

            _googleAuthenticationService = Mvx.IoCProvider.Resolve<IAuthenticationGoogleService>();
            _facebookAuthenticationService = Mvx.IoCProvider.Resolve<IAuthenticationFacebookService>();

            UITapGestureRecognizer facebookButton = new UITapGestureRecognizer(FacebookLogin);
            UITapGestureRecognizer googleButton = new UITapGestureRecognizer(GoogleLogin);

            LoginFacebookButton.AddGestureRecognizer(facebookButton);
            LoginGoogleButton.AddGestureRecognizer(googleButton);


            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidHideNotification, HandleKeyboardDidHide);

            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, HandleKeyboardDidShow);

            ShadowCreate(PasswordField, ShadowViewPasswordField);
            ShadowCreate(LoginField, ShadowView);

            AddShadow(LoginFacebookButton);
            AddShadow(RegistrationButton);
            AddShadow(LoginGoogleButton);
            AddShadow(RememberSwitch);
            AddShadow(LoginButton);

            base.ViewDidLoad();
        }

        #region Login

        void FacebookLogin()
        {
            var authenticator = _facebookAuthenticationService.GetAuthenticator();
            var view = authenticator.GetUI();
            PresentViewController(view, true, null);
        }

        void GoogleLogin()
        {
            var authenticator = _googleAuthenticationService.GetAuthenticator();

            ThisApp.OpenUrlExecuted += (s, uri) =>
            {
                authenticator.OnPageLoading(uri);
            };

            var viewController = authenticator.GetUI();

            PresentViewController(viewController, true, null);

        }

        protected void Unsubscribe()
        {
            _googleAuthenticationService.OnAuthenticationCompleted -= OnGoogleAuthenticationServiceAuthenticationCompleted;

            _googleAuthenticationService.OnAuthenticationFailed -= OnAuthenticationServiceAuthenticationFailed;

            _facebookAuthenticationService.OnAuthenticationCompleted -= OnFacebookAuthenticationServiceAuthenticationCompleted;

            _facebookAuthenticationService.OnAuthenticationFailed -= OnAuthenticationServiceAuthenticationFailed;
        }

        #endregion
    }
}

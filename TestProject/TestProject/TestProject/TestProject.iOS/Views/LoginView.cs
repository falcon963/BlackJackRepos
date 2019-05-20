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
            ;
        }

        public LoginView(IAuthenticationFacebookService facebookAuthenticationService, IAuthenticationGoogleService googleAuthenticationService) : base(nameof(LoginView), null)
        {
            _googleAuthenticationService = googleAuthenticationService;
            _facebookAuthenticationService = facebookAuthenticationService;
        }

        public override bool SetupEvents()
        {
            _googleAuthenticationService.OnAuthenticationCompleted += _googleAuthenticationService_OnAuthenticationCompleted;

            _googleAuthenticationService.OnAuthenticationFailed += AuthenticationService_OnAuthenticationFailed;

            _facebookAuthenticationService.OnAuthenticationCompleted += _facebookAuthenticationService_OnAuthenticationCompleted;

            _facebookAuthenticationService.OnAuthenticationFailed += AuthenticationService_OnAuthenticationFailed;

            return base.SetupEvents();
        }

        #endregion

        #region Handlers

        private void AuthenticationService_OnAuthenticationFailed(object sender, EventArgs e)
        {
            DismissViewController(true, null);
        }

        private void _facebookAuthenticationService_OnAuthenticationCompleted(object sender, EventArgs e)
        {
            ViewModel.SignInWithFacebookCommand.Execute();
            DismissViewController(true, null);
        }

        private void _googleAuthenticationService_OnAuthenticationCompleted(object sender, EventArgs e)
        {
            ViewModel?.SignInWithFacebookCommand?.Execute();
            DismissViewController(true, null);
        }

        #endregion

        public override bool SetupBindings()
        {
            BindingSet.Bind(LoginScrollView).For(v => v.BackgroundColor).To(vm => vm.LoginColor).WithConversion("NativeColor");
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
            base.ViewDidLoad();

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
            var authenticator = _facebookAuthenticationService.GetAuthenticator();
            var view = authenticator.GetUI();
            PresentViewController(view, true, null);
        }

        void GoogleLogin()
        {
            var authenticator = _googleAuthenticationService.GetAuthenticator();
            var viewController = authenticator.GetUI();
            PresentViewController(viewController, true, null);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _googleAuthenticationService.OnAuthenticationCompleted -= _googleAuthenticationService_OnAuthenticationCompleted;

            _googleAuthenticationService.OnAuthenticationFailed -= AuthenticationService_OnAuthenticationFailed;

            _facebookAuthenticationService.OnAuthenticationCompleted -= _facebookAuthenticationService_OnAuthenticationCompleted;

            _facebookAuthenticationService.OnAuthenticationFailed -= AuthenticationService_OnAuthenticationFailed;
        }

        #endregion
    }
}

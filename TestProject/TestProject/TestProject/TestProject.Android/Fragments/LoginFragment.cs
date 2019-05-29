using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using TestProject.Core.ViewModels;
using Android.Support.V4.Widget;
using Android.Support.V4.View;
using Android.Views.InputMethods;
using Acr.UserDialogs;
using static Android.Gms.Common.Apis.GoogleApiClient;
using LoginResult = Xamarin.Facebook.Login.LoginResult;
using Xamarin.Facebook;
using Android.Gms.Common;
using Java.Lang;
using TestProject.Core.Services;
using System.Threading.Tasks;
using Xamarin.Facebook.Login.Widget;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common.Apis;
using Android.Gms.Auth.Api;
using TestProject.Core.Authentication.Interfaces;
using MvvmCross;
using MvvmCross.Logging;
using TestProject.Droid.Services;
using Xamarin.Facebook.Share.Widget;
using Xamarin.Facebook.Share.Model;
using UriParse = Android.Net.Uri;
using TestProject.Core.Authentication;
using TestProject.Droid.Services.Interfaces;
using Resource = MvvmCross.Droid.Support.V7.AppCompat.Resource;
using TestProject.Core.Helpers.Interfaces;
using TestProject.Droid.Controls;
using Android.Gms.Auth;
using TestProject.Core.Constants;
using Void = Java.Lang.Void;
using String = Java.Lang.String;

namespace TestProject.Droid.Fragments
{
    [MvxFragmentPresentation(
        typeof(MainRegistrationViewModel), 
        Resource.Id.login_frame)]
    public class LoginFragment
        : BaseFragment<LoginViewModel>
    {

        protected override int _fragmentId => Resource.Layout.LoginFragment;

        private SignInButton _googleButton;
        private LoginButton _facebookButton;
        private GoogleApiClient _googleApiClient;

        private IFacebookAuthenticationService _facebookAuthenticationService;
        private IGoogleAuthenticationApiService _googleAuthenticationApiService;

        private const string publicProfile = "public_profile";

        private const int SignInGoogleId = 9001;

        

        public LoginFragment()
        {

            _facebookAuthenticationService = Mvx.IoCProvider.Resolve<IFacebookAuthenticationService>();
            _googleAuthenticationApiService = Mvx.IoCProvider.Resolve<IGoogleAuthenticationApiService>();

            _facebookAuthenticationService.InitializeFacebookAuth();
            _facebookAuthenticationService.OnAuthenticationCompleted += FacebookOnAuthenticationCompleted;
        }

        private void FacebookOnAuthenticationCompleted(object sender, EventArgs e)
        {
            ViewModel?.SignInWithFacebookCommand?.Execute();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            _linearLayout = view.FindViewById<AppMainLinearLayout>(Resource.Id.login_linearlayout);

            _googleButton = view.FindViewById<SignInButton>(Resource.Id.btnGoogleSignIn);

            _facebookButton = view.FindViewById<LoginButton>(Resource.Id.btnFacebookSignIn);

            _facebookButton.Click += OnFacebookLoginButtonClicked;

            InitializeGoogleAuth();

            return view;
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == SignInGoogleId)
            {
                var result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
                HandleSignInGoogleResult(result);
            }
        }

        private void HandleSignInGoogleResult(GoogleSignInResult result)
        {
            if (result.IsSuccess)
            {
                var accountData = result.SignInAccount;

                var userHelper = Mvx.IoCProvider.Resolve<IUserHelper>();

                Task.Factory.StartNew(() =>
                {
                    var token = GoogleAuthUtil.GetToken(
                             Context,
                             accountData.Account,
                             "oauth2:" + SocialConstants.Scope);

                    userHelper.UserAccessToken = token;

                    Activity.RunOnUiThread(() =>
                    {
                        ViewModel?.SignInWithGoogleCommand?.Execute();

                    });
                });
               
            }
            if (!result.IsSuccess)
            {
                _googleApiClient.Connect();
            }
        }

        private void SignIn()
        {
            var signInIntent = Auth.GoogleSignInApi.GetSignInIntent(_googleApiClient);

            StartActivityForResult(signInIntent, SignInGoogleId);

            Auth.GoogleSignInApi.SignOut(_googleApiClient);
        }

        #region AuthSocialInitialize

        private void InitializeGoogleAuth()
        {
            GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                .RequestIdToken(GetString(Resource.String.google_default_web_client_id))
                .RequestEmail()
                .Build();

            _googleApiClient = new GoogleApiClient.Builder(Application.Context)
               .EnableAutoManage(Activity, _googleAuthenticationApiService.OnConnectionFailed)
               .AddApi(Auth.GOOGLE_SIGN_IN_API, gso)
               .AddConnectionCallbacks(_googleAuthenticationApiService.OnConnected)
               .Build();

            _googleButton.Click += (sender, e) => {
                SignIn();
            };
        }

        #endregion

        private void OnFacebookLoginButtonClicked(object sender, EventArgs e)
        {
            var authenticator = _facebookAuthenticationService.GetAuthenticator();
            var intent = authenticator.GetUI(Context);
            StartActivity(intent);
        }

        protected override void Dispose(bool disposing)
        {
            _facebookAuthenticationService.OnAuthenticationCompleted -= FacebookOnAuthenticationCompleted;
            _toolbar.Click -= HideSoftKeyboard;
            _linearLayout.Click -= HideSoftKeyboard;

            base.Dispose(disposing);
        }

        public override void OnPause()
        {
            base.OnPause();
            _googleApiClient.StopAutoManage(Activity);
            _googleApiClient.Disconnect();
        }
    }
}
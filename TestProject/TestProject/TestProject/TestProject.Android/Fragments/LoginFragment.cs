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

namespace TestProject.Droid.Fragments
{
    [MvxFragmentPresentation(
        typeof(MainRegistrationViewModel), 
        Resource.Id.login_frame)]
    public class LoginFragment
        : BaseFragment<LoginViewModel>, IOnConnectionFailedListener, IConnectionCallbacks
    {

        protected override int _fragmentId => Resource.Layout.LoginFragment;

        private SignInButton _googleButton;
        private LoginButton _facebookButton;
        private GoogleApiClient _googleApiClient;
        private Action _singInCommand;
        private GoogleAuthenticationService _googleAuthentication;
        private IFacebookAuthenticationService _facebookAuthenticationService;
        private IMvxLog _mvxLog;

        private const string publicProfile = "public_profile";

        private int SIGN_IN_GOOGLE_ID = 9001;

        public LoginFragment(IFacebookAuthenticationService facebookAuthenticationService, IMvxLog mvxLog)
        {
            _facebookAuthenticationService = facebookAuthenticationService;
            _mvxLog = mvxLog;

            _facebookAuthenticationService.OnAuthenticationCompleted += FacebookOnAuthenticationCompleted;
        }

        private void FacebookOnAuthenticationCompleted(object sender, EventArgs e)
        {
            ViewModel?.SignInWithFacebookCommand?.Execute();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            _linearLayout = view.FindViewById<LinearLayout>(Resource.Id.login_linearlayout);

            _googleButton = view.FindViewById<SignInButton>(Resource.Id.btnGoogleSignIn);

            _facebookButton = view.FindViewById<LoginButton>(Resource.Id.btnFacebookSignIn);

            _facebookButton.Click += OnFacebookLoginButtonClicked;

            InitializeGoogleAuth();

            return view;
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == SIGN_IN_GOOGLE_ID)
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
                ViewModel?.SignInWithGoogleCommand?.Execute();
            }
        }

        void IOnConnectionFailedListener.OnConnectionFailed(ConnectionResult result)
        {
            _mvxLog.Trace(result.ErrorMessage);
        }

        private void FailedHandlerGoogleAuth(ConnectionResult obj)
        {
            _mvxLog.Trace(obj.ErrorMessage);
        }


        private void CallBackGoogle(Bundle obj)
        {

        }

        private void SignIn()
        {
            var signInIntent = Auth.GoogleSignInApi.GetSignInIntent(_googleApiClient);

            StartActivityForResult(signInIntent, SIGN_IN_GOOGLE_ID);
        }

        #region AuthSocialInitialize

        private void InitializeGoogleAuth()
        {

            GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                .RequestEmail()
                .Build();

            _googleApiClient = new GoogleApiClient.Builder(Application.Context)
               .EnableAutoManage(Activity, FailedHandlerGoogleAuth)
               .AddApi(Auth.GOOGLE_SIGN_IN_API, gso)
               .AddConnectionCallbacks(CallBackGoogle)
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

        private void OnGoogleLoginButtonClicked(object sender, EventArgs e)
        {
            GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                .RequestEmail()
                .Build();
        }

        public void OnConnected(Bundle connectionHint)
        {
            
        }

        public void OnConnectionSuspended(int cause)
        {
            
        }

        protected override void Dispose(bool disposing)
        {
            _facebookAuthenticationService.OnAuthenticationCompleted -= FacebookOnAuthenticationCompleted;

            base.Dispose(disposing);
        }
    }
}
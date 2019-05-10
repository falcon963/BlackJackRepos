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

namespace TestProject.Droid.Fragments
{
    [MvxFragmentPresentation(
        typeof(MainRegistrationViewModel), 
        Resource.Id.login_frame)]
    public class LoginFragment 
        : BaseFragment<LoginViewModel>, IOnConnectionFailedListener, IGoogleAuthenticationDelegate, IFacebookCallback
    {

        protected override int _fragmentId => Resource.Layout.LoginFragment;

        private SignInButton _googleButton;
        private LoginButton _facebookButton;
        private GoogleApiClient _googleApiClient;
        private Action _singInCommand;
        private GoogleAuthenticationService _googleAuthentication;

        private const string publicProfile = "public_profile";

        public static ICallbackManager _callbackManager;

        private int SIGN_IN_GOOGLE_ID = 9001;

        public LoginFragment()
        {
            _singInCommand = () => { ViewModel?.SignInWithGoogleCommand?.Execute(); };
            _googleAuthentication = new GoogleAuthenticationService(_singInCommand);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            _linearLayout = view.FindViewById<LinearLayout>(Resource.Id.login_linearlayout);

            _googleButton = view.FindViewById<SignInButton>(Resource.Id.btnGoogleSignIn);

            _facebookButton = view.FindViewById<LoginButton>(Resource.Id.btnFacebookSignIn);

            InitializeFacebookAuth();
            InitializeGoogleAuth();

            return view;
        }

        void IFacebookCallback.OnCancel()
        {

        }

        void IFacebookCallback.OnError(FacebookException error)
        {

        }

        void IFacebookCallback.OnSuccess(Java.Lang.Object result)
        {
            ViewModel?.SignInWithFacebookCommand?.Execute();
        }

        void IOnConnectionFailedListener.OnConnectionFailed(ConnectionResult result)
        {

        }

        void IGoogleAuthenticationDelegate.OnAuthenticationCompleted(string token)
        {
            ViewModel?.SignInWithGoogleCommand?.Execute();
        }


        void IGoogleAuthenticationDelegate.OnAuthenticationCanceled()
        {

        }

        void IGoogleAuthenticationDelegate.OnAuthenticationFailed(string message, System.Exception exception)
        {

        }

        private void FailedHandlerGoogleAuth(ConnectionResult obj)
        {
            var logger = Mvx.IoCProvider.Resolve<IMvxLog>();
            logger.Trace(obj.ErrorMessage);
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

        private void InitializeFacebookAuth()
        {
            FacebookSdk.SdkInitialize(Application.Context);

            _callbackManager = CallbackManagerFactory.Create();

            _facebookButton.SetReadPermissions(new List<string> { publicProfile });
            _facebookButton.RegisterCallback(_callbackManager, this);
        }

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
    }
}
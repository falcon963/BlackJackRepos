using CoreGraphics;
using Foundation;
using Google.Maps;
using MvvmCross;
using MvvmCross.Platforms.Ios.Core;
using MvvmCross.Plugin.Color.Platforms.Ios;
using Plugin.SecureStorage;
using System;
using System.Diagnostics;
using System.Linq;
using TestProject.Core;
using TestProject.Core.Colors;
using TestProject.iOS.Providers;
using TestProject.iOS.Views;
using UIKit;

namespace TestProject.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : MvxApplicationDelegate<Setup, App>
    {
        // class-level declarations

        const string MapsApiKey = "AIzaSyCzfcYRYDcsR8nEAEcSJfPxtKpcVlBCq84";

        public override UIWindow Window
        {
            get;
            set;
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            bool result = false;

            try
            {
                MapServices.ProvideAPIKey(MapsApiKey);
                result = base.FinishedLaunching(application, launchOptions);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return result;
        }

        #region OpenUrl

        //public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        //{
        //    AppDeepLinksEntry(url);

        //    Uri uri_netfx = new Uri(url.AbsoluteString);

        //    OpenUrlExecuted?.Invoke(this, uri_netfx);

        //    return true;
        //}

        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            AppDeepLinksEntry(url);

            Uri uri_netfx = new Uri(url.AbsoluteString);

            OpenUrlExecuted?.Invoke(this, uri_netfx);

            return true;
        }

        #endregion OpenUrl


        private bool AppDeepLinksEntry(NSUrl url)
        {
            Debug.WriteLine($"OpenUrl Url : {url}");
            Debug.WriteLine($"OpenUrl Url Query: {url.Query}");
            Debug.WriteLine($"OpenUrl Url Host: {url.Host}");
            Debug.WriteLine($"OpenUrl Url Path: {url.Path}");

            return true;
        }

        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.
        }

        public override void DidEnterBackground(UIApplication application)
        {
            // Use this method to release shared resources, save user data, invalidate timers and store the application state.
            // If your application supports background exection this method is called instead of WillTerminate when the user quits.
        }

        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transiton from background to active state.
            // Here you can undo many of the changes made on entering the background.
        }

        public override void OnActivated(UIApplication application)
        {
            // Restart any tasks that were paused (or not yet started) while the application was inactive. 
            // If the application was previously in the background, optionally refresh the user interface.
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }

        #region Events
        public event EventHandler<Uri> OpenUrlExecuted;
        #endregion
    }
}


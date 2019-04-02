using Foundation;
using System;
using TestProject.Core.ViewModels;
using Google.Maps;
using CoreGraphics;
using CoreLocation;
using MvvmCross.Binding.BindingContext;
using UIKit;
using MvvmCross.Platforms.Ios.Presenters.Attributes;

namespace TestProject.iOS.Views
{
    [MvxTabPresentation(WrapInNavigationController = true, TabName = "Map", TabIconName = "icons8_google_maps_24")]
    public partial class AppMapView
        : BaseMenuView<UserLocationViewModel>
    {
        MapView _mapView;
        Boolean _firstLocationUpdate;
        public AppMapView() : base("AppMapView", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = this.CreateBindingSet<AppMapView, UserLocationViewModel>();
            set.Bind(BackButton).To(vm => vm.GoBackCommand);
            set.Apply();

            ViewModel.GetLocated.Execute();

            var camera = CameraPosition.FromCamera(
                new CoreLocation.CLLocationCoordinate2D(
                    ViewModel.Latitude, ViewModel.Longitude), 12);

            var frame = MapFrameView.Frame;
            _mapView = new MapView(new CGRect(0, 0, frame.Width, frame.Height));
            _mapView = MapView.FromCamera(new CGRect(0, 0, frame.Width, frame.Height), camera);
            _mapView.Settings.CompassButton = true;
            _mapView.Settings.MyLocationButton = true;
            _mapView.Settings.ZoomGestures = true;
            MapFrameView.AddSubview(_mapView);

            BackButton.Image = UIImage.FromFile("back_to_50.png");

            InvokeOnMainThread(() => _mapView.MyLocationEnabled = true);

        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            _mapView.AddObserver(this, new NSString("myLocation"), NSKeyValueObservingOptions.New, IntPtr.Zero);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            _mapView.RemoveObserver(this, new NSString("myLocation"));
        }

        public override void ObserveValue(NSString keyPath, NSObject ofObject, NSDictionary change, IntPtr context)
        {
            //base.ObserveValue (keyPath, ofObject, change, context);

            if (!_firstLocationUpdate)
            {
                // If the first location update has not yet been recieved, then jump to that
                // location.
                _firstLocationUpdate = true;
                var location = change.ObjectForKey(NSValue.ChangeNewKey) as CLLocation;
                _mapView.Camera = CameraPosition.FromCamera(location.Coordinate, 14);
            }
        }
    }
}
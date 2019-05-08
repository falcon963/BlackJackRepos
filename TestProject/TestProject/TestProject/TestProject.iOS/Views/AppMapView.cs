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
    [MvxTabPresentation(WrapInNavigationController = true, TabName = "Map", TabIconName = "map")]
    public partial class AppMapView
        : BaseView<AppMapView ,LocationViewModel>
    {
        MapView _mapView;
        bool _firstLocationUpdate;
        private const string myLocation = "myLocation";

        public AppMapView() : base("AppMapView", null)
        {
        }

        public override bool SetupBindings()
        {
            BindingSet.Bind(BackButton).To(vm => vm.CloseCommand);
            return base.SetupBindings();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ViewModel.GetLocated.Execute();

            var camera = CameraPosition.FromCamera(
                new CLLocationCoordinate2D(
                    ViewModel.Latitude, ViewModel.Longitude), 12);

            var frame = MapFrameView.Frame;
            _mapView = new MapView(new CGRect(0, 0, frame.Width, frame.Height));
            _mapView = MapView.FromCamera(new CGRect(0, 0, frame.Width, frame.Height), camera);
            _mapView.Settings.CompassButton = true;
            _mapView.Settings.MyLocationButton = true;
            _mapView.Settings.ZoomGestures = true;
            MapFrameView.AddSubview(_mapView);

            BackButton.Image = UIImage.FromBundle("back");

            InvokeOnMainThread(() => _mapView.MyLocationEnabled = true);

        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            _mapView.AddObserver(this, new NSString(myLocation), NSKeyValueObservingOptions.New, IntPtr.Zero);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            _mapView.RemoveObserver(this, new NSString(myLocation));
        }

        public override void ObserveValue(NSString keyPath, NSObject ofObject, NSDictionary change, IntPtr context)
        {

            if (!_firstLocationUpdate)
            {
                _firstLocationUpdate = true;
                var location = change.ObjectForKey(NSValue.ChangeNewKey) as CLLocation;
                _mapView.Camera = CameraPosition.FromCamera(location.Coordinate, 14);
            }
        }
    }
}
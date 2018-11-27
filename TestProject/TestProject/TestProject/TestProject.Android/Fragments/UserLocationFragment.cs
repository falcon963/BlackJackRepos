using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using TestProject.Core.ViewModels;

namespace TestProject.Droid.Fragments
{
    [MvxFragmentPresentation(
        typeof(MainViewModel),
        Resource.Id.content_frame,
        true)]
    [Register("testproject.droid.fragments.UserLocationFragment")]
    public class UserLocationFragment 
        : BaseFragment<UserLocationViewModel>,
        IOnMapReadyCallback
    {
        protected override int FragmentId => Resource.Layout.UserLocationFragment;

        private GoogleMap _googleMap;
        private MapView _mapView;
        private Object thisLock = new Object();

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            var view = base.OnCreateView(inflater, container, savedInstanceState);

            _mapView = view.FindViewById<MapView>(Resource.Id.googlemap);

            _mapView.OnCreate(savedInstanceState);

            if (_googleMap == null)
            {
                _mapView.GetMapAsync(this);
            }

            return view;
        }


        public void OnMapReady(GoogleMap googleMap)
        {
            this._googleMap = googleMap;

            _googleMap.MapType = GoogleMap.MapTypeNormal;
            _googleMap.UiSettings.MyLocationButtonEnabled = true;
            _googleMap.UiSettings.ZoomControlsEnabled = true;
            _googleMap.BuildingsEnabled = true;

            // _googleMap.MyLocationEnabled = true;
            LatLng latlng = new LatLng(ViewModel.Latitude, ViewModel.Longitude);
            CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(latlng, 15);

            _googleMap.MoveCamera(camera);

            MarkerOptions markerOptions = new MarkerOptions();
            markerOptions.SetPosition(latlng);
            markerOptions.SetTitle("My located");

            BitmapDescriptor mapIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.icons8_location_off_30);
            markerOptions.SetIcon(mapIcon);

            _googleMap.AddMarker(markerOptions);

            GetRandomLocation(latlng, 80);
        }


        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Latitude")
            {
                LatLng latlng = new LatLng(ViewModel.Latitude, ViewModel.Longitude);
                CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(latlng, 15);
                _googleMap.MoveCamera(camera);

                MarkerOptions markerOptions = new MarkerOptions();
                markerOptions.SetPosition(latlng);
                markerOptions.SetTitle("My located");

                BitmapDescriptor mapIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.icons8_location_off_30);
                markerOptions.SetIcon(mapIcon);

                _googleMap.AddMarker(markerOptions);
            }
        }


        public void GetRandomLocation(LatLng point, Int32 radius)
        {
                for (Int32 i = 0; i < 5; i++)
                {
                    Thread.Sleep(10);
                    Random random = new Random();

                    Double x0 = point.Latitude;
                    Double y0 = point.Longitude;

                    Double radiusInDegrees = radius / 111000f;

                    Double u = random.NextDouble();
                    Double v = random.NextDouble();
                    Double w = radiusInDegrees * Math.Sqrt(u);
                    Double t = 2 * Math.PI * v;
                    Double x = w * Math.Cos(t);
                    Double y = w * Math.Sin(t);

                    Double new_x = x / Math.Cos(y0);

                    Double foundLatitude = new_x + x0;
                    Double foundLongitude = y + y0;

                    LatLng randomLatLng = new LatLng(foundLatitude, foundLongitude);
                    MarkerOptions markerRandomOptions = new MarkerOptions();
                    markerRandomOptions.SetPosition(randomLatLng);
                    _googleMap.AddMarker(markerRandomOptions);
                }
        }


        public override void OnResume()
        {
            _mapView.OnResume();
            base.OnResume();
        }

        public override void OnPause()
        {
            base.OnPause();
            _mapView.OnPause();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            _mapView.OnDestroy();
        }

        public override void OnLowMemory()
        {
            base.OnLowMemory();
            _mapView.OnLowMemory();
        }

    }
}
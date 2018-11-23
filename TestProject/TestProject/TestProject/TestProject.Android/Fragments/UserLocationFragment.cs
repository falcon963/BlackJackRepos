using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, true)]
    [Register("testproject.droid.fragments.UserLocationFragment")]
    public class UserLocationFragment : BaseFragment<UserLocationViewModel>, IOnMapReadyCallback
    {
        protected override int FragmentId => Resource.Layout.UserLocationFragment;

        private GoogleMap _googleMap;

        private LinearLayout _linearLayout;

        private MapView _mapView;

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

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Latitude")
            {
                _googleMap.MyLocationEnabled = true;
                var lat = ViewModel.Latitude;
                var lng = ViewModel.Longitude;
                LatLng latlng = new LatLng(lat, lng);
                CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(latlng, 15);
                _googleMap.MoveCamera(camera);
            }
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            this._googleMap = googleMap;
            _googleMap.MapType = GoogleMap.MapTypeNormal;
            _googleMap.UiSettings.MyLocationButtonEnabled = true;
            _googleMap.MyLocationEnabled = true;
            var lat = ViewModel.Latitude;
            var lng = ViewModel.Longitude;
            LatLng latlng = new LatLng(lat, lng);
            CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(latlng, 15);
            _googleMap.MoveCamera(camera);
            _googleMap.BuildingsEnabled = true;
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
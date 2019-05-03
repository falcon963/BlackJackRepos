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
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using TestProject.Core.ViewModels;
using TestProject.Droid.Views;

namespace TestProject.Droid.Fragments
{
    [MvxFragmentPresentation(
        typeof(MainViewModel),
        Resource.Id.content_frame,
        true)]
    [Register("testproject.droid.fragments.UserLocationFragment")]
    public class LocationFragment 
        : BaseFragment<LocationViewModel>,
        IOnMapReadyCallback
    {
        protected override int FragmentId => Resource.Layout.LocationFragment;

        private GoogleMap _googleMap;
        private MapView _mapView;
        private object thisLock = new object();

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            var view = base.OnCreateView(inflater, container, savedInstanceState);

            _mapView = view.FindViewById<MapView>(Resource.Id.googlemap);

            _mapView.OnCreate(savedInstanceState);

            ParentActivity.SetSupportActionBar(Toolbar);

            if (_googleMap == null)
            {
                _mapView.GetMapAsync(this);
            }

            ((MainActivity)ParentActivity).DrawerLayout.SetDrawerLockMode(DrawerLayout.LockModeLockedClosed);

            return view;
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            ViewModel.GetLocated.Execute();

            this._googleMap = googleMap;

            _googleMap.MapType = GoogleMap.MapTypeNormal;
            _googleMap.UiSettings.MyLocationButtonEnabled = true;
            _googleMap.UiSettings.ZoomControlsEnabled = true;
            _googleMap.BuildingsEnabled = true;

            LatLng latlng = new LatLng(ViewModel.Latitude, ViewModel.Longitude);
            CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(latlng, 15);

            _googleMap.MoveCamera(camera);

            MarkerOptions markerOptions = new MarkerOptions();
            markerOptions.SetPosition(latlng);
            markerOptions.SetTitle("My located");

            BitmapDescriptor mapIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.location);
            markerOptions.SetIcon(mapIcon);

            _googleMap.AddMarker(markerOptions);

            GetRandomLocation(latlng, 80);
        }



        public void GetRandomLocation(LatLng point, int radius)
        {
                for (int i = 0; i < 5; i++)
                {
                    Thread.Sleep(10);
                    Random random = new Random();

                    double x0 = point.Latitude;
                    double y0 = point.Longitude;

                    double radiusInDegrees = radius / 111000f;

                    double u = random.NextDouble();
                    double v = random.NextDouble();
                    double w = radiusInDegrees * Math.Sqrt(u);
                    double t = 2 * Math.PI * v;
                    double x = w * Math.Cos(t);
                    double y = w * Math.Sin(t);

                    double new_x = x / Math.Cos(y0);

                    double foundLatitude = new_x + x0;
                    double foundLongitude = y + y0;

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

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if(item.ItemId == Resource.Id.home)
            {
                ViewModel?.CloseCommand?.Execute();
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}
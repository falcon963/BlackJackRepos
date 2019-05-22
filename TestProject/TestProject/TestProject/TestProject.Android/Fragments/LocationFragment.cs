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
using MvvmCross;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using TestProject.Core.ViewModels;
using TestProject.Droid.Services.Interfaces;
using TestProject.Droid.Views;
using TestProject.LanguageResources;
using ILocationService = TestProject.Droid.Services.Interfaces.ILocationService;

namespace TestProject.Droid.Fragments
{
    [MvxFragmentPresentation(
        typeof(MainViewModel),
        Resource.Id.content_frame,
        true)]
    public class LocationFragment 
        : BaseFragment<LocationViewModel>,
        IOnMapReadyCallback
    {
        protected override int _fragmentId => Resource.Layout.LocationFragment;

        private GoogleMap _googleMap;
        private MapView _mapView;

        private ILocationService _locationService;

        public LocationFragment()
        {
            _locationService = Mvx.IoCProvider.Resolve<ILocationService>();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            var view = base.OnCreateView(inflater, container, savedInstanceState);

            _mapView = view.FindViewById<MapView>(Resource.Id.googlemap);

            _mapView.OnCreate(savedInstanceState);

            ParentActivity.SetSupportActionBar(_toolbar);

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

            _googleMap = googleMap;

            _googleMap.MapType = GoogleMap.MapTypeNormal;
            _googleMap.UiSettings.MyLocationButtonEnabled = true;
            _googleMap.UiSettings.ZoomControlsEnabled = true;
            _googleMap.BuildingsEnabled = true;

            LatLng latlng = new LatLng(ViewModel.Latitude, ViewModel.Longitude);
            CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(latlng, 15);

            _googleMap.MoveCamera(camera);

            MarkerOptions markerOptions = new MarkerOptions();
            markerOptions.SetPosition(latlng);
            markerOptions.SetTitle(Strings.MyLocated);

            BitmapDescriptor mapIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.location);
            markerOptions.SetIcon(mapIcon);

            _googleMap.AddMarker(markerOptions);

            _locationService.GetRandomLocation(latlng, 80, _googleMap);
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
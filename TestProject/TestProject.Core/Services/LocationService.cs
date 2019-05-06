using MvvmCross;
using MvvmCross.Base;
using MvvmCross.Plugin.Location;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Services.Interfaces;

namespace TestProject.Core.Services
{
    public class LocationService 
        : ILocationService
    {
        private MvxGeoLocation _latestLocation;

        private readonly object _lockObject;

        private readonly IMvxLocationWatcher _watcher;

        public LocationService(IMvxMainThreadDispatcher mvxMainThread, IMvxLocationWatcher locationWatcher)
        {
            _lockObject = new object();

            _watcher = locationWatcher;
        }

        private void OnSuccess(MvxGeoLocation location)
        {
            lock (_lockObject)
            {
                _latestLocation = location;
            }
        }

        public void Start()
        {
            _watcher.Start(new MvxLocationOptions() { Accuracy = MvxLocationAccuracy.Coarse }, OnSuccess, OnError);
        }

        public void Stop()
        {
            _watcher.Stop();
        }

        private void OnError(MvxLocationError error)
        {
            
        }

        public bool TryGetLatestLocation(out double lat, out double lng)
        {
            lock (_lockObject)
            {
                if (_latestLocation == null)
                {
                    lat = lng = 0;

                    return false;
                }

                lat = _latestLocation.Coordinates.Latitude;
                lng = _latestLocation.Coordinates.Longitude;

                return true;
            }
        }
    }
}

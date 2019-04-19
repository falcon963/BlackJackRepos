using MvvmCross;
using MvvmCross.Base;
using MvvmCross.Plugin.Location;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Servicies.Interfacies;

namespace TestProject.Core.Servicies
{
    public class LocationService 
        : ILocationService
    {
        private MvxGeoLocation _latestLocation;

        private readonly object _lockObject = new object();

        public IMvxLocationWatcher Watcher { get; }

        public LocationService(IMvxMainThreadDispatcher mvxMainThread)
        {
            Watcher = Mvx.Resolve<IMvxLocationWatcher>();
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
            Watcher.Start(new MvxLocationOptions() { Accuracy = MvxLocationAccuracy.Coarse }, OnSuccess, OnError);
        }

        public void Stop()
        {
            Watcher.Stop();
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

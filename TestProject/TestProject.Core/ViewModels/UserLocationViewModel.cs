using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Location;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TestProject.Core.ViewModels
{
    public class UserLocationViewModel: BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        private readonly IMvxLocationWatcher _watcher; 

        public UserLocationViewModel(IMvxLocationWatcher watcher, IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
            _watcher = watcher;
            _watcher.Start(new MvxLocationOptions(), OnLocation, OnError);
        }

        private void OnError(MvxLocationError error)
        {
            
        }

        private void OnLocation(MvxGeoLocation location)
        {
            Latitude = location.Coordinates.Latitude;
            Longitude = location.Coordinates.Longitude;
        }

        private Double _latitude;
        public Double Latitude
        {
            get => _latitude;
            set => SetProperty(ref _latitude, value);
        }


        private Double _longitude;
        public Double Longitude
        {
            get => _longitude;
            set => SetProperty(ref _longitude, value);
        }

        #region Commands


        public IMvxAsyncCommand GoBackCommand
        {
            get
            {
                return new MvxAsyncCommand(async() =>
                {
                    _watcher.Stop();
                    await _navigationService.Close(this);
                });
            }
        }


        #endregion Commands

    }
}

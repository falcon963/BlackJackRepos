using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Location;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using TestProject.Core.Interfaces;

namespace TestProject.Core.ViewModels
{
    public class UserLocationViewModel
        : BaseViewModel
    {
        #region Fields

        private readonly IMvxNavigationService _navigationService;

        private readonly ILocationService _locationService;

        private Double _latitude;

        private Double _longitude;

        #endregion

        #region Propertys

        public Double Latitude
        {
            get => _latitude;
            set => SetProperty(ref _latitude, value);
        }

        public Double Longitude
        {
            get => _longitude;
            set => SetProperty(ref _longitude, value);
        }

        #endregion

        public UserLocationViewModel(ILocationService locationService, IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
            _locationService = locationService;
        }

        #region Commands


        public IMvxAsyncCommand GoBackCommand
        {
            get
            {
                return new MvxAsyncCommand(async() =>
                {
                    _locationService.Stop();
                    await _navigationService.Close(this);
                });
            }
        }

        public IMvxCommand GetLocated
        {
            get
            {
                return new MvxCommand(() =>
                {
                    _locationService.Start();
                    _locationService.TryGetLatestLocation(out _latitude, out _longitude);
                });
            }
        }


        #endregion Commands

    }
}

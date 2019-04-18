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

        private readonly ILocationService _locationService;

        private double _latitude;

        private double _longitude;

        #endregion

        #region Propertys

        public double Latitude
        {
            get => _latitude;
            set => SetProperty(ref _latitude, value);
        }

        public double Longitude
        {
            get => _longitude;
            set => SetProperty(ref _longitude, value);
        }

        #endregion

        public UserLocationViewModel(ILocationService locationService, IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
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
                    await NavigationService.Close(this);
                });
            }
        }

        public IMvxAsyncCommand GetLocated
        {
            get
            {
                return new MvxAsyncCommand(async() =>
                {
                    _locationService.Start();
                    _locationService.TryGetLatestLocation(out _latitude, out _longitude);
                });
            }
        }


        #endregion Commands

    }
}

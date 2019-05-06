using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Location;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using TestProject.Core.Services.Interfaces;

namespace TestProject.Core.ViewModels
{
    public class LocationViewModel
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

        public LocationViewModel(ILocationService locationService, IMvxNavigationService navigationService) : base(navigationService)
        {
            _locationService = locationService;
        }

        #region Commands


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


        public override void ViewDestroy(bool viewFinishing = true)
        {
            base.ViewDestroy(viewFinishing);
            _locationService.Stop();
        }
    }
}

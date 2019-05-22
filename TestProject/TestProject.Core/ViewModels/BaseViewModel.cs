using MvvmCross.Commands;
using MvvmCross.Localization;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject.Core.ViewModels
{
    public abstract class BaseViewModel 
        : MvxViewModel
    {
        public IMvxNavigationService NavigationService { get; set; }

        protected BaseViewModel(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public IMvxLanguageBinder TextSource
        {
            get { return new MvxLanguageBinder(); }
        }

        public IMvxAsyncCommand CloseCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    await NavigationService.Close(this);
                });
            }
        }

    }

    public abstract class BaseViewModel<TParameter, TResult> 
        : MvxViewModel<TParameter, TResult>
    {
        public IMvxNavigationService NavigationService { get; set; }

        protected BaseViewModel(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public IMvxAsyncCommand CloseCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    await NavigationService.Close(this);
                });
            }
        }
    }

    public abstract class BaseViewModel<TParameter> 
        : MvxViewModel<TParameter>
    {
        public IMvxNavigationService NavigationService { get; set; }

        protected BaseViewModel(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public IMvxAsyncCommand CloseCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    await NavigationService.Close(this);
                });
            }
        }
    }
}


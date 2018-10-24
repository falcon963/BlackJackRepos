using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using TestProject.Core.Models;
using TestProject.Core.Interfaces;
using System.Threading.Tasks;

namespace TestProject.Core.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;



        public MainViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        #region Commands

        public IMvxAsyncCommand ShowMenuCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    await _navigationService.Navigate<TaskListViewModel>();
                });
            }
        }

        public IMvxAsyncCommand ShowTaskCommand
        {
            get
            {
                return new MvxAsyncCommand(async() =>
                {
                   await  _navigationService.Navigate<TaskViewModel>();
                });
            }
        }
        
        #endregion
    }
}

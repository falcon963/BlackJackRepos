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
    public class MainViewModel : BaseViewModel<Int32, ResultModel>
    {
        private readonly IMvxNavigationService _navigationService;

        ResultModel _result;

        ResultModel Result
        {
            get
            {
                return _result;
            }
            set
            {
                _result = value;
                RaisePropertyChanged(() => Result);
            }
        }

        public MainViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
            _result = new ResultModel();
            _result.Changes = new UserTask();
        }

        #region Commands

        public IMvxAsyncCommand<ResultModel> ShowMenuCommand
        {
            get
            {
                return new MvxAsyncCommand<ResultModel>(async (ResultModel model) =>
                {
                    model = Result;
                    await _navigationService.Navigate<TaskListViewModel, ResultModel, ResultModel>(model);
                });
            }
        }

        #endregion

        public override void Prepare(Int32 id)
        {
            Result.Changes.UserId = id;
        }
    }
}

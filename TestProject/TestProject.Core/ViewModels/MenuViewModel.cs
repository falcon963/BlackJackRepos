using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Acr.UserDialogs;
using Plugin.SecureStorage;
using TestProject.Core.Constant;
using TestProject.Core.Models;
using MvvmCross.ViewModels;
using System.Threading.Tasks;

namespace TestProject.Core.ViewModels
{
    public class MenuViewModel
        : BaseViewModel<TaskListViewModel>
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private ResultModel _userTask;
        private MvxObservableCollection<MenuItem> _menuItems;
        private TaskListViewModel _taskList;

        public ResultModel UserTask
        {
            get
            {
                return _userTask;
            }
            set
            {
                _userTask = value;
            }
        }

        public MvxObservableCollection<MenuItem> MenuItems
        {
            get
            {
                return _menuItems;
            }
            set
            {
                SetProperty(ref _menuItems, value);
            }
        }


        public MenuViewModel(IMvxNavigationService navigationService, IUserDialogs userDialogs)
        {
            _navigationService = navigationService;
            _userDialogs = userDialogs;
            _menuItems = new MvxObservableCollection<MenuItem>()
            {
                new MenuItem
                {
                    ItemAction = Enum.MenuItemAction.AddTask,
                    ItemTitle = "New task"
                },
                new MenuItem
                {
                    ItemAction = Enum.MenuItemAction.Logout,
                    ItemTitle = "Logout"
                },
                new MenuItem
                {
                    ItemAction = Enum.MenuItemAction.Location,
                    ItemTitle = "Map"
                }
            };
        }

        public IMvxCommand LogOutCommand
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    var logOut = await _userDialogs.ConfirmAsync(new ConfirmConfig
                    {
                        Title = "Alert Messege",
                        Message = "Do you want logout?",
                        OkText = "Yes",
                        CancelText = "No"
                    });
                    if (logOut)
                    {
                        CrossSecureStorage.Current.DeleteKey(SecureConstant.Status);
                        _taskList.UserLogoutCommand.Execute();
                        await _navigationService.Navigate<LoginViewModel>();
                        await _navigationService.Close(this);
                    }
                    if (!logOut)
                    {
                        return;
                    }
                });
            }
        }

        public IMvxAsyncCommand<MenuItem> ItemSelectCommand
        {
            get
            {
                return new MvxAsyncCommand<MenuItem>(async(item) =>
                {
                    var action = item.ItemAction;
                    if (action == Enum.MenuItemAction.AddTask)
                    {
                        _taskList.ShowTaskCommand.Execute(null);
                        await _navigationService.Close(this);
                    }
                    if(action == Enum.MenuItemAction.Logout)
                    {
                        LogOutCommand.Execute();
                    }
                    if(action == Enum.MenuItemAction.Location)
                    {
                        _taskList.GetLocationCommand.Execute(null);
                        await _navigationService.Close(this);
                    }
                });
            }
        }

        public override void Prepare(TaskListViewModel parameter)
        {
            _taskList = parameter;
        }

    }
}

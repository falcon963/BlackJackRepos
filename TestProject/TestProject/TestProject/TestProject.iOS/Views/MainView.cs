using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.Plugin.Color.Platforms.Ios;
using System;
using TestProject.Core.ViewModels;
using TestProject.Core.Colors;

namespace TestProject.iOS.Views
{
    [MvxRootPresentation]
    public class MainView
        :MvxTabBarViewController<MainViewModel>
    {
        private Boolean _firstTimePresented = true;

        public MainView()
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            TabBar.BarTintColor = AppColors.ColorTheme.ToNativeColor();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (_firstTimePresented)
            {
                _firstTimePresented = false;
                ViewModel.ShowMenuCommand.Execute();
            }
        }
    }
}
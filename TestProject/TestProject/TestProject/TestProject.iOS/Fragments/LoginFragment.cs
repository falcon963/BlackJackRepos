using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using TestProject.Core.Colors;
using TestProject.Core.ViewModels;
using TestProject.iOS.Converters;
using UIKit;
using TestProject.iOS.Converters;

namespace TestProject.iOS.Fragments
{
    [MvxTabPresentation(WrapInNavigationController = true,
        TabName = "Login",
        TabIconName ="ic_login")]
    public class LoginFragment
        :MvxViewController<LoginViewModel>
    {

        public LoginFragment()
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            var set = this.CreateBindingSet<LoginFragment, LoginViewModel>();
            set.Bind(backgroundView)
                .For(v => v.BackgroundColor)
                .To(AppColors.ColorTheme)
                .WithConversion(new ColorConverter());

        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using TestProject.Core.ViewModels;
using MvvmCross;
using Acr.UserDialogs;
using TestProject.Core.Interfaces;
using TestProject.Core.Services;
using MvvmCross.Localization;
using TestProject.Resources;

namespace TestProject.Core
{

    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            Mvx.IoCProvider.RegisterSingleton(() => new ResxTextProvider(Strings.ResourceManager));
            Mvx.IoCProvider.RegisterSingleton(() => UserDialogs.Instance);
            RegisterCustomAppStart<AppStart>();
        }
    }
}

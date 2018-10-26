using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using TestProject.Core.ViewModels;
using MvvmCross;
using Acr.UserDialogs;

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

            Mvx.IoCProvider.RegisterSingleton<IUserDialogs>(() => UserDialogs.Instance);
            RegisterCustomAppStart<AppStart>();
        }
    }
}

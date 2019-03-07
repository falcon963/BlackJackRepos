using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using MvvmCross.IoC;
using MvvmCross;
using MvvmCross.Platforms.Ios.Core;
using TestProject.Core;
using UIKit;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.ViewModels;
using MvvmCross.Navigation;
using Acr.UserDialogs;
using TestProject.Core.Interfaces;
using TestProject.Core.Services;
using System.Reflection;
using System.Collections;
using MvvmCross.Plugin.Color;

namespace TestProject.iOS
{
    public class Setup
        : MvxIosSetup<App>
    {
        //protected override void InitializeFirstChance()
        //{
        //    base.InitializeFirstChance();
        //}

        //protected override void InitializeLastChance()
        //{
        //    base.InitializeLastChance();

        //    var registry = Mvx.Resolve<IMvxTargetBindingFactoryRegistry>();
        //    registry.RegisterFactory(new MvxCustomBindingFactory<UIViewController>("NetworkIndicator", (viewController) =>
        //     new NetworkIndicatorTargetBinding(viewController)));
        //}

        //protected override IMvxIocOptions CreateIocOptions()
        //{
        //    return new MvxIocOptions
        //    {
        //        PropertyInjectorOptions = MvxPropertyInjectorOptions.MvxInject
        //    };
        //}

        protected override void RegisterPresenter()
        {
            base.RegisterPresenter();
        }

        protected override IEnumerable<Assembly> ValueConverterAssemblies
        {
            get
            {
                var toReturn = base.ValueConverterAssemblies as IList;
                toReturn.Add(typeof(MvxNativeColorValueConverter).Assembly);
                return (List<Assembly>)toReturn;
            }
        }

        protected override IMvxApplication CreateApp()
        {
            CreatableTypes()
                 .EndingWith("Service")
                 .AsInterfaces()
                 .RegisterAsLazySingleton();
            return base.CreateApp();
        }
    }
}
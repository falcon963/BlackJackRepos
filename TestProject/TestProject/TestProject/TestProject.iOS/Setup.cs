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
using TestProject.Core.Interfacies;
using TestProject.Core.Servicies;
using System.Reflection;
using System.Collections;
using MvvmCross.Plugin.Color;
using MvvmCross.Converters;
using TestProject.iOS.Converters;
using MvvmCross.Binding.Binders;

namespace TestProject.iOS
{
    public class Setup
        : MvxIosSetup<App>
    {
        //protected override void InitializeFirstChance()
        //{
        //    base.InitializeFirstChance();
        //}

        protected override void InitializeLastChance()
        {
            base.InitializeLastChance();

            var registry = Mvx.Resolve<IMvxValueConverterLookup>();
        }

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

        //protected override IEnumerable<Assembly> ValueConverterAssemblies
        //{
        //    get
        //    {
        //        var toReturn = base.ValueConverterAssemblies as IList;
        //        toReturn.Add(typeof(ColorValueConverter).Assembly);
        //        return (List<Assembly>)toReturn;
        //    }
        //}

        protected override IEnumerable<Assembly> ValueConverterAssemblies
        {
            get
            {
                var assemblies = base.ValueConverterAssemblies;
                var valueConverterAssemblies = assemblies as Assembly[] ?? assemblies.ToArray();
                valueConverterAssemblies.ToList().Add(typeof(ColorValueConverter).Assembly);
                return valueConverterAssemblies;
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
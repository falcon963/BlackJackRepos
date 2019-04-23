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

#pragma warning disable CS0618 // Type or member is obsolete
            var registry = Mvx.Resolve<IMvxValueConverterLookup>();
#pragma warning restore CS0618 // Type or member is obsolete
        }

        protected override void RegisterPresenter()
        {
            base.RegisterPresenter();
        }


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
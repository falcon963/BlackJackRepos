using System.Collections.Generic;
using System.Reflection;
using Acr.UserDialogs;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Widget;
using MvvmCross;
using MvvmCross.Converters;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.IoC;
using MvvmCross.Localization;
using MvvmCross.Platforms.Android.Presenters;
using MvvmCross.ViewModels;
using TestProject.Core;
using TestProject.Droid.Converters;

namespace TestProject.Droid
{
    public class Setup : MvxAppCompatSetup<TestProject.Core.App>
    {
        protected override IEnumerable<Assembly> AndroidViewAssemblies => new List<Assembly>(base.AndroidViewAssemblies)
        {
            typeof(NavigationView).Assembly,
            typeof(CoordinatorLayout).Assembly,
            typeof(FloatingActionButton).Assembly,
            typeof(Toolbar).Assembly,
            typeof(DrawerLayout).Assembly,
            typeof(ViewPager).Assembly,
            typeof(MvxRecyclerView).Assembly,
            typeof(MvxSwipeRefreshLayout).Assembly,
            typeof(RelativeLayout).Assembly,
            typeof(FrameLayout).Assembly,
            typeof(EditText).Assembly,
            typeof(Android.Support.V7.Widget.SwitchCompat).Assembly,
            typeof(LinearLayout).Assembly,
            typeof(CheckedTextView).Assembly,
            typeof(MvxValueConverter).Assembly
        };


        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            return new MvxAppCompatViewPresenter(AndroidViewAssemblies);
        }

        protected override IMvxApplication CreateApp()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();
            return base.CreateApp();
        }

        protected override void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            base.FillValueConverters(registry);
            registry.AddOrOverwrite("Color", new ColorValueConverter());
            registry.AddOrOverwrite("Language", new MvxLanguageConverter());
        }

    }
}
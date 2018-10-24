using MvvmCross;
using MvvmCross.Logging;

namespace TestProject.Core
{
    public static class Logs
    {
        public static IMvxLog Instance { get; } = Mvx.Resolve<IMvxLogProvider>().GetLogFor("TestProject");
    }
}
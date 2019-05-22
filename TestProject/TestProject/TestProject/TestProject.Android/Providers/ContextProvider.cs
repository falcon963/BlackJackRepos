using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TestProject.Droid.Providers.Interfaces;

namespace TestProject.Droid.Providers
{
    public class ContextProvider
        : IContextProvider
    {
        private Context _context;

        public ContextProvider(Context context)
        {
            _context = context;
        }

        public Context GetContext()
        {
            return _context;
        }
    }
}
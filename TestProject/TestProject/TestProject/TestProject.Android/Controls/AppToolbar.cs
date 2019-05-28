using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace TestProject.Droid.Controls
{
    public class AppToolbar
        : Toolbar
    {
        public AppToolbar(Context context) : base(context)
        {
            this.Click += HideSoftKeyboard;
        }

        public AppToolbar(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            this.Click += HideSoftKeyboard;
        }

        public AppToolbar(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            this.Click += HideSoftKeyboard;
        }

        protected AppToolbar(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            this.Click += HideSoftKeyboard;
        }

        public void HideSoftKeyboard(object sender, EventArgs e)
        {
            InputMethodManager close = (InputMethodManager)Context.GetSystemService(Context.InputMethodService);
            close.HideSoftInputFromWindow(this.WindowToken, 0);
        }
    }
}
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

namespace TestProject.Droid.Controls
{
    public class AppMainLinearLayout
        : LinearLayout
    {
        public AppMainLinearLayout(Context context) : base(context)
        {
            this.Click += HideSoftKeyboard;
        }

        public AppMainLinearLayout(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            this.Click += HideSoftKeyboard;
        }

        public AppMainLinearLayout(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            this.Click += HideSoftKeyboard;
        }

        public AppMainLinearLayout(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
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
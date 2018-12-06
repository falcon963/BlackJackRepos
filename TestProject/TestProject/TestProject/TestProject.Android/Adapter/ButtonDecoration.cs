using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace TestProject.Droid.Adapter
{
    public class ButtonDecoration
        :RecyclerView.ItemDecoration
    {
        private MyItemTouchHelper _myItemTouch = null;
        public ButtonDecoration(MyItemTouchHelper myItemTouch)
        {
            _myItemTouch = myItemTouch;
        }
        public override void OnDraw(Canvas c, RecyclerView parent, RecyclerView.State state)
        {
            _myItemTouch.OnDraw(c);
        }
    }
}
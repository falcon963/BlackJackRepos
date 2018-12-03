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

namespace TestProject.Droid.CustomEvents
{
    public class MySwipeEvent: EventArgs
    {
        public Int32 ItemId { get; private set; }

        private MySwipeEvent() { }

        public MySwipeEvent(Int32 itemId)
        {
            ItemId = itemId;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using UIKit;

namespace TestProject.iOS.MvxBindings
{
    public class NetworkIndicatorTargetBinding
        :MvxTargetBinding
    {
        private static List<NetworkIndicatorTargetBinding> CurrentBindings;

        public NetworkIndicatorTargetBinding(object target)
            :base(target)
        {

        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public override Type TargetType => typeof(UIViewController);

        public override void SetValue(object value)
        {
            var visible = (Boolean)value;

            if (CurrentBindings == null)
                CurrentBindings = new List<NetworkIndicatorTargetBinding>();

            if (visible)
            {
                CurrentBindings.Add(this);
                UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
            }
            else
            {
                if (CurrentBindings.Contains(this))
                    CurrentBindings.Remove(this);

                if (!CurrentBindings.Contains(this))
                {
                    UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
                    CurrentBindings = null;
                }
            }
        }
    }
}
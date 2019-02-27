using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace TestProject.iOS.CustomControls
{
    class BaseView:
        UIView
    {
        private Boolean _didSetupConstraints;

        public BaseView()
        {
            RunLifecycle();
        }

        protected void RunLifecycle()
        {
            CreateViews();

            SetNeedsUpdateConstraints();
        }

        protected virtual void CreateViews()
        {

        }

        public sealed override void UpdateConstraints()
        {
            if (!_didSetupConstraints)
            {
                CreateConstraints();

                _didSetupConstraints = true;
            }

            base.UpdateConstraints();
        }

        protected virtual void CreateConstraints()
        {

        }
    }
}
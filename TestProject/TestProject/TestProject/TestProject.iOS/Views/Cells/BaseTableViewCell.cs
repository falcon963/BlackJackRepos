using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace TestProject.iOS.Views.Cells
{
    public class BaseTableViewCell
        :MvxTableViewCell
    {
        private bool _didSetupConstraints;

        public BaseTableViewCell(IntPtr handle) : base(handle)
        {
            RunLifecycle();
        }

        private void RunLifecycle()
        {
            CreateView();

            SetNeedsUpdateConstraints();
        }

        public override sealed void UpdateConstraints()
        {
            if (!_didSetupConstraints)
            {
                CreateConstraints();

                _didSetupConstraints = true;
            }
            base.UpdateConstraints();
        }

        protected virtual void CreateView()
        {
        }

        protected virtual void CreateConstraints()
        {
        }
    }
}
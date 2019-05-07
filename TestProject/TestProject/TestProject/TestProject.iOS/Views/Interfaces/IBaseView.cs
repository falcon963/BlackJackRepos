using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace TestProject.iOS.Views.Interfaces
{
    public interface IBaseView
    {
        bool SetupBindings();
        bool SetupEvents();
        bool CustomizeViews();
    }
}
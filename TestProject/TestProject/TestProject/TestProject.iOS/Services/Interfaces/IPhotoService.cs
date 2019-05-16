using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace TestProject.iOS.Services.Interfaces
{
    public interface IPhotoService
    {
         event EventHandler<UIImagePickerController> PresentPicker;
         event EventHandler<UIAlertController> PresentAlert;
         event EventHandler<NSObject> ImagePickerDelegateSubscription;
    }
}
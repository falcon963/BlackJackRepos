using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using MvvmCross.ViewModels;
using TestProject.Core.Services.Interfaces;
using TestProject.Core.ViewModels;
using TestProject.iOS.Services.Interfaces;
using TestProject.iOS.Views;
using TestProject.LanguageResources;
using UIKit;

namespace TestProject.iOS.Services
{
    public class PhotoService
        : IImagePickerPlatformService, IPhotoService
    {
        #region fields

        private UIImagePickerController _imagePickerController;

        public event EventHandler<UIImagePickerController> PresentPicker;
        public event EventHandler<UIAlertController> PresentAlert;
        public event EventHandler<NSObject> ImagePickerDelegateSubscription;
        public event EventHandler<UIImagePickerController> DismissSubview;

        #endregion


        #region ctor

        public PhotoService()
        {
            _imagePickerController = new UIImagePickerController();
        }

        #endregion


        public async Task<byte[]> GetPhoto()
        {
            var image = await OpenImage();

            if(image == null)
            {
                return new byte[0];
            }

            using (NSData imageData = image.AsPNG())
            {
                byte[] imageByteArray = new byte[imageData.Length];

                System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, imageByteArray, 0, Convert.ToInt32(imageData.Length));

                return imageByteArray;
            }
        }

        public Task<UIImage> OpenImage()
        {
            ImagePickerDelegateSubscription?.Invoke(this, _imagePickerController.Delegate);

            TaskCompletionSource<UIImage> taskCompletionSource = new TaskCompletionSource<UIImage>();

            #region hendlers

            void Handle_FinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs e)
            {
                UIImage originalImage = e.Info[UIImagePickerController.EditedImage] as UIImage;

                taskCompletionSource.TrySetResult(originalImage);

                DismissSubview?.Invoke(this, _imagePickerController);

                _imagePickerController = new UIImagePickerController();
            }

            void Canceled(object sender, EventArgs e)
            {
                DismissSubview?.Invoke(this, _imagePickerController);

                _imagePickerController = new UIImagePickerController();

                taskCompletionSource.TrySetResult(null);
            }

            void OpenCamera()
            {
                if (UIImagePickerController.IsSourceTypeAvailable(UIImagePickerControllerSourceType.Camera))
                {
                    _imagePickerController.Canceled += Canceled;
                    _imagePickerController.SourceType = UIImagePickerControllerSourceType.Camera;
                    _imagePickerController.AllowsEditing = true;

                    PresentPicker?.Invoke(this, _imagePickerController);
                }
                if (!UIImagePickerController.IsSourceTypeAvailable(UIImagePickerControllerSourceType.Camera))
                {
                    var alert = UIAlertController.Create(Strings.Warning, Strings.YouDontHaveCamera, UIAlertControllerStyle.Alert);
                    alert.AddAction(UIAlertAction.Create(Strings.Ok, UIAlertActionStyle.Default, null));

                    PresentAlert?.Invoke(this, alert);
                }
            }


            void OpenLibrary()
            {
                _imagePickerController.Canceled += Canceled;
                _imagePickerController.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
                _imagePickerController.AllowsEditing = true;

                PresentPicker?.Invoke(this, _imagePickerController);
            }

            #endregion

            var actionSheet = UIAlertController.Create(Strings.PhotoSource, Strings.ChooseASource, UIAlertControllerStyle.ActionSheet);

            actionSheet.AddAction(UIAlertAction.Create(Strings.Gallery, UIAlertActionStyle.Default, (actionLibrary) =>
            {
                OpenLibrary();
            }));

            actionSheet.AddAction(UIAlertAction.Create(Strings.Camera, UIAlertActionStyle.Default, (actionCamera) =>
            {
                OpenCamera();
            }));

            actionSheet.AddAction(UIAlertAction.Create(Strings.Cancel, UIAlertActionStyle.Cancel, null));

            _imagePickerController.FinishedPickingMedia += Handle_FinishedPickingMedia;

            PresentAlert?.Invoke(this, actionSheet);

            return taskCompletionSource.Task;
        }

    }
}
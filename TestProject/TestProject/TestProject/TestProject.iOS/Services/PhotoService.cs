using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using TestProject.iOS.Views;
using TestProject.LanguageResources;
using UIKit;

namespace TestProject.iOS.Services
{
    public class PhotoService<T> where T
        : BaseMenuView
    {
        #region fields

        private readonly UIImagePickerController _imagePickerController;
        private readonly T _view;
        private UIImageView _image;

        #endregion


        #region ctor

        public PhotoService(T view, UIImageView image)
        {
            _view = view;
            _image = image;
            _imagePickerController = new UIImagePickerController();
            _imagePickerController.Delegate = view;
        }

        #endregion

        public void OpenImage()
        {
            var actionSheet = UIAlertController.Create(Strings.PhotoSource, Strings.ChooseASource, UIAlertControllerStyle.ActionSheet);

            actionSheet.AddAction(UIAlertAction.Create(Strings.Camera, UIAlertActionStyle.Default, (actionCamera) =>
            {
                OpenCamera();
            }));

            actionSheet.AddAction(UIAlertAction.Create(Strings.Gallery, UIAlertActionStyle.Default, (actionLibrary) =>
            {
                OpenLibrary();
            }));

            actionSheet.AddAction(UIAlertAction.Create(Strings.Cancel, UIAlertActionStyle.Cancel, null));

            _imagePickerController.FinishedPickingMedia += Handle_FinishedPickingMedia;

            _view.PresentViewController(actionSheet, true, null);
        }


        public void OpenCamera()
        {
            if (UIImagePickerController.IsSourceTypeAvailable(UIImagePickerControllerSourceType.Camera))
            {
                _imagePickerController.Canceled += (sender, e) => { _imagePickerController.DismissViewController(true, null); };
                _imagePickerController.SourceType = UIImagePickerControllerSourceType.Camera;
                _imagePickerController.AllowsEditing = true;
                _view.PresentViewController(_imagePickerController, true, null);
            }
            else
            {
                var alert = UIAlertController.Create(Strings.Warning, Strings.YouDontHaveCamera, UIAlertControllerStyle.Alert);
                alert.AddAction(UIAlertAction.Create(Strings.Ok, UIAlertActionStyle.Default, null));
                _view.PresentViewController(alert, true, null);
            }
        }


        public void OpenLibrary()
        {
            _imagePickerController.Canceled += (sender, e) => { _imagePickerController.DismissViewController(true, null); };
            _imagePickerController.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
            _imagePickerController.AllowsEditing = true;

            _view.PresentViewController(_imagePickerController, true, null);
        }

        public void Canceled(object sender, EventArgs e)
        {
            _imagePickerController.DismissViewController(true, null);
        }

        protected void Handle_FinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs e)
        {
            UIImage originalImage = e.Info[UIImagePickerController.EditedImage] as UIImage;

            if (originalImage != null)
            {
                _image.Image = originalImage;
            }

            _imagePickerController.DismissViewController(true, null);
        }

    }
}
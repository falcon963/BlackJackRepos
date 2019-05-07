using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using MobileCoreServices;
using MvvmCross.ViewModels;
using TestProject.Core.ViewModels;
using TestProject.iOS.Views;
using TestProject.LanguageResources;
using UIKit;

namespace TestProject.iOS.Services
{
    public class DocumentsService<TView, TViewModel> where TView
        : BaseView<TView, TViewModel>, new() where TViewModel : class, IMvxViewModel, IMvxNotifyPropertyChanged
    {
        #region Fields

        private readonly TView _view;

        private String[] _allowedUTIs;
        private NSUrl _documentURL;
        public Action<FileItemViewModel> SaveAction { get; set; }

        #endregion


        #region ctor

        public DocumentsService(TView view, Action<FileItemViewModel> saveAction)
        {
            _view = view;
            SaveAction = saveAction;

            #region Init UTIs
            _allowedUTIs = new string[] {
                    UTType.RTF,
                    UTType.PNG,
                    UTType.Text,
                    UTType.PDF,
                    UTType.JPEG
                };
            #endregion
        }

        #endregion


        #region Document picker's actions

        [Export("importFromDocPicker:")]
        public void ImportFromDocPicker(UIButton sender)
        {
            UIDocumentPickerViewController vc = new UIDocumentPickerViewController(_allowedUTIs, UIDocumentPickerMode.Import);
            vc.WasCancelled += OnPickerCancel;
            vc.DidPickDocument += DidPickDocumentForImport;
            _view.PresentViewController(vc, true, null);
        }

        private void DocumentPicker_DidPickDocumentAtUrls(object sender, UIDocumentPickedAtUrlsEventArgs e)
        {
            SaveFile(e.Urls.FirstOrDefault());
        }

        void SaveFile(NSUrl url)
        {
            var file = new FileItemViewModel();
            file.Content = System.IO.File.ReadAllBytes(url.Path);
            file.Name = System.IO.Path.GetFileNameWithoutExtension(url.Path);
            file.Extension = url.PathExtension;
            SaveAction(file);
        }

        void DidPickDocumentForImport(object sender, UIDocumentPickedEventArgs e)
        {
            NSUrl temporaryFileUrl = e.Url;
            PrintFileContent(temporaryFileUrl);
        }

        [Export("exportToDocPicker:")]
        public void ExportToDocPicker(UIButton sender)
        {
            if (TryShowFileNotExistsError())
                return;

            UIDocumentPickerViewController vc = new UIDocumentPickerViewController(_documentURL, UIDocumentPickerMode.ExportToService);
            vc.WasCancelled += OnPickerCancel;
            vc.DidPickDocument += DidPickDocumentForExport;

            _view.PresentViewController(vc, true, null);
        }

        void DidPickDocumentForExport(object sender, UIDocumentPickedEventArgs e)
        {
            NSUrl url = e.Url;
        }

        [Export("openDocPicker:")]
        public void OpenDocPicker(UIButton sender)
        {
            UIDocumentPickerViewController vc = new UIDocumentPickerViewController(_allowedUTIs, UIDocumentPickerMode.Open);
            vc.WasCancelled += OnPickerCancel;
            vc.DidPickDocument += DidPickDocumentForOpen;
            _view.PresentViewController(vc, true, null);
        }

        void DidPickDocumentForOpen(object sender, UIDocumentPickedEventArgs e)
        {

            var securityScopedUrl = e.Url;
            PrintOutsideFileContent(securityScopedUrl);
        }

        [Export("moveToDocPicker:")]
        private void MoveToDocPicker(UIButton sender)
        {
            if (TryShowFileNotExistsError())
                return;

            UIDocumentPickerViewController vc = new UIDocumentPickerViewController(_documentURL, UIDocumentPickerMode.MoveToService);
            vc.WasCancelled += OnPickerCancel;
            vc.DidPickDocument += DidPickDocumentForMove;
            _view.PresentViewController(vc, true, null);
        }

        void DidPickDocumentForMove(object sender, UIDocumentPickedEventArgs e)
        {
            NSUrl securityScopedUrl = e.Url;
            PrintOutsideFileContent(securityScopedUrl);
        }

        void PrintOutsideFileContent(NSUrl securityScopedUrl)
        {
            if (!securityScopedUrl.StartAccessingSecurityScopedResource())
                return;

            PrintFileContent(securityScopedUrl);

            securityScopedUrl.StopAccessingSecurityScopedResource();
        }

        void PrintFileContent(NSUrl url)
        {
            NSData data = null;
            NSError error = null;
            NSFileCoordinator fileCoordinator = new NSFileCoordinator();
            fileCoordinator.CoordinateRead(url, (NSFileCoordinatorReadingOptions)0, out error, newUrl => {
                data = NSData.FromUrl(newUrl);
            });
        }

        #endregion

        #region Documnet menu's actions

        [Export("importFromDocMenu:")]
        public void ImportFromDocMenu(UIButton sender)
        {
            UIDocumentMenuViewController vc = new UIDocumentMenuViewController(_allowedUTIs, UIDocumentPickerMode.Import);
            SetupDelegateThenPresent(vc, sender);
        }

        void SetupDelegateThenPresent(UIDocumentMenuViewController vc, UIButton button)
        {
            vc.WasCancelled += OnPickerSelectionCancel;
            vc.DidPickDocumentPicker += OnPickerPicked;

            vc.ModalPresentationStyle = UIModalPresentationStyle.Popover;
            _view.PresentViewController(vc, true, null);

            UIPopoverPresentationController presentationPopover = vc.PopoverPresentationController;
            if (presentationPopover != null)
            {
                presentationPopover.SourceView = _view.View;
                presentationPopover.PermittedArrowDirections = UIPopoverArrowDirection.Down;
                presentationPopover.SourceRect = button.Frame;
            }
        }

        #endregion

        #region Document menu's handlers

        void OnPickerSelectionCancel(object sender, EventArgs e)
        {
            var menu = (UIDocumentMenuViewController)sender;
            Unsibscribe(menu);
        }

        void OnPickerPicked(object sender, UIDocumentMenuDocumentPickedEventArgs e)
        {
            var menu = (UIDocumentMenuViewController)sender;
            Unsibscribe(menu);

            var documentPicker = e.DocumentPicker;
            documentPicker.WasCancelled += OnPickerCancel;
            switch (documentPicker.DocumentPickerMode)
            {
                case UIDocumentPickerMode.Import:
                    documentPicker.DidPickDocument += DidPickDocumentForImport;
                    documentPicker.DidPickDocumentAtUrls += DocumentPicker_DidPickDocumentAtUrls;
                    break;

                default:
                    break;
            }

            _view.PresentViewController(documentPicker, true, null);
        }

        #endregion

        void OnPickerCancel(object sender, EventArgs e)
        {

        }

        bool TryShowFileNotExistsError()
        {
            if (NSFileManager.DefaultManager.FileExists(_documentURL.Path))
                return false;

            UIAlertController alert = UIAlertController.Create(_documentURL.LastPathComponent, Strings.FileDoesntExistMaybeYouMovedOrExportedItEarlieReRunTheApp, UIAlertControllerStyle.Alert);
            alert.AddAction(UIAlertAction.Create(Strings.Ok, UIAlertActionStyle.Default, null));
            _view.PresentViewController(alert, true, null);
            return true;
        }

        void Unsibscribe(UIDocumentMenuViewController menu)
        {
            menu.WasCancelled -= OnPickerSelectionCancel;
            menu.DidPickDocumentPicker -= OnPickerPicked;
        }

    }
}
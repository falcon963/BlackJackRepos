using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using MobileCoreServices;
using MvvmCross.ViewModels;
using TestProject.Core.Services.Interfaces;
using TestProject.Core.ViewModels;
using TestProject.iOS.Views;
using TestProject.LanguageResources;
using UIKit;
using IDocumentPickerService = TestProject.iOS.Services.Interfaces.IDocumentPickerService;

namespace TestProject.iOS.Services
{
    public class DocumentsService
        : IDocumentPickerPlatformService, IDocumentPickerService
    {
        #region Fields

        private String[] _allowedUTIs;

        public event EventHandler<UIDocumentPickerViewController> PresentedDocumentPicker;
        public event EventHandler<UIDocumentMenuViewController> PresentedMenuDocumentPicker;

        #endregion


        #region ctor

        public DocumentsService()
        {

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

        FileItemViewModel CreateFile(NSUrl url)
        {
 
            var file = new FileItemViewModel();
            file.Content = System.IO.File.ReadAllBytes(url.Path);
            file.Name = System.IO.Path.GetFileNameWithoutExtension(url.Path);
            file.Extension = url.PathExtension;
            return file;         
        }

        #endregion

        #region Documnet menu's actions

        [Export("importFromDocMenu:")]
        public async Task<FileItemViewModel> ImportFromDocMenu()
        {
            UIDocumentMenuViewController vc = new UIDocumentMenuViewController(_allowedUTIs, UIDocumentPickerMode.Import);
            var file = await SetupDelegateThenPresent(vc);
            return file;
        }

        Task<FileItemViewModel> SetupDelegateThenPresent(UIDocumentMenuViewController vc)
        {
            TaskCompletionSource<FileItemViewModel> taskCompletionSource = new TaskCompletionSource<FileItemViewModel>();

            void OnPickerSelectionCancel(object sender, EventArgs e)
            {
                var menu = (UIDocumentMenuViewController)sender;
                menu.WasCancelled -= OnPickerSelectionCancel;
                menu.DidPickDocumentPicker -= OnPickerPicked;

                taskCompletionSource.TrySetResult(null);
            }

            void OnPickerPicked(object sender, UIDocumentMenuDocumentPickedEventArgs e)
            {
                var menu = (UIDocumentMenuViewController)sender;
                menu.WasCancelled -= OnPickerSelectionCancel;
                menu.DidPickDocumentPicker -= OnPickerPicked;

                var documentPicker = e.DocumentPicker;
                documentPicker.WasCancelled += (s1, e1) => {
                    taskCompletionSource.TrySetResult(null);
                };

                switch (documentPicker.DocumentPickerMode)
                {
                    case UIDocumentPickerMode.Import:
                        documentPicker.DidPickDocumentAtUrls += (s, eventArgs) => {

                            var file = CreateFile(eventArgs.Urls.FirstOrDefault());

                            taskCompletionSource.TrySetResult(file);
                        };
                        break;

                    default:
                        break;
                }

                PresentedDocumentPicker?.Invoke(this, documentPicker);
            }

            vc.WasCancelled += OnPickerSelectionCancel;

            vc.DidPickDocumentPicker += OnPickerPicked;

            vc.ModalPresentationStyle = UIModalPresentationStyle.Popover;

            PresentedMenuDocumentPicker?.Invoke(this, vc);

            UIPopoverPresentationController presentationPopover = vc.PopoverPresentationController;
            if (presentationPopover != null)
            {
                presentationPopover.PermittedArrowDirections = UIPopoverArrowDirection.Down;
            }

            return taskCompletionSource.Task;
        }

        #endregion

    }
}
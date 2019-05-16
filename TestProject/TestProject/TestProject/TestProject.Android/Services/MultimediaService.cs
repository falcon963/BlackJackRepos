using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TestProject.Droid.Services.Interfaces;
using Path = System.IO.Path;
using File = Java.IO.File;
using Uri = Android.Net.Uri;
using PopupMenu = Android.Support.V7.Widget.PopupMenu;
using TestProject.Droid.Fragments;
using Android.Graphics;
using Android.Util;
using TestProject.LanguageResources;
using MvvmCross;
using TestProject.Droid.Helpers.Interfaces;
using TestProject.Core.Services.Interfaces;
using System.Threading.Tasks;
using TestProject.Droid.Fragments.Interfaces;
using TestProject.Droid.Models;

namespace TestProject.Droid.Services
{
    public class MultimediaService<T>: IImagePickerPlatformService where T : BaseFragment, IFragmentLifecycle
    {

        private const int REQUEST_CAMERA = 0;

        private const int REQUEST_GALLERY = 1;

        private const int RESULT_CODE_OK = -1;

        private const int RESULT_CODE_CANCEL = 0;

        private readonly T _fragment;
        private View _imageView;

        private IUriHelper _uriHelper;

        public Uri _imageUri;

        public MultimediaService(T fragment, View imageView)
        {
            _fragment = fragment;
            _imageView = imageView;
            _uriHelper = Mvx.IoCProvider.Resolve<IUriHelper>();
        }

        public Task<byte[]> GetPhoto()
        {
            TaskCompletionSource<byte[]> taskCompletionSource = new TaskCompletionSource<byte[]>();

            void OnMenuItemClicked(object sender, PopupMenu.MenuItemClickEventArgs e)
            {
                var label = e.Item.TitleFormatted.ToString();

                if (label == Strings.Camera)
                {
                    var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
                    string time = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    string name = $"Test_Project_{time}.jpg";
                    var filePath = Path.Combine(sdCardPath, name);

                    File image = new File(filePath);
                    _imageUri = Uri.FromFile(image);

                    var intent = new Intent(MediaStore.ActionImageCapture);
                    intent.PutExtra(MediaStore.ExtraOutput, _imageUri);

                    _fragment.StartActivityForResult(intent, REQUEST_CAMERA);
                }

                if (label == Strings.Gallery)
                {
                    var intent = new Intent(Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri);
                    intent.SetType("image/*");

                    _fragment.StartActivityForResult(Intent.CreateChooser(intent, Strings.SelectPicture), REQUEST_GALLERY);
                }

                if (label == Strings.Cancel)
                {

                }

                
            }

            void ActivityResult(object sender1, ResultEventArgs args)
            {
                if (args?.ResultCode == RESULT_CODE_CANCEL)
                {
                    taskCompletionSource.TrySetResult(null);
                }

                if (args?.ResultCode == RESULT_CODE_OK
                    && args?.RequestCode == REQUEST_CAMERA)
                {
                    Bitmap bitmapImage = BitmapFactory.DecodeFile(_imageUri.Path);

                    var imageBytes = GetImageByteArray(bitmapImage);

                    taskCompletionSource.TrySetResult(imageBytes);
                }

                if (args?.ResultCode == RESULT_CODE_OK
                    && args?.RequestCode == REQUEST_GALLERY)
                {
                    string realPath = _uriHelper.GetRealPathFromURI(args?.Data?.Data, _fragment);

                    Bitmap bitmapImage = BitmapFactory.DecodeFile(realPath);

                    var imageBytes = GetImageByteArray(bitmapImage);

                    taskCompletionSource.TrySetResult(imageBytes);
                }
            }

            _fragment.SubscribeOnResult += ActivityResult;

            var popup = new PopupMenu(_fragment.Activity, _imageView);

            popup.Menu.Add(Strings.Camera);
            popup.Menu.Add(Strings.Gallery);
            popup.Menu.Add(Strings.Cancel);
            popup.MenuItemClick += OnMenuItemClicked;
            popup.Show();

            return taskCompletionSource.Task;
        }

        public byte[] GetImageByteArray(Bitmap image)
        {
            if (image == null)
            {
                return null;
            }

            byte[] bitmapData;

            using (var stream = new MemoryStream())
            {
                image.Compress(Bitmap.CompressFormat.Png, 80, stream);
                bitmapData = stream.ToArray();
            }

            return bitmapData;
        }
    }
}
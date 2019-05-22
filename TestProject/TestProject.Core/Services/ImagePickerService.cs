using MvvmCross;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Services.Interfaces;

namespace TestProject.Core.Services
{
    public class ImagePickerService
        : IImagePickerService
    {
        public async Task<string> GetImageBase64()
        {
            Mvx.IoCProvider.TryResolve(out IImagePickerPlatformService _imagePicker);

            if (_imagePicker == null)
            {
                return string.Empty;
            }

            var imageBytes = await _imagePicker.GetPhoto();
            string imageString = Convert.ToBase64String(imageBytes);
            return imageString;
        }
    }
}

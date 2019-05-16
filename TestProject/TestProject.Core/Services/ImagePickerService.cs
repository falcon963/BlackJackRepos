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
        private readonly IImagePickerPlatformService _imagePicker;

        public ImagePickerService(IImagePickerPlatformService imagePicker)
        {
            _imagePicker = imagePicker;
        }

        public async Task<string> GetImageBase64()
        {
            var imageBytes = await _imagePicker.GetPhoto();
            string imageString = Convert.ToBase64String(imageBytes);
            return imageString;
        }
    }
}

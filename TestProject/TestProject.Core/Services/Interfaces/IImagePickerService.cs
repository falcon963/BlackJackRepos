using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Core.Services.Interfaces
{
    public interface IImagePickerService
    {
        Task<string> GetImageBase64();
    }
}

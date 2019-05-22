using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TestProject.Core.Services.Interfaces;
using TestProject.Core.ViewModels;

namespace TestProject.Droid.Services
{
    public class DocumentPickerPlatformService
        : IDocumentPickerPlatformService
    {
        public Task<FileItemViewModel> ImportFromDocMenu()
        {
            throw new NotImplementedException();
        }
    }
}
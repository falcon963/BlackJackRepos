using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Models;
using TestProject.Core.ViewModels;

namespace TestProject.Core.Services.Interfaces
{
    public interface IDocumentPickerService
    {
        Task<FileItemViewModel> GetPickedFile();
        void SaveFile(TaskFileModel file);
    }
}

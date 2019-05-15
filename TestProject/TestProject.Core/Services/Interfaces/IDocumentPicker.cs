using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.ViewModels;

namespace TestProject.Core.Services.Interfaces
{
    public interface IDocumentPicker
    {
        Task<FileItemViewModel> ImportFromDocMenu();
    }
}

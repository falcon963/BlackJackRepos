using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Models;
using TestProject.Core.Repositories.Interfaces;
using TestProject.Core.Services.Interfaces;
using TestProject.Core.ViewModels;

namespace TestProject.Core.Services
{
    public class DocumentPickerService
        : IDocumentPickerService
    {
        private readonly IDocumentPickerPlatformService _documentPicker;
        private readonly IFileRepository _fileRepository;
        private readonly IFileService _fileService;

        public DocumentPickerService(IDocumentPickerPlatformService documentPicker, IFileRepository fileRepository, IFileService fileService)
        {
            _documentPicker = documentPicker;
            _fileRepository = fileRepository;
            _fileService = fileService;
        }

        public async Task<FileItemViewModel> GetPickedFile()
        {
            var file = await _documentPicker.ImportFromDocMenu();

            return file;
        }

        public void SaveFile(TaskFileModel file)
        {
            _fileService.SaveFile(file);
        }
    }
}

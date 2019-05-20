using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Models;
using TestProject.Core.Repositories.Interfaces;
using TestProject.Core.Services.Interfaces;

namespace TestProject.Core.Services
{
    public class FileService
        : IFileService
    {
        private readonly IFileRepository _fileRepository;

        public FileService(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }
        public int SaveFile(TaskFileModel file)
        {
            int id;

            if (file.Id == 0)
            {
                id = _fileRepository.Save(file);

                return id;
            }

            id = _fileRepository.Update(file);

            return id;
        }
    }
}

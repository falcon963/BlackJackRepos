using MvvmCross;
using MvvmCross.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Providers;
using TestProject.Core.Providers.Interfacies;
using TestProject.Core.Models;
using TestProject.Core.Repositories.Interfaces;

namespace TestProject.Core.Repositories
{
    public class FileRepository
        : BaseRepository<TaskFileModel>, IFileRepository
    {
        public FileRepository(IDatabaseConnectionProvider dbConnection) : base(dbConnection)
        {
        }

        IEnumerable<TaskFileModel> IFileRepository.GetFiles(int taskId)
        {
            IEnumerable<TaskFileModel> files = _dbConnection.Database.Table<TaskFileModel>().Where(i => i.TaskId == taskId);

            return files;
        }
    }
}

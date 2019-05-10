using MvvmCross;
using MvvmCross.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.DBConnection;
using TestProject.Core.DBConnection.Interfacies;
using TestProject.Core.Models;
using TestProject.Core.Repositories.Interfaces;

namespace TestProject.Core.Repositories
{
    public class FileRepository
        : BaseRepository<TaskFileModel>, IFileRepository
    {
        public FileRepository(IDatabaseConnectionService dbConnection) : base(dbConnection)
        {
        }

        IEnumerable<TaskFileModel> IFileRepository.GetFiles(int taskId)
        {
            IEnumerable<TaskFileModel> files = _dbConnection.Database.Table<TaskFileModel>().Where(i => i.TaskId == taskId);

            return files;
        }
    }
}

using MvvmCross;
using MvvmCross.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.DBConnection;
using TestProject.Core.DBConnection.Interfacies;
using TestProject.Core.Models;
using TestProject.Core.Repositories.Interfacies;

namespace TestProject.Core.Repositories
{
    public class FileRepository
        : BaseRepository<TaskFileModel>, IFileRepository
    {
        public FileRepository(IDatabaseConnectionService dbConnection) : base(dbConnection)
        {
        }

        IEnumerable<TaskFileModel> IFileRepository.GetRange(int id)
        {
            List<TaskFileModel> listOfFile = _dbConnection.Database.Table<TaskFileModel>().Where(i => i.TaskId == id).ToList();
            return listOfFile;
        }
    }
}

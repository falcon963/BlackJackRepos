using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.DBConnection;
using TestProject.Core.Interfaces;
using TestProject.Core.Models;

namespace TestProject.Core.Services
{
    public class FileService
        : IFileService
    {
        private readonly SqliteAppConnection _dbConnection;

        public FileService(IDatabaseConnectionService connectionService)
        {
            _dbConnection = new SqliteAppConnection(connectionService);
        }

        public void AddAllFile(List<TaskFileModel> files)
        {
            _dbConnection.Database.InsertAll(files);
        }

        public Int32 AddFile(TaskFileModel file)
        {
            return _dbConnection.Database.Insert(file);
        }

        public void DeleteAllFile(List<TaskFileModel> files)
        {
            Int32 count = 0;
            foreach(TaskFileModel file in files)
            {
                _dbConnection.Database.Delete(file);
                count++;
            }
            Console.WriteLine(count + "files deleted.");
        }

        public void DeleteFile(int id)
        {
            var file = _dbConnection.Database.Table<TaskFileModel>().Where(i => i.Id == id).FirstOrDefault();
            _dbConnection.Database.Delete(file);
        }

        public List<TaskFileModel> TakeAllTaskFiles(int taskId)
        {
            List<TaskFileModel> listOfFile = _dbConnection.Database.Table<TaskFileModel>().Where(i => i.TaskId == taskId).ToList();
            return listOfFile;
        }
    }
}

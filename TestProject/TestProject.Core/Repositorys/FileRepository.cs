using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.DBConnection;
using TestProject.Core.Interfaces;
using TestProject.Core.Models;
using TestProject.Core.Repositorys.Interfaces;

namespace TestProject.Core.Repositorys
{
    public class FileRepository
        : IBaseRepository<TaskFileModel>, IFileRepository
    {
        private SqliteAppConnection DBConnection { get; set; }

        public FileRepository(IDatabaseConnectionService connectionService)
        {
            DBConnection = new SqliteAppConnection(connectionService);
        }

        public void SaveAllFile(List<TaskFileModel> files)
        {
            DBConnection.Database.InsertAll(files);
        }

        public void Save(TaskFileModel item)
        {
            if (item.Id == 0)
            {
                DBConnection.Database.Insert(item);
            }
            if (item.Id != 0)
                DBConnection.Database.Update(item);
        }

        public void Delete(TaskFileModel item)
        {
            DBConnection.Database.Delete(item);
        }

        public void DeleteById(int id)
        {
            var file = DBConnection.Database.Table<TaskFileModel>().Where(i => i.Id == id).FirstOrDefault();
            DBConnection.Database.Delete(file);
        }

        public void DeleteAllFile(List<TaskFileModel> files)
        {
            int count = 0;
            foreach(TaskFileModel file in files)
            {
                DBConnection.Database.Delete(file);
                count++;
            }
            Console.WriteLine(count + "files deleted.");
        }

        public TaskFileModel GetDate(int id)
        {
            return DBConnection.Database.Table<TaskFileModel>().Where(i => i.TaskId == id).FirstOrDefault();
        }

        public List<TaskFileModel> GetAllTaskFiles(int taskId)
        {
            List<TaskFileModel> listOfFile = DBConnection.Database.Table<TaskFileModel>().Where(i => i.TaskId == taskId).ToList();
            return listOfFile;
        }
    }
}

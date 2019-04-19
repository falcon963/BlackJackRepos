using MvvmCross;
using MvvmCross.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.DBConnection;
using TestProject.Core.Interfacies;
using TestProject.Core.Models;
using TestProject.Core.Repositories.Interfacies;

namespace TestProject.Core.Repositories
{
    public class FileRepository
        : IBaseRepository<TaskFileModel>, IFileRepository
    {
        private SqliteAppConnection DBConnection { get; set; }

        public FileRepository(IDatabaseConnectionService connectionService)
        {
            DBConnection = new SqliteAppConnection(connectionService);
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

        public void Delete(int id)
        {
            var file = DBConnection.Database.Table<TaskFileModel>().Where(i => i.Id == id).FirstOrDefault();
            DBConnection.Database.Delete(file);
        }

        public TaskFileModel Get(int id)
        {
            return DBConnection.Database.Table<TaskFileModel>().Where(i => i.TaskId == id).FirstOrDefault();
        }

        public void SaveRange(List<TaskFileModel> list)
        {
            DBConnection.Database.InsertAll(list);
        }

        public void DeleteRange(List<TaskFileModel> list)
        {
            int count = 0;
            foreach (TaskFileModel file in list)
            {
                DBConnection.Database.Delete(file);
                count++;
            }
            var log = Mvx.IoCProvider.Resolve<IMvxLog>();
            log.Trace(count.ToString());
        }

        public List<TaskFileModel> GetRange(int taskId)
        {
            List<TaskFileModel> listOfFile = DBConnection.Database.Table<TaskFileModel>().Where(i => i.TaskId == taskId).ToList();
            return listOfFile;
        }
    }
}

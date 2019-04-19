using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Models;
using TestProject.Core.Interfacies;
using SQLite;
using System.Linq;
using MvvmCross;
using TestProject.Core.ViewModels;
using TestProject.Core.DBConnection;
using System.Threading;
using TestProject.Core.Repositories.Interfacies;
using MvvmCross.Logging;

namespace TestProject.Core.Repositories
{
    public class TasksRepository 
        : ITasksRepository 
    {

        private SqliteAppConnection DBConnection { get; set; }

        public TasksRepository(IDatabaseConnectionService connectionService)
        {
            DBConnection = new SqliteAppConnection(connectionService);
        }

        public void SwipeTaskDelete(UserTask item)
        {
            DBConnection.Database.Delete(item);
        }

        public List<int> GetUserTasksIdAsync(int userId)
        {
            List<UserTask> listOfTasks = DBConnection.Database.Table<UserTask>().Where(i => i.UserId == userId).ToList();
            List<int> listId = listOfTasks.Select(i => i.Id).ToList();
            return listId;
        }

        public void Save(UserTask item)
        {
            if (item.Id != 0)
            {
                DBConnection.Database.Update(item);
            }
            DBConnection.Database.Insert(item);
        }

        public void Delete(UserTask item)
        {
            DBConnection.Database.Delete(item);
        }

        public void Delete(int id)
        {
            var task = DBConnection.Database.Table<UserTask>().Where(i => i.UserId == id).FirstOrDefault();
            DBConnection.Database.Delete(task);
        }

        public UserTask Get(int id)
        {
            return DBConnection.Database.Table<UserTask>().Where(i => i.Id == id).FirstOrDefault();
        }

        public int SaveTask(UserTask item)
        {
            if (item.Id != 0)
            {
                return DBConnection.Database.Update(item);
            }
            return DBConnection.Database.Insert(item);
        }

        public List<UserTask> GetUserTasks(int userId)
        {
            return DBConnection.Database.Table<UserTask>().Where(i => i.UserId == userId).ToList();
        }

        public void SaveRange(List<UserTask> list)
        {
            DBConnection.Database.Insert(list);
        }

        public void DeleteRange(List<UserTask> list)
        {
            int count = 0;
            foreach (UserTask file in list)
            {
                DBConnection.Database.Delete(file);
                count++;
            }
            var log = Mvx.IoCProvider.Resolve<IMvxLog>();
            log.Trace(count.ToString() + "task deleted.");
        }

        public List<UserTask> GetRange(int userId)
        {
            List<UserTask> listOfTasks = DBConnection.Database.Table<UserTask>().Where(i => i.UserId == userId).ToList();
            return listOfTasks;
        }
    }
}

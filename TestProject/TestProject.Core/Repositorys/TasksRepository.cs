using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Models;
using TestProject.Core.Interfaces;
using SQLite;
using System.Linq;
using MvvmCross;
using TestProject.Core.ViewModels;
using TestProject.Core.DBConnection;
using System.Threading;
using TestProject.Core.Repositorys.Interfaces;

namespace TestProject.Core.Repositorys
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

        public List<int> RefreshUserTasks(int userId)
        {
            return GetUserTasksIdAsync(userId);
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

        public void DeleteById(int id)
        {
            var task = DBConnection.Database.Table<UserTask>().Where(i => i.UserId == id).FirstOrDefault();
            DBConnection.Database.Delete(task);
        }

        public UserTask GetDate(int id)
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
    }
}

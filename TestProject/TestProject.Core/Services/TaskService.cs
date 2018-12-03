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

namespace TestProject.Core.Services
{
    public class TaskService : ITasksRepository 
    {

        private readonly SqliteAppConnection _dbConnection;

        public TaskService(IDatabaseConnectionService connectionService)
        {
            _dbConnection = new SqliteAppConnection(connectionService);
        }

        public void SwipeTaskDelete(UserTask item)
        {
            _dbConnection.Database.Delete(item);
        }

        public List<Int32> GetUserTasksIdAsync(Int32 userId)
        {
            List<UserTask> listOfTasks = _dbConnection.Database.Table<UserTask>().Where(i => i.UserId == userId).ToList();
            List<Int32> listId = listOfTasks.Select(i => i.Id).ToList();
            return listId;
        }

        public UserTask GetUserTaskAsync(Int32 taskId)
        {
            return _dbConnection.Database.Table<UserTask>().Where(i => i.Id == taskId).FirstOrDefault();
        }

        public Int32 SaveUserTaskAsync(UserTask userTask)
        {
            if(userTask.Id != 0)
            {
                return _dbConnection.Database.Update(userTask);
            }
            return _dbConnection.Database.Insert(userTask);
        }

        public Int32 DeleteUserTaskAsync(UserTask userTask)
        {
            return _dbConnection.Database.Delete(userTask);
        }

        public List<Int32> RefreshUserTasks(Int32 userId)
        {
            return GetUserTasksIdAsync(userId);
        }

    }
}

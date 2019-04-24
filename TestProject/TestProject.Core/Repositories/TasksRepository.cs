using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Models;
using SQLite;
using System.Linq;
using MvvmCross;
using TestProject.Core.ViewModels;
using TestProject.Core.DBConnection;
using System.Threading;
using TestProject.Core.Repositories.Interfaces;
using MvvmCross.Logging;
using TestProject.Core.DBConnection.Interfacies;

namespace TestProject.Core.Repositories
{
    public class TasksRepository 
        : BaseRepository<UserTask>, ITasksRepository
    {
        public TasksRepository(IDatabaseConnectionService dbConnection) : base(dbConnection)
        {
        }

        IEnumerable<int> ITasksRepository.GetUserTasksIdAsync(int userId)
        {
            List<UserTask> listOfTasks = _dbConnection.Database.Table<UserTask>().Where(i => i.UserId == userId).ToList();

            List<int> listId = listOfTasks.Select(i => i.Id).ToList();

            return listId;
        }

        IEnumerable<UserTask> ITasksRepository.GetTasksList(int userId)
        {
            List<UserTask> listOfTasks = _dbConnection.Database.Table<UserTask>().Where(i => i.UserId == userId).ToList();

            return listOfTasks;
        }

        int ITasksRepository.Save(UserTask item)
        {
            int itemId;

            if (item.Id == 0)
            {
                itemId = _dbConnection.Database.Insert(item);

                return itemId;
            }

            itemId = _dbConnection.Database.Update(item);

            return itemId;
        }

        public UserTask Get(int taskId)
        {
            var user = _dbConnection.Database.Table<UserTask>().FirstOrDefault(v => v.Id == taskId);

            return user;
        }
    }
}

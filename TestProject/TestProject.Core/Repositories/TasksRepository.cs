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
using TestProject.Core.Repositories.Interfacies;
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

        IEnumerable<int> ITasksRepository.GetUserTasksIdAsync(int id)
        {
            List<UserTask> listOfTasks = _dbConnection.Database.Table<UserTask>().Where(i => i.UserId == id).ToList();
            List<int> listId = listOfTasks.Select(i => i.Id).ToList();
            return listId;
        }

        IEnumerable<UserTask> ITasksRepository.GetRange(int id)
        {
            List<UserTask> listOfTasks = _dbConnection.Database.Table<UserTask>().Where(i => i.UserId == id).ToList();
            return listOfTasks;
        }

        int ITasksRepository.Save(UserTask item)
        {
            if (item.Id == 0)
            {
                return _dbConnection.Database.Insert(item);
            }
                return _dbConnection.Database.Update(item);
        }

        public UserTask Get(int id)
        {
            return _dbConnection.Database.Table<UserTask>().FirstOrDefault(v => v.Id == id);
        }
    }
}

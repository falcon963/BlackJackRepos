using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Models;
using TestProject.Core.Interfaces;
using SQLite;
using MvvmCross;

namespace TestProject.Core.Services
{
    public class TaskService : ITaskService 
    {

        private SQLiteAsyncConnection database;

        public TaskService(IDatabaseConnectionService connectionService)
        {
            database = connectionService.DbConnection();
        }

        public Task<List<UserTask>> GetTasksAsync()
        {
            return database.Table<UserTask>().ToListAsync();
        }

        public Task<UserTask> GetTaskAsync(Int32 id)
        {
            return database.Table<UserTask>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<Int32> SaveTaskAsync(UserTask userTask)
        {
            return userTask.Id != 0 ? database.UpdateAsync(userTask) : database.InsertAsync(userTask);
        }

        public Task<Int32> DeleteTaskAsync(UserTask userTask)
        {
            return database.DeleteAsync(userTask);
        }

        public Task<List<BaseUserTask>> GetCustomUserTasks()
        {
            return new Task<List<BaseUserTask>>(GetCusromListUserTask);
        }

        private List<BaseUserTask> GetCusromListUserTask()
        {
            var result = new List<BaseUserTask>();
            result.Add(new UserTask
            {
                Id = 1,
                Title = "Just do it!",
                Note = "You can do this bro!",
                Status = false
            });
            result.Add(new UserTask
            {
                Id = 2,
                Title = "Keep calm!",
                Note = "It is easy for you!",
                Status = false
            });
            result.Add(new UserTask
            {
                Id = 3,
                Title = "Dont worry, be happy!",
                Note = "Good luck, have fan!",
                Status = false
            });
            return result;
        }
    }
}

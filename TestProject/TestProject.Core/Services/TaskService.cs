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

        public async Task<List<UserTask>> RefreshUserTasks()
        {
            return await GetTasksAsync();
        }

    }
}

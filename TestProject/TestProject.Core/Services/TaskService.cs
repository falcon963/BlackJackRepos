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

namespace TestProject.Core.Services
{
    public class TaskService : ITaskService 
    {

        private SQLiteAsyncConnection database;

        public TaskService(IDatabaseConnectionService connectionService)
        {
            database = connectionService.DbConnection();
            database.CreateTableAsync<UserTask>();
            database.CreateTableAsync<User>();
        }

        public void SwipeTaskDelete(TaskListViewModel item)
        {
            database.DeleteAsync(item);
        }

        public async Task<List<UserTask>> GetTasksAsync(Int32 id)
        {
            return await database.Table<UserTask>().Where(i => i.UserId == id).ToListAsync();
        }


        public async Task<Int32> SaveTaskAsync(UserTask userTask)
        {
            return userTask.Id != 0 ? await database.UpdateAsync(userTask) : await database.InsertAsync(userTask);
        }

        public async Task<Int32> DeleteTaskAsync(UserTask userTask)
        {
            return await database.DeleteAsync(userTask);
        }

        public async Task<List<UserTask>> RefreshUserTasks(Int32 id)
        {
            return await GetTasksAsync(id);
        }

        public async Task<User> CheckAccountAccess(String login, String password)
        {
            var user = await database.Table<User>().Where(v => v.Login == login && v.Password == password).FirstOrDefaultAsync();

            return user;
        }

        public async Task<Boolean> CheckValidLogin(String login)
        {

            var result = await database.Table<User>().Where(v => v.Login == login).FirstOrDefaultAsync();

            return result == null ? true : false;

        }

        public async Task<Boolean> CreateUser(User user)
        {
            await database.InsertAsync(user);
            return await database.Table<User>().Where(v => v.Login == user.Login && v.Password == user.Password).FirstAsync() != null ? true : false;
        }


    }
}

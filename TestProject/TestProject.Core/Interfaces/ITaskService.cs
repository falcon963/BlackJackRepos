using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Models;

namespace TestProject.Core.Interfaces
{
    public interface ITaskService
    {
        Task<List<UserTask>> GetTasksAsync();
        Task<UserTask> GetTaskAsync(Int32 id);
        Task<Int32> SaveTaskAsync(UserTask userTask);
        Task<Int32> DeleteTaskAsync(UserTask userTask);
        Task<List<BaseUserTask>> GetCustomUserTasks();
    }
}

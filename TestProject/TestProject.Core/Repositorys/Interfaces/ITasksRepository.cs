using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Models;
using TestProject.Core.ViewModels;

namespace TestProject.Core.Repositorys.Interfaces
{
    public interface ITasksRepository
        : IBaseRepository<UserTask>
    {
        List<int> GetUserTasksIdAsync(int id);
        List<UserTask> GetUserTasks(int userId);
        List<int> RefreshUserTasks(int id);
        void SwipeTaskDelete(UserTask item);
        int SaveTask(UserTask item);
    }
}

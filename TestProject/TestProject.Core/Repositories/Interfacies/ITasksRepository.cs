using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Models;
using TestProject.Core.ViewModels;

namespace TestProject.Core.Repositories.Interfacies
{
    public interface ITasksRepository
        : IBaseRepository<UserTask>
    {
        List<int> GetUserTasksIdAsync(int id);
        List<UserTask> GetUserTasks(int userId);
        void SwipeTaskDelete(UserTask item);
        int SaveTask(UserTask item);
    }
}

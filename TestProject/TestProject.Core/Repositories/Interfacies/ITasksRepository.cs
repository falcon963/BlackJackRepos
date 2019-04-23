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
        IEnumerable<int> GetUserTasksIdAsync(int id);
        IEnumerable<UserTask> GetRange(int id);
        int Save(UserTask id);
        UserTask Get(int id);
    }
}

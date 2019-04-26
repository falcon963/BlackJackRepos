using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Models;
using TestProject.Core.ViewModels;

namespace TestProject.Core.Repositories.Interfaces
{
    public interface ITasksRepository
        : IBaseRepository<UserTask>
    {

        IEnumerable<UserTask> GetTasksList(int userId);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Models;
using TestProject.Core.ViewModels;

namespace TestProject.Core.Interfaces
{
    public interface ITasksRepository
    {
        List<Int32> GetUserTasksIdAsync(Int32 id);
        UserTask GetUserTaskAsync(Int32 taskId);
        Int32 SaveUserTaskAsync(UserTask userTask);
        Int32 DeleteUserTaskAsync(UserTask userTask);
        List<Int32> RefreshUserTasks(Int32 id);
        void SwipeTaskDelete(UserTask item);
    }
}

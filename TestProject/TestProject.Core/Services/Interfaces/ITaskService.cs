using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Models;

namespace TestProject.Core.Services.Interfaces
{
    public interface ITaskService
    {
        int SaveTask(UserTask task);
    }
}

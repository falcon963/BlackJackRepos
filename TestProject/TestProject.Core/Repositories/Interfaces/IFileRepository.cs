using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Models;

namespace TestProject.Core.Repositories.Interfaces
{
    public interface IFileRepository
        : IBaseRepository<TaskFileModel>
    {
        IEnumerable<TaskFileModel> GetFilesList(int taskId);
    }
}

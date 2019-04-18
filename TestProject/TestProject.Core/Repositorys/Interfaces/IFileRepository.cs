using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Models;

namespace TestProject.Core.Repositorys.Interfaces
{
    public interface IFileRepository
        : IBaseRepository<TaskFileModel>
    {
        void SaveAllFile(List<TaskFileModel> files);
        void DeleteAllFile(List<TaskFileModel> files);
        List<TaskFileModel> GetAllTaskFiles(int taskId);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Models;

namespace TestProject.Core.Interfaces
{
    public interface IFileService
    {
        Int32 AddFile(TaskFileModel file);
        void DeleteFile(Int32 id);
        void AddAllFile(List<TaskFileModel> files);
        void DeleteAllFile(List<TaskFileModel> files);
        List<TaskFileModel> TakeAllTaskFiles(Int32 taskId);
    }
}

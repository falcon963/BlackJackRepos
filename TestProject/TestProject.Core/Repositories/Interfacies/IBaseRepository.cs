using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Models;

namespace TestProject.Core.Repositories.Interfacies
{
    public interface IBaseRepository<T> where T : BaseModel
    {
        void Save(T item);
        void Delete(T item);
        void Delete(int id);
        void SaveRange(List<T> list);
        void DeleteRange(List<T> list);
        List<T> GetRange(int id);
        T Get(int id);
    }
}

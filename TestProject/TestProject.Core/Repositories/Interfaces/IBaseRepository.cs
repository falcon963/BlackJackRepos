using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Models;

namespace TestProject.Core.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T : BaseModel
    {
        int Save(T item);
        int Update(T item);
        void Delete(T item);
        void Delete(int id);
        void SaveRange(List<T> list);
        void DeleteRange(List<T> list);
        T Get(int id);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject.Core.Repositorys.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        void Save(T item);
        void Delete(T item);
        void DeleteById(int id);
        T GetDate(int id);
    }
}

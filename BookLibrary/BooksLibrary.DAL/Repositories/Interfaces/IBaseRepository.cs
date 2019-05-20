using BooksLibrary.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BooksLibrary.DAL.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T: BaseEntity
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
    }
}

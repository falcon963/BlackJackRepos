using MvvmCross;
using MvvmCross.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Providers;
using TestProject.Core.Providers.Interfacies;
using TestProject.Core.Models;
using TestProject.Core.Repositories.Interfaces;

namespace TestProject.Core.Repositories
{
    public class BaseRepository<T>
        : IBaseRepository<T> where T : BaseModel, new()
    {

        protected readonly SqliteAppConnectionProvider _dbConnection;

        public BaseRepository(IDatabaseConnectionProvider dbConnection)
        {
            _dbConnection = new SqliteAppConnectionProvider(dbConnection);
        }

        public void Delete(T item)
        {
            _dbConnection.Database.Delete(item);
        }

        public void Delete(int id)
        {
            var item = _dbConnection.Database.Table<T>().FirstOrDefault(i => i.Id == id);

            Delete(item);
        }

        public void DeleteRange(List<T> list)
        {
            foreach (T item in list)
            {
                Delete(item);
            }
        }

        public int Save(T item)
        {
            int id = _dbConnection.Database.Insert(item);

            return id;
        }

        public void SaveRange(List<T> list)
        {
            foreach(T item in list)
            {
                Save(item);
            }
        }

        public T Get(int id)
        {
            var user = _dbConnection.Database.Table<T>().FirstOrDefault(v => v.Id == id);

            return user;
        }

        public int Update(T item)
        {
            int id = _dbConnection.Database.Update(item);
            return id;
        }
    }
}

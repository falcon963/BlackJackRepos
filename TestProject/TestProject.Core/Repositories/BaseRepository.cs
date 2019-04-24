using MvvmCross;
using MvvmCross.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.DBConnection;
using TestProject.Core.DBConnection.Interfacies;
using TestProject.Core.Models;
using TestProject.Core.Repositories.Interfaces;

namespace TestProject.Core.Repositories
{
    public class BaseRepository<T>
        : IBaseRepository<T> where T : BaseModel, new()
    {

        public SqliteAppConnection _dbConnection;

        public BaseRepository(IDatabaseConnectionService dbConnection)
        {
            _dbConnection = new SqliteAppConnection(dbConnection);
        }

        public void Delete(T item)
        {
            _dbConnection.Database.Delete(item);
        }

        public void Delete(int id)
        {
            var file = _dbConnection.Database.Table<T>().FirstOrDefault(i => i.Id == id);
            _dbConnection.Database.Delete(file);
        }

        public void DeleteRange(List<T> list)
        {
            foreach (T file in list)
            {
                _dbConnection.Database.Delete(file);
            }
        }

        public void Save(T item)
        {
            if (item.Id == 0)
            {
                _dbConnection.Database.Insert(item);
                return;
            }
               _dbConnection.Database.Update(item);
        }

        public void SaveRange(List<T> list)
        {
            foreach(T item in list)
            {
                if (item.Id == 0)
                {
                    _dbConnection.Database.Insert(item);
                }
                if (item.Id != 0)
                {
                    _dbConnection.Database.Update(item);
                }
            }
        }
    }
}

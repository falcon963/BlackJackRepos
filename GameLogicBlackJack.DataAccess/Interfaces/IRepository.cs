using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.DataAccess.Entities;

namespace GameLogicBlackJack.DataAccess.Interfaces
{
    public interface IRepository<T> where T: BaseEntities
    {
        void Create(T item);
        void Delete(Int32 id);
        T Get(Int32 id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Func<T, Boolean> predicate);
    }
}

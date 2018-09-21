using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.DataAccess.Entities;

namespace GameLogicBlackJack.DataAccess.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<PlayerDAL> Players { get; }
        IRepository<BotDAL> Bots { get; }
        IRepository<DealerDAL> Dealers { get; }
        IRepository<GameDAL> Games { get; }
    }
}

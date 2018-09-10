using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GameLogicBlackJack.MVC.Models
{
    public class GameContex : DbContext
    {
        public DbSet<GameData> Game { get}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.Models;
using GameLogicBlackJack.Controllers;
using GameLogicBlackJack.DataAccess.SQLite;
using GameLogicBlackJack.Interface;
using GameLogicBlackJack.Services;
using GameLogicBlackJack.DataAccess.Repositories;

namespace GameLogicBlackJack
{
    class Program
    {
        
        static void Main(string[] args)
        {
         
            GameController controllers = new GameController();
          //  GameService.GetInstance(controllers.unit).BotAdd();
            Console.WriteLine("1 - Load\n2 - AllPlayers\n3 - Delete Account\n4 - New Account\n Esc - Close program.");
            GameService.GetInstance(controllers.unit).Launcher(controllers);
            Console.WriteLine("Game End");
            Console.ReadKey();
        }
    }
}

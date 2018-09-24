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
using GameLogicBlackJack.DataAccess.Interfaces;

namespace GameLogicBlackJack
{
    class Program
    {
        
        static void Main(string[] args)
        {
            GameController controllers = new GameController();
            controllers.GameInitialize();
            controllers.ConsoleChoise();
            Console.WriteLine("Game End");
            Console.ReadKey();
        }
    }
}

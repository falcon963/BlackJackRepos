using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.Controllers;
using GameLogicBlackJack.GameLogic;
using GameLogicBlackJack.Controllers;

namespace GameLogicBlackJack.View
{
    

    public class GameConsole
    {
        public static void ConsolePlayerEnterNickname()
        {
            Console.WriteLine("Enter your nickname: ");
        }
        public static void PlayerEnterBalance()
        {
            Console.WriteLine("Enter your balance: ");
        }
        public static void ConsolePlayerEnterNumberOfBots()
        {
            Console.WriteLine(@"How much bots do you want {0 - 5}?");
        }
        public static void ConsolePlayerEnterBet()
        {
            Console.WriteLine("{1} balance: {0}$.", Game.Player.PlayerBalance, Game.Player.PlayerName);
            Console.WriteLine(@"How much bet do you want (0 - {0})?", Game.Player.PlayerBalance);
        }

        public static void PlayerInfo()
        {
            Console.WriteLine("You cards: {0}, {1}. You score: {2}",
                Game.Player.playerHand.ElementAt(0).CardFace + " " + Game.Player.playerHand.ElementAt(0).CardSuit,
                Game.Player.playerHand.ElementAt(1).CardFace + " " + Game.Player.playerHand.ElementAt(1).CardSuit, Game.Player.TotalValue);
            Console.WriteLine("Dealer score: {0}. Dealer cards: {1}, {2}", 
                Game.Dealer.TotalValue, Game.Dealer.dealerHand.ElementAt(0).CardFace + " " + Game.Dealer.dealerHand.ElementAt(0).CardSuit,
                Game.Dealer.dealerHand.ElementAt(1).CardFace + " " + Game.Dealer.dealerHand.ElementAt(1).CardSuit);
        }

        public static void PlayerMakeChoise()
        {
            Console.WriteLine("Do you want take card {0}. Press SPACE if want, or ENTER if want continue", Game.Player.PlayerName);
        }
    }
}

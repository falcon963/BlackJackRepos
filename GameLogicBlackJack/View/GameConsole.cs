using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.Controllers;
using GameLogicBlackJack.GameLogic;

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
            Console.WriteLine("You score: {2}. You cards: {0}, {1}.",
                Game.Player.playerHand.ElementAt(0).CardFace + " " + Game.Player.playerHand.ElementAt(0).CardSuit,
                Game.Player.playerHand.ElementAt(1).CardFace + " " + Game.Player.playerHand.ElementAt(1).CardSuit, Game.Player.TotalValue);
            Console.WriteLine("Dealer score: {0}. Dealer cards: {1}, {2}", 
                Game.Dealer.TotalValue, Game.Dealer.dealerHand.ElementAt(0).CardFace + " " + Game.Dealer.dealerHand.ElementAt(0).CardSuit,
                Game.Dealer.dealerHand.ElementAt(1).CardFace + " " + Game.Dealer.dealerHand.ElementAt(1).CardSuit);
        }

        public static void Score()
        {
            Console.WriteLine("You score: {0}.", Game.Player.TotalValue);
            Console.WriteLine("Dealer score: {0}.", Game.Dealer.TotalValue);
        }

        public static void PlayerTakeCard()
        {
            Console.WriteLine("You take {0}.", Game.Player.playerHand.Last().CardFace + " " + Game.Player.playerHand.Last().CardSuit);
        }

        public static void DealerTakeCard()
        {
            Console.WriteLine("Dealer take {0}.", Game.Dealer.dealerHand.Last().CardFace + " " + Game.Dealer.dealerHand.Last().CardSuit);
        }

        public static void PlayerMakeChoise()
        {
            Console.WriteLine("Do you want take card {0}? Press SPACE if want, or ENTER if want continue", Game.Player.PlayerName);
        }

        public static void PlayerLose()
        {
            Console.WriteLine("{0} lose!", Game.Player.PlayerName);
        }

        public static void PlayerWon()
        {
            Console.WriteLine("{0} won!", Game.Player.PlayerName);
        }

        public static void PlayerDraw()
        {
            Console.WriteLine("Diller and {0} played in a draw!", Game.Player.PlayerName);
        }

        public static void PlayerWonBlackJack()
        {
            Console.WriteLine("{0} won! {0} has Black Jack!", Game.Player.PlayerName);
        }

        public static void PlayerLoseBlackJack()
        {
            Console.WriteLine("{0} lose! Dealer has Black Jack!", Game.Player.PlayerName);
        }

        public static void PlayerDrawBlackJack()
        {
            Console.WriteLine("Diller and {0} have Black Jack, it is draw!", Game.Player.PlayerName);
        }
        public static void ContinueOrStopGame()
        {
            Console.WriteLine("If you want continue game, press N. Else press Escape.");
        }
        public static void EndGame()
        {
            Console.WriteLine("Game was stoped. {0} balance: {1}.", Game.Player.PlayerName, Game.Player.PlayerBalance);
        }
        public static void BustGame()
        {
            Console.WriteLine("Game was stoped. You dont have money. {0} balance: {1}.\n ..............Press Escape..............", Game.Player.PlayerName, Game.Player.PlayerBalance);
        }
      /*  public static void BotsInfo()
        {
            Console.WriteLine("Bot{0} cards: {1}, {2}", Game.Bot.BotId, Game.Bot.botHand.ElementAt(0).CardSuit + " " + Game.Bot.botHand.ElementAt(0).CardFace,
                Game.Bot.botHand.ElementAt(1).CardSuit + " " + Game.Bot.botHand.ElementAt(1).CardFace);
        }*/
    }
}

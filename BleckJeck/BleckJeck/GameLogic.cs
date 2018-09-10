using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlackJack
{
    class GameLogic
    {
        
        Diller diller1 = new Diller();
        static Deck deck = new Deck();
        static List<AllCards> userHand;
        static List<AllCards> dillerHand;
       
        static User user = new User() { PlayerName = "Men", PlayerBalance = 1000 };
        static NewGame game = new NewGame();
        static Int32 betAmount;

     

     
            public static void Hand()
        {

            if (deck.GetAmountOfRemeiningCard() < 20)
            {
                deck.Initialize();
            }

            Console.WriteLine("Cards: {0}", deck.GetAmountOfRemeiningCard());
            
           
            Console.WriteLine("Your balance {0}, is:  {1}$", user.PlayerName, user.PlayerBalance);
            Console.WriteLine("Your can bet (1 - {0})", user.PlayerBalance);
            String input = Console.ReadLine().Trim().Replace(" ", "");


            while (!Int32.TryParse(input, out betAmount) || betAmount < 1 || betAmount > user.PlayerBalance)
            {
                Console.WriteLine("Input error.\nTry again:");
                input = Console.ReadLine().Trim().Replace(" ", "");
            }

            userHand = new List<AllCards>();
            userHand.Add(deck.DrowACard());
            userHand.Add(deck.DrowACard());

          //  Int32 totalUserCardsValue = 0;
            Boolean acePlayer = true;
            foreach (AllCards card in userHand)//исправить чтоб при получении туза повторно он был равен 1 +
            {

                if (card.Face == Face.Ace && acePlayer)
                {
                    card.Value += 10; 
                    acePlayer = false;
                    break;
                }
           
               // totalUserCardsValue += card.Value;
            }
            
            Console.WriteLine("[{0}]", user.PlayerName);
            Console.WriteLine("1 card: {0} {1}", userHand[0].Face, userHand[0].Suit);
            Console.WriteLine("2 card: {0} {1}", userHand[1].Face, userHand[1].Suit);
            Console.WriteLine("Your score: {0}\n", userHand[0].Value + userHand[1].Value);

            dillerHand = new List<AllCards>();
            dillerHand.Add(deck.DrowACard());
            dillerHand.Add(deck.DrowACard());

           // Int32 totalDillerCardValue = 0;
            Boolean aceDiller = true;

            foreach (AllCards card in dillerHand)//исправить чтоб при получении туза повторно он был равен 1 +
            {
                if (card.Face == Face.Ace && aceDiller)
                {
                    card.Value += 10;
                    aceDiller = false;
                    break;
                }
               
               // totalDillerCardValue += card.Value;
            }

            Console.WriteLine("[Diller]");
            Console.WriteLine("1 card: {0} {1}", dillerHand[0].Face, dillerHand[0].Suit);
            Console.WriteLine("2 card: {0} {1}", dillerHand[1].Face, dillerHand[1].Suit);
            Console.WriteLine("Diller score: {0}\n", dillerHand[0].Value+dillerHand[1].Value);
            Int32 totalDillerCardValue = dillerHand[0].Value + dillerHand[1].Value;
            Int32 totalUserCardsValue = userHand[0].Value + userHand[1].Value;

            if ( totalDillerCardValue == 21 && totalUserCardsValue == 21)
            {
                Console.WriteLine("The draw");
                
                user.PlayerBalance += betAmount;
            }else if(totalDillerCardValue == 21 && totalUserCardsValue != 21)
                {
                    Console.WriteLine("Your lose");
               
                user.PlayerBalance -= betAmount;
                return;//1
                }
                else if(totalUserCardsValue == 21 && totalDillerCardValue != 21)
                    {
                        Console.WriteLine("Your win.");
               
                user.PlayerBalance += betAmount;
                return;//2
            }
           

            do
            {
                Console.WriteLine("Would you like to take another card ? If yes, press Y, if not press N.");
                ConsoleKeyInfo userChose = Console.ReadKey(true);
                while (userChose.Key != ConsoleKey.Y && userChose.Key != ConsoleKey.N)
                {
                    Console.WriteLine("No command, press again");
                    userChose = Console.ReadKey(true);
                }
                switch (userChose.Key)
                {
                    case ConsoleKey.Y:
                        userHand.Add(deck.DrowACard());
                        Console.WriteLine("Your take a card {0} {1}", userHand[userHand.Count - 1].Face, userHand[userHand.Count - 1].Suit);
                        totalUserCardsValue += userHand[userHand.Count - 1].Value;
                        Console.WriteLine("Your score is: {0}\n", totalUserCardsValue);
                        if (totalUserCardsValue > 21)
                        {
                            Console.WriteLine("Your lose\n");
                            user.PlayerBalance -= betAmount;
                            return;
                        }
                        else if (totalUserCardsValue == 21)
                        {
                            Console.WriteLine("Your have Black Jack!");
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                      break;
                
                    case ConsoleKey.N:
                        Console.WriteLine("[Diller]");
                        Console.WriteLine("1 card: {0} {1}", dillerHand[0].Face, dillerHand[0].Suit);
                        Console.WriteLine("2 card: {0} {1}", dillerHand[1].Face, dillerHand[1].Suit);
                        Console.WriteLine("Diller score: {0}\n", totalDillerCardValue);

                        while (totalDillerCardValue < 17)
                        {
                            dillerHand.Add(deck.DrowACard());
                            Console.WriteLine("Diller take a card {0}, {1} {2}", dillerHand.Count, dillerHand[dillerHand.Count - 1].Face, dillerHand[dillerHand.Count - 1].Suit);
                            totalDillerCardValue += dillerHand[dillerHand.Count - 1].Value;
                            Console.WriteLine("Diller score {0}", totalDillerCardValue);

                            if (totalDillerCardValue > 21)
                            {
                                Console.WriteLine("Your win!");
                                user.PlayerBalance += betAmount;
                            }
                            else
                            {
                                if (totalDillerCardValue > totalUserCardsValue)
                                {
                                    Console.WriteLine("Diller has {0}, {1} has {2}. Diller win!", totalDillerCardValue, user.PlayerName, totalUserCardsValue);

                                    user.PlayerBalance -= betAmount;
                                    return;
                                }
                                else if(totalDillerCardValue == totalUserCardsValue)
                                {
                                    Console.WriteLine("Diller has {0}, {1} has {2}. Standoff!", totalDillerCardValue, user.PlayerName, totalUserCardsValue);

                                    user.PlayerBalance += betAmount;
                                    return;
                                }
                                else
                                {
                                    Console.WriteLine("Diller has {0}, {1} has {2}. {1} win!", totalDillerCardValue, user.PlayerName, totalUserCardsValue);

                                    user.PlayerBalance += betAmount;
                                    return;
                                }
                            }
                        } break;
                       default:
                        break;
                        
                }
                Console.ReadLine();
            }while(true);

            
        }
         
    }
}

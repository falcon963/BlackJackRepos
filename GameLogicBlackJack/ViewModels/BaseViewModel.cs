using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.BusinessLogic.Models;

namespace GameLogicBlackJack.ViewModels
{
    public class BaseViewModel
    {
        public List<Card> hand = new List<Card>();
        public Int32 Id { get; set; }
    }
}

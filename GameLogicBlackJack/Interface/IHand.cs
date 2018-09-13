using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.GameLogic;

namespace GameLogicBlackJack.Interface
{
    public interface IHand
    {
        List<Card> Hand { get; set; } 
    }
}

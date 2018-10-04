using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogicBlackJack.ViewModels
{
    public class GameViewModel
    {
        public Int32 GameId { get; set; }
        public PlayerViewModel Player { get; set; }
        public DealerViewModel Dealer { get; set; }
        public List<BotViewModel> bots = new List<BotViewModel>();
        public Int32 Bet { get; set; }
        public Boolean PlayerWon { get; set; }
        public Boolean PlayerDraw { get; set; }

        public GameViewModel()
        {
            Player = new PlayerViewModel();
            Dealer = new DealerViewModel();
        }
    }
}

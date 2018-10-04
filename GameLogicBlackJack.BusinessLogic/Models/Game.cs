﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GameLogicBlackJack.BusinessLogic.Enums;


namespace GameLogicBlackJack.BusinessLogic.Models
{


    public class Game
    {
        private GameAction allowedActions;
        private GameState lastState;
        public Deck deck = new Deck();
        public Int32 GameId { get; set; }
        public Player Player { get; set; }
        public Dealer Dealer { get; set; }
        public List<Bot> bots = new List<Bot>();
        public GameAction AllowedActions { get; set; }
        public GameState LastState { get; set; }
        public Int32 Bet { get; set; }
        public Boolean PlayerWon { get; set; }
        public Boolean PlayerDraw { get; set; }


        public Game()
        {
            Player = new Player();
            Dealer = new Dealer();
            this.LastState = GameState.Unknown;
            this.AllowedActions = GameAction.None;
        }


    
}   }

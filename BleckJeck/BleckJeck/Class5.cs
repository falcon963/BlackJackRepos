using System;


    interface INewGame : IDiller, IPlayerAcount, IBots
    {
        Int32 GameId { get; set; }
        Int32 GameBank { get; set; }
        Int32 NumberOfPlayers { get; set; }
        //String GameWinner { get; set; }
    }

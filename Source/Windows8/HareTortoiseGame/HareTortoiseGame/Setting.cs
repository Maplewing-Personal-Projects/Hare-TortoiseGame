using System;
using HareTortoiseGame.Component;

namespace HareTortoiseGame
{
    public static class Setting
    {
        #region Field
        static int _maxPly = 12;
        static Board.Player[] _players = { Board.Player.User, Board.Player.Computer };
        #endregion

        #region Property
        public static int MaxPly { get { return _maxPly; } set { _maxPly = value; } }
        public static Board.Player[] Players { get { return _players; } set { _players = value; } }
        #endregion

        #region Constructor
        #endregion
    }
}
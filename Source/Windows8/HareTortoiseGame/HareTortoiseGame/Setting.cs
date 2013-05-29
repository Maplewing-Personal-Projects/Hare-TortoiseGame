using System;
using HareTortoiseGame.Component;
using HareTortoiseGame.GameLogic;

namespace HareTortoiseGame
{
    public static class Setting
    {
        #region Field
        static int _maxPlyOrSecond = 12;
        static Board.Player[] _players = { Board.Player.User, Board.Player.Computer };
        static int _maxEdgeCount = 4;
        #endregion

        #region Property
        public static int MaxPly { get { return _maxPlyOrSecond; } set { _maxPlyOrSecond = value; } }
        public static Board.Player[] Players { get { return _players; } set { _players = value; } }
        public static int MaxEdgeCount { 
            get { return _maxEdgeCount; } 
            set { 
                _maxEdgeCount = value;

                BoardData.Row = new ulong[_maxEdgeCount];
                ulong row = 0;
                for (int i = 0; i < _maxEdgeCount; ++i)
                {
                    row <<= 1;
                    row |= 1;
                }
                for (int i = 0; i < _maxEdgeCount; ++i)
                {
                    BoardData.Row[i] = row;
                    row <<= _maxEdgeCount;
                }

                BoardData.Column = new ulong[_maxEdgeCount];
                ulong column = 0;
                for (int i = 0; i < _maxEdgeCount; ++i)
                {
                    column <<= Setting.MaxEdgeCount;
                    column |= 1;
                }
                for (int i = 0; i < _maxEdgeCount; ++i)
                {
                    BoardData.Column[i] = column;
                    column <<= 1;
                }
            } 
        }
        #endregion

        #region Constructor
        #endregion
    }
}
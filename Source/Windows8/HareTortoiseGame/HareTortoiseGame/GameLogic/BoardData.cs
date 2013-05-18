using System;
using HareTortoiseGame.Component;

namespace HareTortoiseGame.GameLogic
{
    public class BoardData
    {

        #region Properties

        public int Hare { set; get; }
        public int Tortoise { set; get; }

        #endregion

        #region Methods

        public BoardData( int hare = 0x0111, int tortoise = 0xE000 )
        {
            Hare = hare;
            Tortoise = tortoise;
        }

        static public int GetOneChessPosition(int board)
        {
            int position = 0;
            if (board == 0) return position;
            while ((board & 0x1) == 0)
            {
                board >>= 1;
                ++position;
            }
            return position;
        }

        #endregion
    }
}
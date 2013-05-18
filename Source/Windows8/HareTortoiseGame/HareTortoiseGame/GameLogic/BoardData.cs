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

        public BoardData(Chess[] tortoise, Chess[] hare)
        {
            Tortoise = 0;
            for (int i = 0; i < 3; ++i)
            {
                Tortoise |= 1 << (tortoise[i].Y * 4 + tortoise[i].X);
            }

            Hare = 0;
            for (int i = 0; i < 3; ++i)
            {
                Hare |= 1 << (hare[i].Y * 4 + hare[i].X);
            }
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
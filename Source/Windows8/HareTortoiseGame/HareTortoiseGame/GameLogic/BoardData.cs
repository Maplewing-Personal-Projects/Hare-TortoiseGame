using System;
using HareTortoiseGame.Component;

namespace HareTortoiseGame.GameLogic
{
    public class BoardData
    {

        public static int[] Row = { 0x000F, 0x00F0, 0x0F00, 0xF000 };
        public static int[] Column = { 0x1111, 0x2222, 0x4444, 0x8888 };

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
                if( !tortoise[i].Finish ) Tortoise |= 1 << (tortoise[i].Y * 4 + tortoise[i].X);
            }

            Hare = 0;
            for (int i = 0; i < 3; ++i)
            {
                if( !hare[i].Finish ) Hare |= 1 << (hare[i].Y * 4 + hare[i].X);
            }
        }

        public bool TerminalTest()
        {
            return (Tortoise == 0 || Hare == 0);
        }

        public int HaveChess()
        {
            return Hare | Tortoise;
        }

        public int Empty()
        {
            return ~HaveChess();
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
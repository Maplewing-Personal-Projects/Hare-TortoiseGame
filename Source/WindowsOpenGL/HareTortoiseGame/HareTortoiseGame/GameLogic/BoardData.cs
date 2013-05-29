using System;
using HareTortoiseGame.Component;

namespace HareTortoiseGame.GameLogic
{
    public class BoardData
    {
        #region Field
        public static ulong[] Row = { 0x000F, 0x00F0, 0x0F00, 0xF000 };
        public static ulong[] Column = { 0x1111, 0x2222, 0x4444, 0x8888 };
        public ulong Hare;
        public ulong Tortoise;
        #endregion

        #region Method

        public BoardData( ulong hare = 0, ulong tortoise = 0 )
        {
            Hare = hare;
            if (hare == 0)
            {
                for (int i = 0; i < Setting.MaxEdgeCount - 1; ++i)
                {
                    Hare <<= Setting.MaxEdgeCount;
                    Hare |= 1;
                }
            }

            Tortoise = tortoise;
            if (Tortoise == 0)
            {
                for (int i = 0; i < Setting.MaxEdgeCount - 1; ++i)
                {
                    Tortoise <<= 1;
                    Tortoise |= 1;
                }
                Tortoise <<= (Setting.MaxEdgeCount * Setting.MaxEdgeCount) - (Setting.MaxEdgeCount - 1);
            }
        }

        public BoardData(Chess[] tortoise, Chess[] hare)
        {
            Tortoise = 0;
            for (int i = 0; i < Setting.MaxEdgeCount - 1; ++i)
            {
                if (!tortoise[i].GoalArrived) Tortoise |= 1u << (tortoise[i].Y * Setting.MaxEdgeCount + tortoise[i].X);
            }

            Hare = 0;
            for (int i = 0; i < Setting.MaxEdgeCount - 1; ++i)
            {
                if (!hare[i].GoalArrived) Hare |= 1u << (hare[i].Y * Setting.MaxEdgeCount + hare[i].X);
            }
        }

        public bool TerminalTest()
        {
            return (Tortoise == 0 || Hare == 0);
        }

        public ulong HaveChess()
        {
            return Hare | Tortoise;
        }

        public ulong Empty()
        {
            return ~HaveChess();
        }

        static public int GetOneChessPosition(ulong board)
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
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

        public BoardData( int hare = 0x8880, int tortoise = 0x0007 )
        {
            Hare = hare;
            Tortoise = tortoise;
        }
        
        #endregion
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using HareTortoiseGame.State;
using HareTortoiseGame.GameLogic;

namespace HareTortoiseGame.Component
{
    public class ChessButton : GraphComponent
    {
        #region Field
        
        #endregion

        #region Property
        public bool HaveChess { get; set; }
        public int WantToGo { get; set; }
        public Chess.Action WantToGoAction { get; set; }
        #endregion

        #region Constructor

        public ChessButton(Game game, Texture2D picture, DrawState state)
            : base(game, picture, state)
        {
            HaveChess = false;
            WantToGo = -1;
        }

        #endregion
    }
}

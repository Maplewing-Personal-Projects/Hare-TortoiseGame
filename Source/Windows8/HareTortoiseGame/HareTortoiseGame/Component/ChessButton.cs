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

        #region Constructor

        public ChessButton(Game game, Texture2D picture, DrawState state)
            : base(game, picture, state)
        {
        }

        #endregion
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using HareTortoiseGame.State;
using HareTortoiseGame.GameLogic;

namespace HareTortoiseGame.Component
{
    public class Board : GraphComponent
    {
        #region Field

        BoardData _boardData = new BoardData();

        #endregion

        #region Constructor

        public Board(Game game, Texture2D picture, DrawState state, BoardData boardData = null)
            : base(game, picture, state)
        {
            if( boardData != null ) _boardData = boardData;
        }

        public override void Draw(GameTime gameTime)
        {

        }

        #endregion
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HareTortoiseGame.State;
using HareTortoiseGame.Component;

namespace HareTortoiseGame.PackageScene
{
    public class GameBoardScene : Scene
    {
        #region Field

        Board _board;

        #endregion
        
        #region Constructor

        public GameBoardScene(Game game)
            : base(game,
                game.Content.Load<Texture2D>("blank"),
                new DrawState(game, new Vector4(2f, 0f, 1f, 1f), Color.Gray))
        {
            _board = new Board(game, game.Content.Load<Texture2D>("blank"),
                new DrawState(game, new Vector4(0.35f, 0.1f, 0.6f, 0.8f), Color.Gray));
            AddComponent(_board);
        }

        public override void Initialize()
        {
            _state.AddState(2.0f, new DrawState(Game, new Vector4(0, 0, 1, 1), Color.Gray));
            base.Initialize();
        }

        #endregion

    }
}

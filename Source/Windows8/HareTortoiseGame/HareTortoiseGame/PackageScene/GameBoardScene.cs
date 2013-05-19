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
        Container _panel;

        #endregion
        
        #region Constructor

        public GameBoardScene(Game game)
            : base(game,
                game.Content.Load<Texture2D>("blank"),
                new DrawState(game, new Vector4(2f, 0f, 1f, 1f), Color.Gray))
        {
            _board = new Board(game, game.Content.Load<Texture2D>("blank"),
                new DrawState(game, new Vector4(0.65f, 0.5f, 0f, 0f), Color.Gray));
            _board.AddState(0.5f, new DrawState(game, new Vector4(0.65f, 0.5f, 0f, 0f), Color.Gray));
            _board.AddState(0.5f, new DrawState(game, new Vector4(0.35f, 0.1f, 0.6f, 0.8f), Color.Gray));
            _panel = new Container(game, game.Content.Load<Texture2D>("blank"),
                new DrawState(game, new Vector4(0f, 0f, 0f, 1.0f), new Color(0.0f, 0.0f, 0.0f, 0.3f)));
            _panel.AddState(0.5f, new DrawState(game, new Vector4(0f, 0f, 0f, 1.0f), new Color(0.0f, 0.0f, 0.0f, 0.3f)));
            _panel.AddState(0.5f, new DrawState(game, new Vector4(0f, 0f, 0.3f, 1.0f), new Color(0.0f, 0.0f, 0.0f, 0.3f)));
            AddComponent(_board);
            AddComponent(_panel);
        }

        #endregion

        #region Method

        public override void Initialize()
        {
            _state.AddState(0.5f, new DrawState(Game, new Vector4(0, 0, 1, 1), Color.Gray));
            base.Initialize();
        }

        #endregion

    }
}

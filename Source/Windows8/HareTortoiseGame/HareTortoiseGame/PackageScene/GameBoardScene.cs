using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HareTortoiseGame.State;
using HareTortoiseGame.Component;

namespace HareTortoiseGame.PackageScene
{
    public class GameBoardScene : Scene
    {

        #region Constructor

        public GameBoardScene(Game game)
            : base(game,
                game.Content.Load<Texture2D>("blank"),
                new DrawState(game, new Vector4(0.5f, 0.5f, 0, 0), Color.White))
        {

        }

        public override void Initialize()
        {
            _state.AddState(5.0f, new DrawState(Game, new Vector4(0, 0, 1, 1), Color.White));
            base.Initialize();
        }

        #endregion

    }
}

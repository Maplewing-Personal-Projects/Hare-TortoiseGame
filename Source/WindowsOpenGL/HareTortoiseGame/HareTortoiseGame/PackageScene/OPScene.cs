using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HareTortoiseGame.State;
using HareTortoiseGame.Component;

namespace HareTortoiseGame.PackageScene
{
    public class OPScene : Scene
    {

        #region Constructor

        public OPScene(Game game)
            : base(game, 
                game.Content.Load<Texture2D>("blank"), 
                new DrawState(game, new Vector4(0.5f, 0.5f, 0, 0), Color.White))
        {
            
        }

        #endregion

        #region Method

        public override void Initialize()
        {
            _state.AddState(1.0f, new DrawState(Game, new Vector4(0, 0, 1, 1), Color.White));
            base.Initialize();
        }

        #endregion

    }
}

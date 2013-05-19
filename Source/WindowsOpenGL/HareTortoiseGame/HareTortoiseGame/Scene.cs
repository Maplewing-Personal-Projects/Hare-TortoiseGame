using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HareTortoiseGame.State;
using HareTortoiseGame.Component;

namespace HareTortoiseGame
{
    public class Scene : Container
    {
        #region Field

        public string NextScene = null;

        #endregion

        #region Constructor

        public Scene(Game game, Texture2D background, DrawState state)
            : base(game, background, state)
        {
        }

        #endregion

    }
}

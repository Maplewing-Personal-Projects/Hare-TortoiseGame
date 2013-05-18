using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using HareTortoiseGame.Manager;
using HareTortoiseGame.State;

namespace HareTortoiseGame.Component
{
    public class BasicComponent : DrawableGameComponent
    {
        #region Field

        public Viewport PreviousBounds;

        #endregion

        #region Constructor

        public BasicComponent(Game game)
            : base(game)
        {
            PreviousBounds = game.GraphicsDevice.Viewport;
        }

        public virtual void Start()
        {
            Game.Components.Add(this);
        }

        public virtual void End()
        {
            if (Game.Components.Contains(this))
            {
                Game.Components.Remove(this);
            }
        }

        #endregion
    }
}

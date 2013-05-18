using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using HareTortoiseGame.Manager;
using HareTortoiseGame.State;

namespace HareTortoiseGame.Component
{
    public class GraphComponent : BasicComponent
    {
        #region Field

        protected Texture2D _picture;
        protected StateManager _state;
        protected SpriteBatch _spriteBatch;

        #endregion

        #region Constructor

        public GraphComponent(Game game, Texture2D picture, DrawState state)
            : base(game)
        {
            _picture = picture;
            _state = new StateManager(game, state);
            PreviousBounds = game.GraphicsDevice.Viewport;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(_picture, new Rectangle((int)(PreviousBounds.X + PreviousBounds.Width * _state.CurrentState.Bounds.X),
                    (int)(PreviousBounds.Y + PreviousBounds.Height * _state.CurrentState.Bounds.Y),
                    (int)(PreviousBounds.Width * _state.CurrentState.Bounds.Z),
                    (int)(PreviousBounds.Height * _state.CurrentState.Bounds.W)), _state.CurrentState.SourcePosition,
                    _state.CurrentState.Color, _state.CurrentState.RotateAngle,
                    Vector2.Zero, _state.CurrentState.SpriteEffects, _state.CurrentState.Depth);
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        public override void Start()
        {
            _state.Start();
            base.Start();
        }

        public override void End()
        {
            _state.End();
            base.End();
        }

        #endregion
    }
}

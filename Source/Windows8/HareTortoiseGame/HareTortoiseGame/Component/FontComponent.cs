using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using HareTortoiseGame.Manager;
using HareTortoiseGame.State;

namespace HareTortoiseGame.Component
{
    public class FontComponent : BasicComponent
    {
        #region Field

        protected SpriteFont _font;
        protected StateManager _state;
        protected SpriteBatch _spriteBatch;

        #endregion

        #region Property

        public StateManager State { get { return _state; } }
        public string Content { get; set; }

        #endregion

        #region Constructor

        public FontComponent(Game game, SpriteFont font, DrawState state)
            : base(game)
        {
            _font = font;
            _state = new StateManager(game, state);
            PreviousBounds = game.GraphicsDevice.Viewport;
            Content = "";
        }

        #endregion

        #region Method

        public bool IsFinish()
        {
            return _state.IsFinish();
        }

        public Rectangle Bounds()
        {
            return new Rectangle((int)(PreviousBounds.X + PreviousBounds.Width * _state.CurrentState.Bounds.X),
                        (int)(PreviousBounds.Y + PreviousBounds.Height * _state.CurrentState.Bounds.Y),
                        (int)(PreviousBounds.Width * _state.CurrentState.Bounds.Z),
                        (int)(PreviousBounds.Height * _state.CurrentState.Bounds.W));
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            Rectangle bounds = Bounds();
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_font, Content, new Vector2(bounds.X, bounds.Y),
                _state.CurrentState.Color, _state.CurrentState.RotateAngle, Vector2.Zero,
                bounds.Width/_font.MeasureString(Content).X , _state.CurrentState.SpriteEffects,
                _state.CurrentState.Depth);
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        public bool IsHit()
        {
            if ((TouchControl.IsMouseClick() && Bounds().Contains(TouchControl.MousePosition()))
                || (TouchControl.IsTouchClick() && Bounds().Contains(TouchControl.TouchPosition())))
            {
                return true;
            }
            else return false;
        }

        public void AddState(float second, DrawState drawState)
        {
            _state.AddState(second, drawState);
        }

        public void ClearAllAndAddState(float second, DrawState drawState)
        {
            _state.ClearAllAndAddState(second, drawState);
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

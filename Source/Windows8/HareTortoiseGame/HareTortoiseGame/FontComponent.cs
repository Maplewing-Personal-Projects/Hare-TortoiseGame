// 人工智慧作業三 龜兔賽跑程式之研發                
// 資工系103級 499470098 曹又霖
// 使用方法：                                       
// 使用Visual Studio 2012並灌入MonoGame後即可打開並編譯。
// 執行方法：
// 需先灌入.NET Framework 4 和 OpenAL 後，即可打開。
// 信箱：sinmaplewing@gmail.com

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
                MathHelper.Max(bounds.Width/_font.MeasureString(Content).X,
                bounds.Height / _font.MeasureString(Content).Y), _state.CurrentState.SpriteEffects,
                _state.CurrentState.Depth);
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        public bool IsHit()
        {
            var bounds = Bounds();
            bounds.Width = (int)(_font.MeasureString(Content).X *
                MathHelper.Max(bounds.Width / _font.MeasureString(Content).X,
                bounds.Height / _font.MeasureString(Content).Y));
            bounds.Height = (int)(_font.MeasureString(Content).Y * 
                MathHelper.Max(bounds.Width / _font.MeasureString(Content).X,
                bounds.Height / _font.MeasureString(Content).Y));
            if ((TouchControl.IsMouseClick() && bounds.Contains(TouchControl.MousePosition()))
                || (TouchControl.IsTouchClick() && bounds.Contains(TouchControl.TouchPosition())))
            {
                return true;
            }
            else return false;
        }

        public bool IsHover()
        {
            var bounds = Bounds();
            bounds.Width = (int)(_font.MeasureString(Content).X *
                MathHelper.Max(bounds.Width / _font.MeasureString(Content).X,
                bounds.Height / _font.MeasureString(Content).Y));
            bounds.Height = (int)(_font.MeasureString(Content).Y *
                MathHelper.Max(bounds.Width / _font.MeasureString(Content).X,
                bounds.Height / _font.MeasureString(Content).Y));
            if (bounds.Contains(TouchControl.MousePosition())
                || bounds.Contains(TouchControl.TouchPosition()))
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

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
    public class GraphComponent : BasicComponent
    {
        #region Field

        protected Texture2D _picture;
        protected StateManager _state;
        protected SpriteBatch _spriteBatch;

        #endregion

        #region Property

        public StateManager State { get { return _state;  } }

        #endregion

        #region Constructor

        public GraphComponent(Game game, Texture2D picture, DrawState state)
            : base(game)
        {
            _picture = picture;
            _state = new StateManager(game, state);
            PreviousBounds = game.GraphicsDevice.Viewport;
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
            _spriteBatch.Begin();
            _spriteBatch.Draw(_picture, Bounds(), _state.CurrentState.SourcePosition,
                    _state.CurrentState.Color, _state.CurrentState.RotateAngle,
                    Vector2.Zero, _state.CurrentState.SpriteEffects, _state.CurrentState.Depth);
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

        public bool IsHover()
        {
            if (Bounds().Contains(TouchControl.MousePosition())
                || Bounds().Contains(TouchControl.TouchPosition()))
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

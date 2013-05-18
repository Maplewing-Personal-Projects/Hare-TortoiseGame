using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using HareTortoiseGame.State;
using HareTortoiseGame.GameLogic;

namespace HareTortoiseGame.Component
{
    public class Chess : GraphComponent
    {
        public enum Action { Left, Right, Up, Down };

        #region Field
        #endregion

        #region Properties

        public int X { get; private set; }
        public int Y { get; private set; }

        #endregion

        #region Constructor

        public Chess(Game game, Texture2D picture, DrawState state, int x, int y)
            : base(game, picture, state)
        {
            X = x;
            Y = y;
        }

        #endregion

        #region Methods

        public void Move(Action action)
        {
            switch (action)
            {
                case Action.Left:
                    X -= 1;
                    AddState( 1.0f, new DrawState(Game, 
                        new Vector4( _state.CurrentState.Bounds.X - 0.25f, _state.CurrentState.Bounds.Y,
                            _state.CurrentState.Bounds.Z, _state.CurrentState.Bounds.W ), _state.CurrentState.Color ));
                break;
                case Action.Right:
                    X += 1;
                    AddState( 1.0f, new DrawState(Game, 
                        new Vector4( _state.CurrentState.Bounds.X + 0.25f, _state.CurrentState.Bounds.Y,
                            _state.CurrentState.Bounds.Z, _state.CurrentState.Bounds.W ), _state.CurrentState.Color ));
                
                break;
                case Action.Up:
                    Y -= 1;
                    AddState( 1.0f, new DrawState(Game,
                        new Vector4(_state.CurrentState.Bounds.X, _state.CurrentState.Bounds.Y - 0.25f,
                            _state.CurrentState.Bounds.Z, _state.CurrentState.Bounds.W ), _state.CurrentState.Color ));
                break;
                case Action.Down:
                    Y += 1;
                    AddState( 1.0f, new DrawState( Game,
                        new Vector4(_state.CurrentState.Bounds.X, _state.CurrentState.Bounds.Y + 0.25f,
                            _state.CurrentState.Bounds.Z, _state.CurrentState.Bounds.W), _state.CurrentState.Color));
                
                break;
            }
        }

        #endregion
    }
}

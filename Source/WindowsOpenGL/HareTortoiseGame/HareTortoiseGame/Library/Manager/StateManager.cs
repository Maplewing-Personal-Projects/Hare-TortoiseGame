// 人工智慧作業三 龜兔賽跑程式之研發                
// 資工系103級 499470098 曹又霖
// 使用方法：                                       
// 使用Visual Studio 2012並灌入MonoGame後即可打開並編譯。
// 執行方法：
// 需先灌入.NET Framework 4 和 OpenAL 後，即可打開。
// 信箱：sinmaplewing@gmail.com

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using HareTortoiseGame.Component;
using HareTortoiseGame.State;

namespace HareTortoiseGame.Manager
{
    public class StateManager : BasicComponent
    {
        #region Field
        
        public const int FPS = 60;

        private Queue<float> _stateChangeSecondQueue;
        private Queue<DrawState> _stateQueue;
        private DrawState _goalState;
        private int _addframe;
        private Vector2 _addPosition;
        private Vector2 _addSize;
        private Vector4 _addColor;

        private Vector4 _currentColorAvoidSmallNumber;
        #endregion
        
        #region Property
        
        public DrawState CurrentState { get; set; }
        public DrawState LastState { 
            get {
                if (_stateQueue.Count <= 0) return _goalState;
                DrawState[] array = _stateQueue.ToArray(); 
                return array[array.Length - 1]; 
            } 
        }

        #endregion

        #region Method

        public StateManager(Game game, DrawState state) : base(game)
        {
            _stateChangeSecondQueue = new Queue<float>();
            _stateQueue = new Queue<DrawState>();
            CurrentState = state;
            _goalState = state;
            _currentColorAvoidSmallNumber = CurrentState.Color.ToVector4();
        }

        public bool IsFinish()
        {
            if (_addframe <= 0 && _stateQueue.Count == 0) return true;
            return false;
        }

        public override void Update(GameTime gameTime) 
        {
            if (_addframe > 0)
            {
                CurrentState.Bounds = new Vector4( CurrentState.Position.X + _addPosition.X,
                    CurrentState.Position.Y + _addPosition.Y,
                    CurrentState.Size.X + _addSize.X,
                    CurrentState.Size.Y + _addSize.Y );

                _currentColorAvoidSmallNumber = new Vector4(
                    _currentColorAvoidSmallNumber.X + _addColor.X,
                    _currentColorAvoidSmallNumber.Y + _addColor.Y,
                    _currentColorAvoidSmallNumber.Z + _addColor.Z,
                    _currentColorAvoidSmallNumber.W + _addColor.W);
                CurrentState.Color = new Color(_currentColorAvoidSmallNumber);

                --_addframe;
                if (_addframe <= 0) CurrentState = _goalState;
            }
            else if (_stateQueue.Count > 0)
            {
                _goalState = _stateQueue.Dequeue();
                SetNextState(_stateChangeSecondQueue.Dequeue(), _goalState);
            }

            base.Update(gameTime);
        
        }

        private void SetNextState( float second, DrawState state ){
            _addPosition = new Vector2(((state.Position.X - CurrentState.Position.X) / second / FPS),
                ((state.Position.Y - CurrentState.Position.Y) / second / FPS));
            _addSize = new Vector2(((state.Size.X - CurrentState.Size.X) / second / FPS),
                ((state.Size.Y - CurrentState.Size.Y) / second / FPS));
            Vector4 color = state.Color.ToVector4(), currentColor = CurrentState.Color.ToVector4();
            _addColor = new Vector4(((color.X - currentColor.X) / second / FPS),
                ((color.Y - currentColor.Y) / second / FPS),
                ((color.Z - currentColor.Z) / second / FPS),
                ((color.W - currentColor.W) / second / FPS));
            _addframe = (int)(second * (float)FPS) + 1;
            _goalState = state;
        }

        public void AddState(float second, DrawState state)
        {
            _stateChangeSecondQueue.Enqueue(second);
            _stateQueue.Enqueue(state);
        }

        public void ClearAllAndAddState(float second, DrawState state)
        {
            _stateChangeSecondQueue.Clear();
            _stateQueue.Clear();
            SetNextState(second, state);
        }

        #endregion


    }
}

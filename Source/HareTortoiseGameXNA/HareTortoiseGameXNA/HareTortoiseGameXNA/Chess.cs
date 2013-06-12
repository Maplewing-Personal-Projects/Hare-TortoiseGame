// 人工智慧作業三 龜兔賽跑程式之研發                
// 資工系103級 499470098 曹又霖
// 使用方法：                                       
// 使用Visual Studio 2012並灌入MonoGame後即可打開並編譯。
// 執行方法：
// 需先灌入.NET Framework 4 和 OpenAL 後，即可打開。
// 信箱：sinmaplewing@gmail.com

using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using HareTortoiseGame.State;
using HareTortoiseGame.GameLogic;

namespace HareTortoiseGame.Component
{
    public class Chess : GraphComponent
    {
        #region Enum
        public enum Type { Tortoise, Hare };
        public enum Action { Left = 1, Right = 2, Up = 3, Down = 4, None = 0, Goal = 5};
        #endregion

        #region Field
        #endregion

        #region Property

        public Type ChessType { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public bool GoalArrived { get; private set; }

        #endregion

        #region Constructor

        public Chess(Game game, Texture2D picture, DrawState state, int x, int y, Type chessType)
            : base(game, picture, state)
        {
            ChessType = chessType;
            X = x;
            Y = y;
            GoalArrived = false;
        }

        #endregion

        #region Method

        public void Move(Action action)
        {
            switch (action)
            {
                case Action.Left:
                    X -= 1;
                    AddState( 0.2f, new DrawState(Game,
                        new Vector4(_state.LastState.Bounds.X - (1.0f / Setting.MaxEdgeCount), _state.LastState.Bounds.Y,
                            _state.LastState.Bounds.Z, _state.LastState.Bounds.W), Color.White));
                break;
                case Action.Right:
                    X += 1;
                    if (X < Setting.MaxEdgeCount)
                    {
                        AddState(0.2f, new DrawState(Game,
                            new Vector4(_state.LastState.Bounds.X + (1.0f / Setting.MaxEdgeCount), _state.LastState.Bounds.Y,
                                _state.LastState.Bounds.Z, _state.LastState.Bounds.W), Color.White));
                    }
                    else
                    {
                        AddState(0.2f, new DrawState(Game,
                            new Vector4(_state.LastState.Bounds.X + (1.0f / Setting.MaxEdgeCount), _state.LastState.Bounds.Y,
                                _state.LastState.Bounds.Z, _state.LastState.Bounds.W), Color.White));
                        AddState(0.2f, new DrawState(Game,
                            new Vector4(_state.LastState.Bounds.X + 1f, _state.LastState.Bounds.Y,
                                _state.LastState.Bounds.Z, _state.LastState.Bounds.W), Color.White));
                        GoalArrived = true;
                    }
                break;
                case Action.Up:
                    Y -= 1;
                    if (Y >= 0)
                    {
                        AddState(0.2f, new DrawState(Game,
                            new Vector4(_state.LastState.Bounds.X, 
                                _state.LastState.Bounds.Y - (1.0f / Setting.MaxEdgeCount),
                                _state.LastState.Bounds.Z, _state.LastState.Bounds.W), Color.White));
                    }
                    else
                    {
                        AddState(0.2f, new DrawState(Game,
                            new Vector4(_state.LastState.Bounds.X,
                                _state.LastState.Bounds.Y - (1.0f / Setting.MaxEdgeCount),
                                _state.LastState.Bounds.Z, _state.LastState.Bounds.W), Color.White));
                        AddState(0.2f, new DrawState(Game,
                            new Vector4(_state.LastState.Bounds.X, _state.LastState.Bounds.Y - 1f,
                                _state.LastState.Bounds.Z, _state.LastState.Bounds.W), Color.White));
                        GoalArrived = true;
                    }
                break;
                case Action.Down:
                    Y += 1;
                    AddState(0.2f, new DrawState(Game,
                        new Vector4(_state.LastState.Bounds.X, 
                            _state.LastState.Bounds.Y + (1.0f / Setting.MaxEdgeCount),
                            _state.LastState.Bounds.Z, _state.LastState.Bounds.W), Color.White));
                
                break;
            }
        }
        public List< Tuple<int, Action> > GetAllPossibleMove()
        {
            List<Tuple<int, Action>> neighbor = new List<Tuple<int, Action>>();
            if (!GoalArrived)
            {
                if (Y != 0)
                    neighbor.Add(new Tuple<int, Action>((Y - 1) * Setting.MaxEdgeCount + X, Action.Up));
                if (X != Setting.MaxEdgeCount - 1)
                    neighbor.Add(new Tuple<int, Action>(Y * Setting.MaxEdgeCount + X + 1, Action.Right));
                if (ChessType == Type.Tortoise && X != 0)
                    neighbor.Add(new Tuple<int, Action>(Y * Setting.MaxEdgeCount + X - 1, Action.Left));
                if (ChessType == Type.Hare && Y != Setting.MaxEdgeCount - 1)
                    neighbor.Add(new Tuple<int, Action>((Y + 1) * Setting.MaxEdgeCount + X, Action.Down));
            }
            return neighbor;
        }
        public List< Tuple<int, Action> > GetAllGoalMove()
        {
            List<Tuple<int, Action>> neighbor = new List<Tuple<int, Action>>();
            if (!GoalArrived)
            {
                if (ChessType == Type.Tortoise && Y == 0)
                    neighbor.Add(new Tuple<int, Action>(X, Action.Up));
                if (ChessType == Type.Hare && X == Setting.MaxEdgeCount - 1)
                    neighbor.Add(new Tuple<int, Action>(Setting.MaxEdgeCount + Y, Action.Right));
            }
            return neighbor;
        }

        #endregion
    }
}

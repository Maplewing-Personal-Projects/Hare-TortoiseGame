using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using HareTortoiseGame.State;
using HareTortoiseGame.GameLogic;

namespace HareTortoiseGame.Component
{
    public class Board : Container
    {
        #region Enum
        public enum BoardState { WaitIO, Animation, Computer };
        public enum Turn { TortoiseTurn = 0, HareTurn = 1 };
        public enum Player { User, Computer };
        #endregion

        #region Field
        public const int TotalChessButton = 16;
        public const int GoalChessButton = 8;
        ChessButton[] _chessbutton;
        ChessButton[] _goalbutton;
        Chess[] _hare;
        Chess[] _tortoise;
        Player[] _players;
        
        Task<Tuple<int, Chess.Action>> _computerAITask; 
        #endregion

        #region Property
        public BoardState NowState { get; private set; }
        public Turn NowTurn { get; private set; }
        public Boolean NoMove { get; private set; }
        #endregion

        #region Constructor

        public Board(Game game, Texture2D picture, DrawState state, Player[] players = null, BoardData boardData = null)
            : base(game, picture, state)
        {
            if (players != null) _players = players;
            if (boardData == null) boardData = new BoardData();
            NowState = BoardState.WaitIO;
            NowTurn = Turn.TortoiseTurn;
            _chessbutton = new ChessButton[TotalChessButton];
            for (int i = 0; i < TotalChessButton; ++i)
            {
                _chessbutton[i] = new ChessButton(game, game.Content.Load<Texture2D>("blank"),
                    new DrawState(game, new Vector4(0.025f + 0.25f* (i % 4),0.025f + 0.25f * (i / 4), 0.23f, 0.24f), new Color(0.0f, 0.0f, 0.0f, 0.5f)));
                AddComponent(_chessbutton[i]);
            }

            _goalbutton = new ChessButton[GoalChessButton];
            for (int i = 0; i < GoalChessButton/2; ++i)
            {
                _goalbutton[i] = new ChessButton( game, game.Content.Load<Texture2D>("blank"),
                    new DrawState(game, new Vector4(0.025f + 0.25f * (i % 4), -0.04f, 0.23f, 0.05f), new Color(0.0f, 0.0f, 0.0f, 0.5f)));
                AddComponent(_goalbutton[i]);
            }
            for (int i = GoalChessButton / 2; i < GoalChessButton; ++i)
            {
                _goalbutton[i] = new ChessButton(game, game.Content.Load<Texture2D>("blank"),
                    new DrawState(game, new Vector4(1.02f, 0.025f + 0.25f * (i % 4), 0.04f, 0.24f), new Color(0.0f, 0.0f, 0.0f, 0.5f)));
                AddComponent(_goalbutton[i]);
            }

            int index = 0;
            _hare = new Chess[3];
            int hareChess = boardData.Hare;
            while (hareChess != 0)
            {
                int oneChess = (hareChess & -hareChess);
                int position = BoardData.GetOneChessPosition(oneChess);
                _chessbutton[position].HaveChess = true;
                hareChess &= ~oneChess;
                   
                _hare[index] = new Chess(game, game.Content.Load<Texture2D>("Rabbit"), new DrawState(
                    game, new Vector4(0.025f + 0.25f * (position % 4), 0.025f + 0.25f * (position / 4), 0.23f, 0.24f),
                    Color.White), position % 4, position / 4, Chess.Type.Hare);
                AddComponent(_hare[index]);
                ++index;
            }

            index = 0;
            _tortoise = new Chess[3];
            int tortoiseChess = boardData.Tortoise;
            while (tortoiseChess != 0)
            {
                int oneChess = (tortoiseChess & -tortoiseChess);
                int position = BoardData.GetOneChessPosition(oneChess);
                _chessbutton[position].HaveChess = true;
                tortoiseChess &= ~oneChess;

                _tortoise[index] = new Chess(game, game.Content.Load<Texture2D>("Turtle"), new DrawState(
                    game, new Vector4(0.025f + 0.25f * (position % 4), 0.025f + 0.25f * (position / 4), 0.23f, 0.24f),
                    Color.White), position % 4, position / 4, Chess.Type.Tortoise);
                AddComponent(_tortoise[index]);
                ++index;
            }

            _players = Setting.Players;
            _computerAITask = null;
        }

        #endregion

        #region Method

        public override void Update(GameTime gameTime)
        {
            if (!(TortoiseVictory() || HareVictory()))
            {
                Chess[] nowChess = null;
                switch (NowTurn)
                {
                    case Turn.TortoiseTurn:
                        nowChess = _tortoise;
                        break;
                    case Turn.HareTurn:
                        nowChess = _hare;
                        break;
                }

                if (_players[(int)NowTurn] == Player.Computer && NowState == BoardState.WaitIO)
                    NowState = BoardState.Computer;

                if (NowState == BoardState.WaitIO)
                {
                    if (TouchControl.IsMouseClick() || TouchControl.IsTouchClick())
                    {
                        for (int i = 0; i < GoalChessButton; ++i)
                        {
                            if (_goalbutton[i].IsHit() && _goalbutton[i].WantToGo != -1)
                            {
                                for (int j = 0; j < GoalChessButton; ++j)
                                {
                                    _goalbutton[j].ClearAllAndAddState(0.5f,
                                            new DrawState(Game, _goalbutton[j].State.CurrentState.Bounds,
                                            new Color(0.0f, 0.0f, 0.0f, 0.5f)));
                                    if (j != i) _goalbutton[j].WantToGo = -1;
                                }
                                for (int j = 0; j < TotalChessButton; ++j)
                                {
                                    _chessbutton[j].ClearAllAndAddState(0.5f,
                                            new DrawState(Game, _chessbutton[j].State.CurrentState.Bounds,
                                            new Color(0.0f, 0.0f, 0.0f, 0.5f)));
                                    _chessbutton[j].WantToGo = -1;
                                }
                                Chess chess = nowChess[_goalbutton[i].WantToGo];
                                _goalbutton[i].WantToGo = -1;
                                _chessbutton[chess.Y * 4 + chess.X].HaveChess = false;
                                chess.Move(_goalbutton[i].WantToGoAction);
                                NowTurn = (Turn)((int)NowTurn ^ 1);
                                NowState = BoardState.Animation;
                                NoMove = false;
                            }
                        }
                        for (int i = 0; i < TotalChessButton; ++i)
                        {
                            if (_chessbutton[i].WantToGo != -1 && _chessbutton[i].IsHit())
                            {
                                for (int j = 0; j < GoalChessButton; ++j)
                                {
                                    _goalbutton[j].ClearAllAndAddState(0.5f,
                                            new DrawState(Game, _goalbutton[j].State.CurrentState.Bounds,
                                            new Color(0.0f, 0.0f, 0.0f, 0.5f)));
                                    _goalbutton[j].WantToGo = -1;
                                }
                                for (int j = 0; j < TotalChessButton; ++j)
                                {
                                    _chessbutton[j].ClearAllAndAddState(0.5f,
                                            new DrawState(Game, _chessbutton[j].State.CurrentState.Bounds,
                                            new Color(0.0f, 0.0f, 0.0f, 0.5f)));
                                    if (j != i) _chessbutton[j].WantToGo = -1;
                                }
                                Chess chess = nowChess[_chessbutton[i].WantToGo];
                                _chessbutton[i].WantToGo = -1;
                                _chessbutton[chess.Y * 4 + chess.X].HaveChess = false;
                                _chessbutton[i].HaveChess = true;
                                chess.Move(_chessbutton[i].WantToGoAction);
                                NowTurn = (Turn)((int)NowTurn ^ 1);
                                NowState = BoardState.Animation;
                                NoMove = false;
                            }
                        }
                        for (int i = 0; i < 3; i++)
                        {
                            if (nowChess[i].IsHit() && !nowChess[i].Finish)
                            {
                                for (int j = 0; j < GoalChessButton; ++j)
                                {
                                    _goalbutton[j].ClearAllAndAddState(0.5f,
                                            new DrawState(Game, _goalbutton[j].State.CurrentState.Bounds,
                                            new Color(0.0f, 0.0f, 0.0f, 0.5f)));
                                    _goalbutton[j].WantToGo = -1;
                                }
                                for (int j = 0; j < TotalChessButton; ++j)
                                {
                                    _chessbutton[j].ClearAllAndAddState(0.5f,
                                            new DrawState(Game, _chessbutton[j].State.CurrentState.Bounds,
                                            new Color(0.0f, 0.0f, 0.0f, 0.5f)));
                                    _chessbutton[j].WantToGo = -1;
                                }
                                _chessbutton[nowChess[i].Y * 4 + nowChess[i].X].ClearAllAndAddState(0.5f,
                                    new DrawState(Game, _chessbutton[nowChess[i].Y * 4 + nowChess[i].X].State.CurrentState.Bounds,
                                        Color.PowderBlue));

                                List<Tuple<int, Chess.Action>> position = nowChess[i].GetAllPossibleMove();
                                foreach (Tuple<int, Chess.Action> possiblePosition in position)
                                {
                                    if (_chessbutton[possiblePosition.Item1].HaveChess)
                                    {
                                        _chessbutton[possiblePosition.Item1].ClearAllAndAddState(0.5f,
                                            new DrawState(Game, _chessbutton[possiblePosition.Item1].State.CurrentState.Bounds,
                                            Color.DarkRed));
                                    }
                                    else
                                    {
                                        _chessbutton[possiblePosition.Item1].ClearAllAndAddState(0.5f,
                                            new DrawState(Game, _chessbutton[possiblePosition.Item1].State.CurrentState.Bounds,
                                            Color.DarkSeaGreen));
                                        _chessbutton[possiblePosition.Item1].WantToGo = i;
                                        _chessbutton[possiblePosition.Item1].WantToGoAction = possiblePosition.Item2;
                                    }
                                }

                                position = nowChess[i].GetAllGoalMove();
                                foreach (Tuple<int, Chess.Action> possiblePosition in position)
                                {
                                    _goalbutton[possiblePosition.Item1].ClearAllAndAddState(0.5f,
                                            new DrawState(Game, _goalbutton[possiblePosition.Item1].State.CurrentState.Bounds,
                                            Color.DarkSeaGreen));
                                    _goalbutton[possiblePosition.Item1].WantToGo = i;
                                    _goalbutton[possiblePosition.Item1].WantToGoAction = possiblePosition.Item2;
                                }

                            }
                        }
                    }
                }
                else if (NowState == BoardState.Animation)
                {
                    CheckPass(nowChess);

                    bool isFinish = true;
                    for (int i = 0; i < 3; ++i)
                    {
                        isFinish &= _tortoise[i].IsFinish() && _hare[i].IsFinish();
                    }
                    if (isFinish)
                    {
                        if (_players[(int)NowTurn] == Player.Computer) NowState = BoardState.Computer;
                        else NowState = BoardState.WaitIO;
                    }
                    if (_computerAITask == null && _players[(int)NowTurn] == Player.Computer)
                    {
                        ComputerAI.setComputerAI(new BoardData(_tortoise, _hare), Setting.MaxPly, NowTurn);
                        _computerAITask = new Task<Tuple<int, Chess.Action>>(ComputerAI.BestMove);
                        _computerAITask.Start();
                    }
                }
                else
                {
                    if (_computerAITask == null)
                    {
                        ComputerAI.setComputerAI(new BoardData(_tortoise, _hare), Setting.MaxPly, NowTurn);
                        _computerAITask = new Task<Tuple<int, Chess.Action>>(ComputerAI.BestMove);
                        _computerAITask.Start();
                    }
                    else
                    {
                        if (_computerAITask.IsCompleted)
                        {
                            Tuple<int, Chess.Action> move = _computerAITask.Result;
                            for (int i = 0; i < 3; ++i)
                            {
                                if (nowChess[i].X == move.Item1 % 4 && nowChess[i].Y == move.Item1 / 4)
                                {
                                    _chessbutton[nowChess[i].Y * 4 + nowChess[i].X].HaveChess = false;
                                    nowChess[i].Move(move.Item2);
                                    if (!nowChess[i].Finish) _chessbutton[nowChess[i].Y * 4 + nowChess[i].X].HaveChess = true;
                                }
                            }
                            NowTurn = (Turn)((int)(NowTurn) ^ 1);
                            if (_players[(int)NowTurn] == Player.Computer) NowState = BoardState.Animation;
                            else NowState = BoardState.WaitIO;
                            _computerAITask = null;
                        }
                    }
                }
            }
            base.Update(gameTime);
        }

        private void CheckPass(Chess[] nowChess)
        {
            List<Tuple<int, Chess.Action>> possibleMove = new List<Tuple<int, Chess.Action>>();
            bool turnOther = true;
            for (int i = 0; i < 3; ++i)
            {
                if (nowChess[i].GetAllGoalMove().Count > 0)
                {
                    turnOther = false;
                    break;
                }
                possibleMove.AddRange(nowChess[i].GetAllPossibleMove());
            }
            if (turnOther)
            {
                foreach (var possible in possibleMove)
                {
                    if (!_chessbutton[possible.Item1].HaveChess)
                    {
                        turnOther = false;
                        break;
                    }
                }
            }
            if (turnOther)
            {
                NowTurn = (Turn)((int)NowTurn ^ 1);
                NoMove = true;
            }
        }

        public bool TortoiseVictory()
        {
            return _tortoise[0].Finish && _tortoise[1].Finish && _tortoise[2].Finish;
        }

        public bool HareVictory()
        {
            return _hare[0].Finish && _hare[1].Finish && _hare[2].Finish;
        }
        #endregion
    }
}

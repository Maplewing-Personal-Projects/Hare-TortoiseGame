using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

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
        int _totalChessButtonCount;
        int _goalChessButtonCount;
        ChessButton[] _chessbutton;
        ChessButton[] _goalbutton;
        Chess[] _hare;
        Chess[] _tortoise;
        Player[] _players;

        SoundEffect _click;
        SoundEffect _clickError;
        bool _errorSound;
        
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

            InitParameters();
            InitAndLoadContent(game, boardData);
        }

        private void InitParameters()
        {
            _totalChessButtonCount = Setting.MaxEdgeCount * Setting.MaxEdgeCount;
            _goalChessButtonCount = Setting.MaxEdgeCount * 2;
            NowState = BoardState.WaitIO;
            NowTurn = Turn.TortoiseTurn;
            _players = Setting.Players;
            _computerAITask = null;

            _chessbutton = new ChessButton[_totalChessButtonCount];
            _goalbutton = new ChessButton[_goalChessButtonCount];
            _hare = new Chess[Setting.MaxEdgeCount - 1];
            _tortoise = new Chess[Setting.MaxEdgeCount - 1];
        }
        private void InitAndLoadContent(Game game, BoardData boardData)
        {
            _click = game.Content.Load<SoundEffect>("misc_menu_4");
            _clickError = game.Content.Load<SoundEffect>("negative_2");

            InitChessButton(game);
            InitGoalButton(game);
            InitChess(game, boardData.Hare, _hare, game.Content.Load<Texture2D>("Rabbit"),
                Chess.Type.Hare);
            InitChess(game, boardData.Tortoise, _tortoise, game.Content.Load<Texture2D>("Turtle"),
                Chess.Type.Tortoise);
        }
        private void InitGoalButton(Game game)
        {
            for (int i = 0; i < _goalChessButtonCount / 2; ++i)
            {
                _goalbutton[i] = new ChessButton(game, game.Content.Load<Texture2D>("blank"),
                    new DrawState(game, new Vector4(0.025f + (1.0f / Setting.MaxEdgeCount) * (i % Setting.MaxEdgeCount),
                        -0.04f, (1.0f / Setting.MaxEdgeCount) - 0.02f, 0.05f), new Color(0.0f, 0.0f, 0.0f, 0.5f)));
                AddComponent(_goalbutton[i]);
            }
            for (int i = _goalChessButtonCount / 2; i < _goalChessButtonCount; ++i)
            {
                _goalbutton[i] = new ChessButton(game, game.Content.Load<Texture2D>("blank"),
                    new DrawState(game, new Vector4(1.02f, 0.025f + (1.0f / Setting.MaxEdgeCount) * (i % Setting.MaxEdgeCount),
                        0.04f, (1.0f / Setting.MaxEdgeCount) - 0.01f), new Color(0.0f, 0.0f, 0.0f, 0.5f)));
                AddComponent(_goalbutton[i]);
            }
        }
        private void InitChessButton(Game game)
        {
            for (int i = 0; i < _totalChessButtonCount; ++i)
            {
                _chessbutton[i] = new ChessButton(game, game.Content.Load<Texture2D>("blank"),
                    new DrawState(game, new Vector4(0.025f + (1.0f / Setting.MaxEdgeCount) * (i % Setting.MaxEdgeCount),
                        0.025f + (1.0f / Setting.MaxEdgeCount) * (i / Setting.MaxEdgeCount),
                        (1.0f / Setting.MaxEdgeCount) - 0.02f,
                        (1.0f / Setting.MaxEdgeCount) - 0.01f), new Color(0.0f, 0.0f, 0.0f, 0.5f)));
                AddComponent(_chessbutton[i]);
            }
        }
        private void InitChess(Game game, ulong chessMap, Chess[] chess, Texture2D chessTexture, Chess.Type chessType )
        {
            int index = 0;
            while (chessMap != 0)
            {
                ulong oneChess = (chessMap & ((~chessMap)+1));
                int position = BoardData.GetOneChessPosition(oneChess);
                _chessbutton[position].HaveChess = true;
                chessMap &= ~(ulong)oneChess;

                chess[index] = new Chess(game, chessTexture, new DrawState(
                    game, new Vector4(0.025f + (1.0f / Setting.MaxEdgeCount) * (position % Setting.MaxEdgeCount),
                        0.025f + (1.0f / Setting.MaxEdgeCount) * (position / Setting.MaxEdgeCount),
                        (1.0f / Setting.MaxEdgeCount) - 0.02f, (1.0f / Setting.MaxEdgeCount) - 0.01f),
                    Color.White), position % Setting.MaxEdgeCount, position / Setting.MaxEdgeCount, chessType);
                AddComponent(chess[index]);
                ++index;
            }
        }

        #endregion

        #region Method

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (NotUpdate()) return;

            Chess[] nowChess = GetCorrectTurnChess();

            switch(NowState){
                case BoardState.WaitIO:
                    WaitUserInput(nowChess);
                break;
                case BoardState.Animation:
                    WaitAnimationDone();
                break;
                case BoardState.Computer:
                    WaitComputerMove(nowChess);
                break;
            }
        }

        public bool CheckVictory()
        {
            return TortoiseVictory() || HareVictory();
        }
        public bool TortoiseVictory()
        {
            return CheckChessVictory(_tortoise);
        }
        public bool HareVictory()
        {
            return CheckChessVictory(_hare);
        }
        private bool CheckChessVictory(Chess[] chess)
        {
            bool allGoalArrived = true;
            for (int i = 0; i < Setting.MaxEdgeCount - 1; ++i)
            {
                allGoalArrived &= chess[i].GoalArrived;
            }
            return allGoalArrived;
        }
        private bool NotUpdate()
        {
            return !IsFinish() || CheckVictory();
        }
        private Chess[] GetCorrectTurnChess()
        {
            if (_players[(int)NowTurn] == Player.Computer && NowState == BoardState.WaitIO)
                NowState = BoardState.Computer;

            Chess[] nowChess = null;
            Turn previousTurn = NowTurn;
            do
            {
                previousTurn = NowTurn;
                switch (NowTurn)
                {
                    case Turn.TortoiseTurn:
                        nowChess = _tortoise;
                        break;
                    case Turn.HareTurn:
                        nowChess = _hare;
                        break;
                }
                CheckPass(nowChess);
            } while (previousTurn != NowTurn);
            return nowChess;
        }
        private void CheckPass(Chess[] nowChess)
        {
            List<Tuple<int, Chess.Action>> possibleMove = new List<Tuple<int, Chess.Action>>();
            bool turnOther = true;
            for (int i = 0; i < Setting.MaxEdgeCount - 1; ++i)
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
                TurnTheTurn();
                NoMove = true;
            }
        }

        private void WaitUserInput(Chess[] nowChess)
        {
            if (TouchControl.IsClick())
            {
                _errorSound = false;
                ChessButtonClickMove(nowChess, _goalbutton, _goalChessButtonCount);
                ChessButtonClickMove(nowChess, _chessbutton, _totalChessButtonCount);
                ChessClickMove(nowChess);

                if (_errorSound) _clickError.Play();
            }
        }
        private void WaitAnimationDone()
        {
            bool isAnimationFinish = GetAllChessAnimationFinish();
            if (isAnimationFinish)
            {
                if (_players[(int)NowTurn] == Player.Computer) NowState = BoardState.Computer;
                else NowState = BoardState.WaitIO;
            }
            if (_computerAITask == null && _players[(int)NowTurn] == Player.Computer)
            {
                ComputerStartToCompute();
            }
        }
        private void WaitComputerMove(Chess[] nowChess)
        {
            if (_computerAITask == null)
            {
                ComputerStartToCompute();
            }
            else if (_computerAITask.IsCompleted)
            {
                ComputerMove(nowChess);
            }
        }

        private void ComputerMove(Chess[] nowChess)
        {
            Tuple<int, Chess.Action> move = _computerAITask.Result;
            for (int i = 0; i < Setting.MaxEdgeCount - 1; ++i)
            {
                if (nowChess[i].X == move.Item1 % Setting.MaxEdgeCount && nowChess[i].Y == move.Item1 / Setting.MaxEdgeCount)
                {
                    ChessMove(nowChess[i], _chessbutton[nowChess[i].Y * Setting.MaxEdgeCount + nowChess[i].X],
                        move.Item2);
                }
            }

            TurnTheTurn();
            if (_players[(int)NowTurn] == Player.Computer) NowState = BoardState.Animation;
            else NowState = BoardState.WaitIO;
            _computerAITask = null;
        }
        private void ComputerStartToCompute()
        {
            var computer = new ComputerAI(new BoardData(_tortoise, _hare), Setting.MaxPly, NowTurn);
            _computerAITask = new Task<Tuple<int, Chess.Action>>(computer.BestMove);
            _computerAITask.Start();
        }
        private bool GetAllChessAnimationFinish()
        {
            bool isAnimationFinish = true;
            for (int i = 0; i < Setting.MaxEdgeCount - 1; ++i)
            {
                isAnimationFinish &= _tortoise[i].IsFinish() && _hare[i].IsFinish();
            }
            return isAnimationFinish;
        }
        
        private void ChessClickMove(Chess[] nowChess)
        {
            for (int i = 0; i < Setting.MaxEdgeCount - 1; i++)
            {
                if (nowChess[i].IsHit() && !nowChess[i].GoalArrived)
                {
                    _click.Play();
                    _errorSound = false;
                    CleanAllChessButtonAnimation(_goalbutton, _goalChessButtonCount);
                    CleanAllChessButtonAnimation(_chessbutton, _totalChessButtonCount);

                    _chessbutton[nowChess[i].Y * Setting.MaxEdgeCount + nowChess[i].X].ClearAllAndAddState(0.5f,
                        new DrawState(Game,
                            _chessbutton[nowChess[i].Y * Setting.MaxEdgeCount + nowChess[i].X].State.CurrentState.Bounds,
                            Color.PowderBlue));

                    UpdatePossibleMoveButton(i, _chessbutton, nowChess[i].GetAllPossibleMove());
                    UpdatePossibleMoveButton(i, _goalbutton, nowChess[i].GetAllGoalMove());
                }
            }
        }
        private void ChessButtonClickMove(Chess[] nowChess,
            ChessButton[] chessButtons, int totalButtonCount)
        {
            for (int i = 0; i < totalButtonCount; ++i)
            {
                if (chessButtons[i].IsHit() && chessButtons[i].WantToGo != -1)
                {
                    _click.Play();
                    _errorSound = false;
                    Chess chess = nowChess[chessButtons[i].WantToGo];
                    ChessMove(chess, _chessbutton[chess.Y * Setting.MaxEdgeCount + chess.X],
                        chessButtons[i].WantToGoAction);
                    CleanAllChessButtonAnimation(_goalbutton, _goalChessButtonCount);
                    CleanAllChessButtonAnimation(_chessbutton, _totalChessButtonCount);

                    TurnTheTurn();
                    NowState = BoardState.Animation;
                    NoMove = false;
                }
                else if (chessButtons[i].IsHit())
                {
                    _errorSound = true;
                }
            }
        }

        private void UpdatePossibleMoveButton(int i,  ChessButton[] chessButton, List<Tuple<int, Chess.Action>> position)
        {
            foreach (Tuple<int, Chess.Action> possiblePosition in position)
            {
                if (chessButton[possiblePosition.Item1].HaveChess)
                {
                    chessButton[possiblePosition.Item1].ClearAllAndAddState(0.5f,
                        new DrawState(Game, chessButton[possiblePosition.Item1].State.CurrentState.Bounds,
                        Color.DarkRed));
                }
                else
                {
                    chessButton[possiblePosition.Item1].ClearAllAndAddState(0.5f,
                        new DrawState(Game, chessButton[possiblePosition.Item1].State.CurrentState.Bounds,
                        Color.DarkSeaGreen));
                    chessButton[possiblePosition.Item1].WantToGo = i;
                    chessButton[possiblePosition.Item1].WantToGoAction = possiblePosition.Item2;
                }
            }
        }
        private void ChessMove(Chess moveChess, ChessButton moveOut, Chess.Action chessAction)
        {
            moveOut.HaveChess = false;
            moveChess.Move(chessAction);
            if (!moveChess.GoalArrived)
                _chessbutton[moveChess.Y * Setting.MaxEdgeCount + moveChess.X].HaveChess = true;
        }
        private void CleanAllChessButtonAnimation(ChessButton[] chessButton, int chessButtonCount)
        {
            for (int i = 0; i < chessButtonCount; ++i)
            {
                chessButton[i].ClearAllAndAddState(0.5f,
                        new DrawState(Game, chessButton[i].State.CurrentState.Bounds,
                        new Color(0.0f, 0.0f, 0.0f, 0.5f)));
                chessButton[i].WantToGo = -1;
            }
        }
        private void TurnTheTurn()
        {
            NowTurn = (Turn)((int)NowTurn ^ 1);
        }
        
        #endregion
    }
}

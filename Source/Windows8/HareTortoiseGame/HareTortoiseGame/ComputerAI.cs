// 人工智慧作業三 龜兔賽跑程式之研發                
// 資工系103級 499470098 曹又霖
// 使用方法：                                       
// 使用Visual Studio 2012並灌入MonoGame後即可打開並編譯。
// 執行方法：
// 需先灌入.NET Framework 4 和 OpenAL 後，即可打開。
// 信箱：sinmaplewing@gmail.com

using System;
using System.IO;
using System.Collections.Generic;
using HareTortoiseGame.Component;

namespace HareTortoiseGame.GameLogic
{
    public class ComputerAI
    {
        #region Field

        BoardData _board;
        int _ply;
        Board.Turn _nowTurn;
        int[] _stuck;
        int[,] _actionCount;
        bool _top;

        int _bestPosition = -1;
        Chess.Action _bestAction = Chess.Action.None;

        const int WinValue = 1000;
        const int MinValue = -9999999;
        const int MaxValue = 9999999;
        //static Random _random = new Random();
        #endregion

        #region Method

        static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp;
            temp = lhs;
            lhs = rhs;
            rhs = temp;
        }

        public ComputerAI(BoardData initBoard, int maxPly, Board.Turn nowTurn)
        {
            _board = initBoard;
            _ply = maxPly;
            _nowTurn = nowTurn;
            _stuck = new int[] { 0, 0 };
            _actionCount = new int[,] { { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } };
            _top = true;
        }

        public Tuple<int, Chess.Action> BestMove()
        {
            _top = true;
            int result = AlphaBeta(_nowTurn, MinValue, MaxValue);
            return new Tuple<int, Chess.Action>(_bestPosition, _bestAction);
        }

        private int AlphaBeta(Board.Turn turn, int alpha, int beta)
        {
            bool top = false;
            Swap(ref _top,ref top);

            --_ply;
            if (_board.TerminalTest() || _ply == 0)
            { ++_ply; return Eval(turn, _board); }
            
            int value = MinValue;
            ulong goalMove = 0, leftMove = 0, rightMove = 0, upMove = 0, downMove = 0;

            GetAllMove(turn, ref goalMove, ref leftMove, ref rightMove, ref upMove, ref downMove);

            if (CheckNoMove(goalMove, leftMove, rightMove, upMove, downMove))
            {
                ++_stuck[(int)turn];
                var result = AlphaBeta(TurnTheTurn(turn), -beta, -alpha);
                ++_ply;
                --_stuck[(int)turn];
                return result;
            }

            var returnValue = TestMove(turn, ref alpha, beta, ref value, top, ref goalMove, Chess.Action.Goal);
            if (returnValue.HasValue) return returnValue.Value;

            returnValue = TestMove(turn, ref alpha, beta, ref value, top, ref rightMove, Chess.Action.Right);
            if (returnValue.HasValue) return returnValue.Value;

            returnValue = TestMove(turn, ref alpha, beta, ref value, top, ref upMove, Chess.Action.Up);
            if (returnValue.HasValue) return returnValue.Value;

            returnValue = TestMove(turn, ref alpha, beta, ref value, top, ref downMove, Chess.Action.Down);
            if (returnValue.HasValue) return returnValue.Value;

            returnValue = TestMove(turn, ref alpha, beta, ref value, top, ref leftMove, Chess.Action.Left);
            if (returnValue.HasValue) return returnValue.Value; 

            ++_ply;
            return value;
        }

        private int? TestMove(Board.Turn turn, ref int alpha, 
            int beta, ref int bestValue, bool top, 
            ref ulong allMove, Chess.Action action)
        {
            ulong move;
            while (allMove != 0)
            {
                move = GetOneMove(allMove);
                allMove = DeleteAMove(allMove, move);

                MoveBoard(turn, action, move);
                int result = AlphaBeta(TurnTheTurn(turn), -beta, -alpha);
                UnMoveBoard(turn, action, move);

                if (-result > bestValue)
                {
                    bestValue = -result;
                    if (top)
                    {
                        _bestPosition = BoardData.GetOneChessPosition(GetOriginalMove(action, move));
                        if (turn == Board.Turn.HareTurn)
                        {
                            if (action == Chess.Action.Goal) _bestAction = Chess.Action.Right;
                            else _bestAction = action;
                        }
                        else
                        {
                            if (action == Chess.Action.Goal) _bestAction = Chess.Action.Up;
                            else _bestAction = action;
                        }
                    }
                }

                if (bestValue >= beta)
                { ++_ply; return bestValue; }
                alpha = Math.Max(alpha, bestValue);
            }
            return null;
        }
        private ulong GetOriginalMove(Chess.Action action, ulong move)
        {
            switch (action)
            {
                case Chess.Action.Goal:
                    return move;
                case Chess.Action.Left:
                    return move << 1;
                case Chess.Action.Right:
                    return move >> 1;
                case Chess.Action.Up:
                    return move << SettingParameters.MaxEdgeCount;
                case Chess.Action.Down:
                    return move >> SettingParameters.MaxEdgeCount;
                default:
                    return move;
            }
        }
        private void UnMoveBoard(Board.Turn turn, Chess.Action action, ulong move)
        {
            if (action != Chess.Action.Goal)
            {
                --_actionCount[(int)turn, (int)action];
            }

            if (turn == Board.Turn.HareTurn)
            {
                if (action == Chess.Action.Goal)
                {
                    --_actionCount[(int)turn, (int)Chess.Action.Right];
                }
                UnOneMoveBoard(ref _board.Hare, turn, move, action);
            }
            else
            {
                if (action == Chess.Action.Goal)
                {
                    --_actionCount[(int)turn, (int)Chess.Action.Up];
                }
                UnOneMoveBoard(ref _board.Tortoise, turn, move, action);
            }
        }
        private void MoveBoard(Board.Turn turn, Chess.Action action, ulong move)
        {
            if (turn == Board.Turn.HareTurn)
            {
                OneMoveBoard(ref _board.Hare, turn, move, action);
                if (action == Chess.Action.Goal)
                {
                    ++_actionCount[(int)turn, (int)Chess.Action.Right];
                }
            }
            else
            {
                OneMoveBoard(ref _board.Tortoise, turn, move, action);
                if (action == Chess.Action.Goal)
                {
                    ++_actionCount[(int)turn, (int)Chess.Action.Up];
                }
            }

            if (action != Chess.Action.Goal)
            {
                ++_actionCount[(int)turn, (int)action];
            }
        }
        private void OneMoveBoard(ref ulong board, Board.Turn turn, ulong move, Chess.Action action)
        {
            switch (action)
            {
                case Chess.Action.Goal:
                    board &= ~move;
                break;

                case Chess.Action.Left:
                    board &= ~(move << 1);
                    board |= move;
                break;

                case Chess.Action.Right:
                    board &= ~(move >> 1);
                    board |= move;
                break;

                case Chess.Action.Up:
                    board &= ~(move << SettingParameters.MaxEdgeCount);
                    board |= move;
                break;

                case Chess.Action.Down:
                    board &= ~(move >> SettingParameters.MaxEdgeCount);
                    board |= move;
                break;

                default:
                break;
            }
        }
        private void UnOneMoveBoard(ref ulong board, Board.Turn turn, ulong move, Chess.Action action)
        {
            switch (action)
            {
                case Chess.Action.Goal:
                    board |= move;
                break;

                case Chess.Action.Left:
                    board |= (move << 1);
                    board &= ~move;
                break;

                case Chess.Action.Right:
                    board |= (move >> 1);
                    board &= ~move;
                break;

                case Chess.Action.Up:
                    board |= (move << SettingParameters.MaxEdgeCount);
                    board &= ~move;
                break;

                case Chess.Action.Down:
                    board |= (move >> SettingParameters.MaxEdgeCount);
                    board &= ~move;
                break;

                default:
                break;
            }
        }
        private ulong DeleteAMove(ulong allMove, ulong move)
        {
            allMove &= ~move;
            return allMove;
        }
        private ulong GetOneMove(ulong allMove)
        {
            return (allMove & ((~allMove) + 1));
        }
        private Board.Turn TurnTheTurn(Board.Turn turn)
        {
            return (Board.Turn)((int)turn ^ 1);
        }

        private bool CheckNoMove(ulong goalMove, ulong leftMove, ulong rightMove, ulong upMove, ulong downMove)
        {
            return goalMove == 0 && leftMove == 0 && rightMove == 0 && upMove == 0 && downMove == 0;
        }
        private void GetAllMove(Board.Turn turn, ref ulong goalMove, ref ulong leftMove, ref ulong rightMove, ref ulong upMove, ref ulong downMove)
        {
            if (turn == Board.Turn.HareTurn)
            {
                goalMove = (_board.Hare & BoardData.Column[SettingParameters.MaxEdgeCount - 1]);
                rightMove = ((_board.Hare & ~BoardData.Column[SettingParameters.MaxEdgeCount - 1]) << 1) & _board.Empty();
                upMove = ((_board.Hare & ~BoardData.Row[0]) >> SettingParameters.MaxEdgeCount) & _board.Empty();
                downMove = ((_board.Hare & ~BoardData.Row[SettingParameters.MaxEdgeCount - 1]) << SettingParameters.MaxEdgeCount) & _board.Empty();
            }
            else
            {
                goalMove = (_board.Tortoise & BoardData.Row[0]);
                leftMove = ((_board.Tortoise & ~BoardData.Column[0]) >> 1) & _board.Empty();
                rightMove = ((_board.Tortoise & ~BoardData.Column[SettingParameters.MaxEdgeCount - 1]) << 1) & _board.Empty();
                upMove = ((_board.Tortoise & ~BoardData.Row[0]) >> SettingParameters.MaxEdgeCount) & _board.Empty();
            }
        }
        public int CountBit(ulong bit)
        {
            int count = 0;
            while (bit != 0)
            {
                if ((bit & 1) == 1) ++count;
                bit >>= 1;
            }
            return count;
        }
        public int Eval(Board.Turn turn, BoardData board)
        {
            int hareScore = SettingParameters.MaxEdgeCount * SettingParameters.MaxEdgeCount,
                tortoiseScore = SettingParameters.MaxEdgeCount * SettingParameters.MaxEdgeCount;

            for (int i = 0; i < SettingParameters.MaxEdgeCount; ++i)
            {
                hareScore -= (SettingParameters.MaxEdgeCount - i) * CountBit(board.Hare & BoardData.Column[i]);
                tortoiseScore -= (i + 1) * CountBit(board.Tortoise & BoardData.Row[i]);
            }

            if (board.Hare == 0) 
                hareScore += WinValue;
            else if (board.Tortoise == 0) 
                tortoiseScore += WinValue;

            if (turn == Board.Turn.HareTurn) return hareScore - tortoiseScore;
            else return tortoiseScore - hareScore;
        }

        #endregion
    }
}
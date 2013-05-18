using System;
using HareTortoiseGame.Component;

namespace HareTortoiseGame.GameLogic
{
    public static class ComputerAI
    {

        #region Properties

        static BoardData _board;
        static int _maxPly;
        static Board.Turn _nowTurn;
        #endregion

        #region Methods

        public static void setComputerAI(BoardData initBoard, int maxPly, Board.Turn nowTurn)
        {
            _board = initBoard;
            _maxPly = maxPly;
            _nowTurn = nowTurn;
        }

        public static Tuple<int, Chess.Action> BestMove()
        {
            Tuple<int, int, Chess.Action> result = AlphaBeta(_nowTurn, int.MinValue, int.MaxValue);
            return new Tuple<int, Chess.Action>(result.Item2, result.Item3);
        }

        public static Tuple<int, int, Chess.Action> AlphaBeta(Board.Turn turn, int alpha, int beta)
        {
            --_maxPly;
            if (_board.TerminalTest() || _maxPly == 0)
            { ++_maxPly; return new Tuple<int,int,Chess.Action>(Eval(_nowTurn, _board), -1, Chess.Action.None); }
            
            int value = int.MinValue;
            int position = -1;
            Chess.Action action = Chess.Action.None;
            int goalMove = 0, leftMove = 0, rightMove = 0, upMove = 0, downMove = 0;
            if (turn == Board.Turn.HareTurn)
            {
                goalMove = (_board.Hare & BoardData.Column[3]);
                rightMove = ((_board.Hare & ~BoardData.Column[3]) << 1) & _board.Empty();
                upMove = ((_board.Hare & ~BoardData.Row[0]) >> 4) & _board.Empty();
                downMove = ((_board.Hare & ~BoardData.Row[3]) << 4) & _board.Empty();
            }
            else
            {
                goalMove = (_board.Tortoise & BoardData.Row[0]);
                leftMove = ((_board.Tortoise & ~BoardData.Column[0]) >> 1) & _board.Empty();
                rightMove = ((_board.Tortoise & ~BoardData.Column[3]) << 1) & _board.Empty();
                upMove = ((_board.Tortoise & ~BoardData.Row[0]) >> 4) & _board.Empty();
            }

            if (goalMove == 0 && leftMove == 0 && rightMove == 0 && upMove == 0 && downMove == 0)
            {
                var result = AlphaBeta((Board.Turn)((int)turn ^ 1), -beta, -alpha);
                ++_maxPly;
                return result;
            }

            int move;
            while (goalMove != 0)
            {
                move = (goalMove & -goalMove);
                goalMove &= ~move;
                if (turn == Board.Turn.HareTurn)
                {
                    _board.Hare &= ~move;
                }
                else
                {
                    _board.Tortoise &= ~move;
                }

                Tuple<int,int,Chess.Action> result = AlphaBeta((Board.Turn)((int)turn ^ 1), -beta, -alpha);
                if (-result.Item1 > value)
                {
                    value = -result.Item1;
                    position = BoardData.GetOneChessPosition(move);
                    if (turn == Board.Turn.HareTurn) action = Chess.Action.Right;
                    else action = Chess.Action.Up;
                }

                if (turn == Board.Turn.HareTurn)
                {
                    _board.Hare |= move;
                }
                else
                {
                    _board.Tortoise |= move;
                }

                if (value >= beta)
                { ++_maxPly; return new Tuple<int, int, Chess.Action>(value, position, action); }
                alpha = Math.Max(alpha, value);
            }

            while (leftMove != 0)
            {
                move = (leftMove & -leftMove);
                leftMove &= ~move;
                if (turn == Board.Turn.HareTurn)
                {
                    _board.Hare &= ~(move << 1);
                    _board.Hare |= move;
                }
                else
                {
                    _board.Tortoise &= ~(move << 1);
                    _board.Tortoise |= move;
                }

                Tuple<int, int, Chess.Action> result = AlphaBeta((Board.Turn)((int)turn ^ 1), -beta, -alpha);
                if (-result.Item1 > value)
                {
                    value = -result.Item1;
                    position = BoardData.GetOneChessPosition(move << 1);
                    action = Chess.Action.Left;
                }

                if (turn == Board.Turn.HareTurn)
                {
                    _board.Hare |= move << 1;
                    _board.Hare &= ~move;
                }
                else
                {
                    _board.Tortoise |= move << 1;
                    _board.Tortoise &= ~move;
                }

                if (value >= beta)
                { ++_maxPly; return new Tuple<int, int, Chess.Action>(value, position, action); }
                alpha = Math.Max(alpha, value);
            }

            while (rightMove != 0)
            {
                move = (rightMove & -rightMove);
                rightMove &= ~move;
                if (turn == Board.Turn.HareTurn)
                {
                    _board.Hare &= ~(move >> 1);
                    _board.Hare |= move;
                }
                else
                {
                    _board.Tortoise &= ~(move >> 1);
                    _board.Tortoise |= move;
                }

                Tuple<int, int, Chess.Action> result = AlphaBeta((Board.Turn)((int)turn ^ 1), -beta, -alpha);
                if (-result.Item1 > value)
                {
                    value = -result.Item1;
                    position = BoardData.GetOneChessPosition(move >> 1);
                    action = Chess.Action.Right;
                }

                if (turn == Board.Turn.HareTurn)
                {
                    _board.Hare |= move >> 1;
                    _board.Hare &= ~move;
                }
                else
                {
                    _board.Tortoise |= move >> 1;
                    _board.Tortoise &= ~move;
                }

                if (value >= beta)
                { ++_maxPly; return new Tuple<int, int, Chess.Action>(value, position, action); }
                alpha = Math.Max(alpha, value);
            }

            while (upMove != 0)
            {
                move = (upMove & -upMove);
                upMove &= ~move;
                if (turn == Board.Turn.HareTurn)
                {
                    _board.Hare &= ~(move << 4);
                    _board.Hare |= move;
                }
                else
                {
                    _board.Tortoise &= ~(move << 4);
                    _board.Tortoise |= move;
                }

                Tuple<int, int, Chess.Action> result = AlphaBeta((Board.Turn)((int)turn ^ 1), -beta, -alpha);
                if (-result.Item1 > value)
                {
                    value = -result.Item1;
                    position = BoardData.GetOneChessPosition(move << 4);
                    action = Chess.Action.Up;
                }

                if (turn == Board.Turn.HareTurn)
                {
                    _board.Hare |= move << 4;
                    _board.Hare &= ~move;
                }
                else
                {
                    _board.Tortoise |= move << 4;
                    _board.Tortoise &= ~move;
                }

                if (value >= beta)
                { ++_maxPly; return new Tuple<int, int, Chess.Action>(value, position, action); }
                alpha = Math.Max(alpha, value);
            }

            while (downMove != 0)
            {
                move = (downMove & -downMove);
                downMove &= ~move;
                if (turn == Board.Turn.HareTurn)
                {
                    _board.Hare &= ~(move >> 4);
                    _board.Hare |= move;
                }
                else
                {
                    _board.Tortoise &= ~(move >> 4);
                    _board.Tortoise |= move;
                }

                Tuple<int, int, Chess.Action> result = AlphaBeta((Board.Turn)((int)turn ^ 1), -beta, -alpha);
                if (-result.Item1 > value)
                {
                    value = -result.Item1;
                    position = BoardData.GetOneChessPosition(move >> 4);
                    action = Chess.Action.Down;
                }

                if (turn == Board.Turn.HareTurn)
                {
                    _board.Hare |= move >> 4;
                    _board.Hare &= ~move;
                }
                else
                {
                    _board.Tortoise |= move >> 4;
                    _board.Tortoise &= ~move;
                }

                if (value >= beta)
                { ++_maxPly; return new Tuple<int, int, Chess.Action>(value, position, action); }
                alpha = Math.Max(alpha, value);
            }

            ++_maxPly;
            return new Tuple<int, int, Chess.Action>(value, position, action);
        }

        public static int CountBit(int bit)
        {
            int count = 0;
            while (bit != 0)
            {
                if ((bit & 1) == 1) ++count;
                bit >>= 1;
            }
            return count;
        }

        public static int Eval(Board.Turn turn, BoardData board)
        {
            int score = 0;
            if (turn == Board.Turn.HareTurn)
            {
                score = 10000 *(3 - CountBit(board.Hare));
                for (int i = 0; i < 4; ++i)
                {
                    score += 100 * (i + 1) * CountBit(board.Tortoise & BoardData.Row[i]);
                    score -= 100 * (4 - i) * CountBit(board.Hare & BoardData.Column[i]);
                    score += (4 - i) * CountBit(board.Hare & BoardData.Row[i]);
                    score -= (i + 1) * CountBit(board.Tortoise & BoardData.Column[i]);
                }
            }
            else
            {
                score = 10000 * (3 - CountBit(board.Tortoise));
                for (int i = 0; i < 4; ++i)
                {
                    score += 100 * (4 - i) * CountBit(board.Hare & BoardData.Column[i]);
                    score -= 100 * (i + 1) * CountBit(board.Tortoise & BoardData.Row[i]);
                    score += (i + 1) * CountBit(board.Tortoise & BoardData.Column[i]);
                    score += (4 - i) * CountBit(board.Hare & BoardData.Row[i]);
                }
            }
            return score;
        }

        #endregion
    }
}
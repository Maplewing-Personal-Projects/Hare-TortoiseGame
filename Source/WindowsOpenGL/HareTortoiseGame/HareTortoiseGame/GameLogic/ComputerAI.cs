using System;
using HareTortoiseGame.Component;

namespace HareTortoiseGame.GameLogic
{
    public static class ComputerAI
    {

        #region Field

        static BoardData _board;
        static int _ply;
        static Board.Turn _nowTurn;
        static int[] _stuck;
        static int[,] _actionCount;
        const int MinValue = -9999999;
        const int MaxValue = 9999999;
        //static Random _random = new Random();
        #endregion

        #region Method

        public static void setComputerAI(BoardData initBoard, int maxPly, Board.Turn nowTurn)
        {
            _board = initBoard;
            _ply = maxPly;
            _nowTurn = nowTurn;
            _stuck = new int[] {0, 0};
            _actionCount = new int[,] { { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 } };
        }

        public static Tuple<int, Chess.Action> BestMove()
        {
            Tuple<int, int, Chess.Action> result = AlphaBeta(_nowTurn, MinValue, MaxValue);
            return new Tuple<int, Chess.Action>(result.Item2, result.Item3);
        }

        public static Tuple<int, int, Chess.Action> AlphaBeta(Board.Turn turn, int alpha, int beta)
        {
            --_ply;
            if (_board.TerminalTest() || _ply == 0)
            { ++_ply; return new Tuple<int, int, Chess.Action>(Eval(turn, _board), -1, Chess.Action.None); }
            
            int value = MinValue;
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
                ++_stuck[(int)turn];
                var result = AlphaBeta((Board.Turn)((int)turn ^ 1), -beta, -alpha);
                ++_ply;
                --_stuck[(int)turn];
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

                if (turn == Board.Turn.HareTurn) ++_actionCount[(int)turn, (int)Chess.Action.Right];
                else ++_actionCount[(int)turn, (int)Chess.Action.Up];
                Tuple<int,int,Chess.Action> result = AlphaBeta((Board.Turn)((int)turn ^ 1), -beta, -alpha);
                if (turn == Board.Turn.HareTurn) --_actionCount[(int)turn, (int)Chess.Action.Right];
                else --_actionCount[(int)turn, (int)Chess.Action.Up];
                
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
                { ++_ply; return new Tuple<int, int, Chess.Action>(value, position, action); }
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
                ++_actionCount[(int)turn, (int)Chess.Action.Down];
                Tuple<int, int, Chess.Action> result = AlphaBeta((Board.Turn)((int)turn ^ 1), -beta, -alpha);
                --_actionCount[(int)turn, (int)Chess.Action.Down];
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
                { ++_ply; return new Tuple<int, int, Chess.Action>(value, position, action); }
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

                ++_actionCount[(int)turn, (int)Chess.Action.Left];
                Tuple<int, int, Chess.Action> result = AlphaBeta((Board.Turn)((int)turn ^ 1), -beta, -alpha);
                --_actionCount[(int)turn, (int)Chess.Action.Left]; 
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
                { ++_ply; return new Tuple<int, int, Chess.Action>(value, position, action); }
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

                ++_actionCount[(int)turn, (int)Chess.Action.Right];
                Tuple<int, int, Chess.Action> result = AlphaBeta((Board.Turn)((int)turn ^ 1), -beta, -alpha);
                --_actionCount[(int)turn, (int)Chess.Action.Right]; 
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
                { ++_ply; return new Tuple<int, int, Chess.Action>(value, position, action); }
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

                ++_actionCount[(int)turn, (int)Chess.Action.Up];
                Tuple<int, int, Chess.Action> result = AlphaBeta((Board.Turn)((int)turn ^ 1), -beta, -alpha);
                --_actionCount[(int)turn, (int)Chess.Action.Up]; 
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
                { ++_ply; return new Tuple<int, int, Chess.Action>(value, position, action); }
                alpha = Math.Max(alpha, value);
            }

            ++_ply;
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
            int hareScore = 16, tortoiseScore = 16;
            
            for (int i = 0; i < 4; ++i)
            {
                hareScore -= (4 - i) * CountBit(board.Hare & BoardData.Column[i]);
                tortoiseScore -= (i + 1) * CountBit(board.Tortoise & BoardData.Row[i]);
            }

            if (board.Hare == 0) hareScore += 100;
            else if (board.Tortoise == 0) tortoiseScore += 100;

            if (turn == Board.Turn.HareTurn) return hareScore - tortoiseScore;
            else return tortoiseScore - hareScore;
        }

        #endregion
    }
}
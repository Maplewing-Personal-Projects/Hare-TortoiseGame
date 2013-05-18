using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using HareTortoiseGame.State;
using HareTortoiseGame.GameLogic;

namespace HareTortoiseGame.Component
{
    public class Board : Container
    {
        #region Field
        public const int TotalChessButton = 16; 
        BoardData _boardData = new BoardData();
        ChessButton[] _chessbutton;
        Chess[] _hare;
        Chess[] _tortoise;

        #endregion

        #region Constructor

        public Board(Game game, Texture2D picture, DrawState state, BoardData boardData = null)
            : base(game, picture, state)
        {
            if( boardData != null ) _boardData = boardData;
            _chessbutton = new ChessButton[TotalChessButton];
            for (int i = 0; i < TotalChessButton; ++i)
            {
                _chessbutton[i] = new ChessButton(game, game.Content.Load<Texture2D>("blank"),
                    new DrawState(game, new Vector4(0.025f + 0.25f* (i % 4),0.025f + 0.25f * (i / 4), 0.23f, 0.24f), new Color(0.0f, 0.0f, 0.0f, 0.5f)));
                AddComponent(_chessbutton[i]);
            }

            int index = 0;
            _hare = new Chess[3];
            int hareChess = _boardData.Hare;
            while (hareChess != 0)
            {
                int oneChess = (hareChess & -hareChess);
                int position = BoardData.GetOneChessPosition(oneChess);
                hareChess &= ~oneChess;
                   
                _hare[index] = new Chess(game, game.Content.Load<Texture2D>("Rabbit"), new DrawState(
                    game, new Vector4(0.025f + 0.25f * (position % 4), 0.025f + 0.25f * (position / 4), 0.23f, 0.24f),
                    Color.White), position % 4, position / 4);
                AddComponent(_hare[index]);
                ++index;
            }

            index = 0;
            _tortoise = new Chess[3];
            int tortoiseChess = _boardData.Tortoise;
            while (tortoiseChess != 0)
            {
                int oneChess = (tortoiseChess & -tortoiseChess);
                int position = BoardData.GetOneChessPosition(oneChess);
                tortoiseChess &= ~oneChess;

                _tortoise[index] = new Chess(game, game.Content.Load<Texture2D>("Turtle"), new DrawState(
                    game, new Vector4(0.025f + 0.25f * (position % 4), 0.025f + 0.25f * (position / 4), 0.23f, 0.24f),
                    Color.White), position % 4, position / 4);
                AddComponent(_tortoise[index]);
                ++index;
            }

        }

        #endregion
    }
}

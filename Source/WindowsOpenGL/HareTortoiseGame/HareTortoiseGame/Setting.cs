using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using HareTortoiseGame.Component;
using HareTortoiseGame.GameLogic;

namespace HareTortoiseGame
{
    public static class Setting
    {
        #region Field
        static int _maxPlyOrSecond = 12;
        static Board.Player[] _players = { Board.Player.User, Board.Player.Computer };
        static int _maxEdgeCount = 4;
        static int _soundVolume = 100;
        static int _musicVolume = 30;
        #endregion

        #region Property
        public static int MaxPly { get { return _maxPlyOrSecond; } set { _maxPlyOrSecond = value; } }
        public static Board.Player[] Players { get { return _players; } set { _players = value; } }
        public static int MaxEdgeCount { 
            get { return _maxEdgeCount; } 
            set { 
                _maxEdgeCount = value;

                BoardData.Row = new ulong[_maxEdgeCount];
                ulong row = 0;
                for (int i = 0; i < _maxEdgeCount; ++i)
                {
                    row <<= 1;
                    row |= 1;
                }
                for (int i = 0; i < _maxEdgeCount; ++i)
                {
                    BoardData.Row[i] = row;
                    row <<= _maxEdgeCount;
                }

                BoardData.Column = new ulong[_maxEdgeCount];
                ulong column = 0;
                for (int i = 0; i < _maxEdgeCount; ++i)
                {
                    column <<= Setting.MaxEdgeCount;
                    column |= 1;
                }
                for (int i = 0; i < _maxEdgeCount; ++i)
                {
                    BoardData.Column[i] = column;
                    column <<= 1;
                }
            } 
        }
        public static int SoundVolume { get { return _soundVolume; } set { _soundVolume = value; } }
        public static int MusicVolume { get { return _musicVolume; } set { _musicVolume = value; } }
        #endregion

        #region Constructor
        #endregion

        #region Method

        public static void LoadMusic(Game game)
        {
            LoadSong.SongInitialize();
            foreach (var song in LoadSong.Songlist)
            {
                game.Content.Load<Song>(song);
            }
        }
        #endregion
    }
}
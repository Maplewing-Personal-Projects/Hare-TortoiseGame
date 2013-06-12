// 人工智慧作業三 龜兔賽跑程式之研發                
// 資工系103級 499470098 曹又霖
// 使用方法：                                       
// 使用Visual Studio 2012並灌入MonoGame後即可打開並編譯。
// 執行方法：
// 需先灌入.NET Framework 4 和 OpenAL 後，即可打開。
// 信箱：sinmaplewing@gmail.com

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
            //LoadSong.SongInitialize();
            foreach (var song in LoadSong.Songlist)
            {
                game.Content.Load<Song>(song);
            }
        }
        #endregion
    }
}
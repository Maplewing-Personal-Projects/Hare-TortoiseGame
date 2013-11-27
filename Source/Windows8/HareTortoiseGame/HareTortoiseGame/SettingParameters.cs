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
    public static class SettingParameters
    {
        #region Field
        public static RealSettingParameters rsp = new RealSettingParameters();
        #endregion

        #region Property
        public static int MaxPly { get { return rsp.MaxPly; } set { rsp.MaxPly = value; } }
        public static Board.Player[] Players { get { return rsp.Players; } set { rsp.Players = value; } }
        public static int MaxEdgeCount { 
            get { return rsp.MaxEdgeCount; } 
            set { 
                rsp.MaxEdgeCount = value;
            } 
        }
        public static int SoundVolume { get { return rsp.SoundVolume; } set { rsp.SoundVolume = value; } }
        public static int MusicVolume { get { return rsp.MusicVolume; } set { rsp.MusicVolume = value; } } 
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
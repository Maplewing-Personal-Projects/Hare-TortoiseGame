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
using Windows.UI.Notifications;

namespace HareTortoiseGame
{
    public static class SettingParameters
    {
        #region Field
        public static bool update = false;
        public static int HareScore = 0;
        public static int TortoiseScore = 0;
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
        public static bool GameWait { get { return rsp.GameWait; } set { rsp.GameWait = value; } }
        
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

        public static void Save()
        {
            Windows.Storage.ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            roamingSettings.Values["maxPly"] = MaxPly;
            roamingSettings.Values["players0"] = (int)Players[0];
            roamingSettings.Values["players1"] = (int)Players[1];
            roamingSettings.Values["maxEdgeCount"] = MaxEdgeCount;
            roamingSettings.Values["soundVolume"] = SoundVolume;
            roamingSettings.Values["musicVolume"] = MusicVolume;
            roamingSettings.Values["hareScore"] = HareScore;
            roamingSettings.Values["tortoiseScore"] = TortoiseScore;
        }

        public static void UpdateTile()
        {
            // create a string with the tile template xml
            var tileContent = NotificationsExtensions.TileContent.TileContentFactory.CreateTileWideImageAndText01();
            tileContent.TextCaptionWrap.Text = "烏龜獲勝：" + TortoiseScore + "次\n兔子獲勝：" + HareScore + "次";
            tileContent.Image.Src = "ms-appx:///Assets/WideLogo.scale-100.png";
            tileContent.Image.Alt = "Logo";

            var squareTileContent = NotificationsExtensions.TileContent.TileContentFactory.CreateTileSquareText03();
            squareTileContent.TextBody1.Text = "烏龜獲勝：" + TortoiseScore + "次";
            squareTileContent.TextBody2.Text = "兔子獲勝：" + HareScore + "次";
            
            tileContent.SquareContent = squareTileContent;
            /*
            string tileXmlString = "<toast>"
                              + "<visual>"
                              + "<binding template='TileWideImageAndText01'>"
                              + "<text id='1'>烏龜獲勝：" + TortoiseScore + "次\n兔子獲勝：" + HareScore + "次</text>"
                              + "<image id='1' src='ms-appx:///Assets/WideLogo.scale-100.png' alt='Logo' />"
                              + "</binding>"
                              + "</visual>"
                              + "</toast>";

            
            // create a DOM
            Windows.Data.Xml.Dom.XmlDocument tileDOM = new Windows.Data.Xml.Dom.XmlDocument();
            // load the xml string into the DOM, catching any invalid xml characters 
            tileDOM.LoadXml(tileXmlString);
            
            // create a tile notification
            TileNotification tile = new TileNotification(tileDOM);
            */
            // send the notification to the app's application tile
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileContent.CreateNotification());
            /*
            Windows.Data.Xml.Dom.XmlDocument toastXml = new Windows.Data.Xml.Dom.XmlDocument();
            toastXml.LoadXml(tileXmlString);

            ToastNotification toast = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
            */
        }
        #endregion
    }
}
// 人工智慧作業三 龜兔賽跑程式之研發                
// 資工系103級 499470098 曹又霖
// 使用方法：                                       
// 使用Visual Studio 2012並灌入MonoGame後即可打開並編譯。
// 執行方法：
// 需先灌入.NET Framework 4 和 OpenAL 後，即可打開。
// 信箱：sinmaplewing@gmail.com

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HareTortoiseGame.Component;
using HareTortoiseGame.State;
using Windows.UI.ApplicationSettings;

namespace HareTortoiseGame.PackageScene
{
    class GameStartMenu : Scene
    {
        #region Field

        Song _backgroundSong;
        SoundEffect _click;

        GraphComponent _logo;
        FontComponent _startGame;
        FontComponent _settingGame;
        FontComponent _exitGame;
        FontComponent _copyRight;

        bool _startHover;
        bool _settingHover;
        bool _exitHover;
        #endregion

        public GameStartMenu(Game game)
            : base(game,
                game.Content.Load<Texture2D>("blank"),
                new DrawState(game, new Vector4(2f, 0f, 1f, 1f), Color.Gray))
        {
            _startHover = false;
            _settingHover = false;
            _exitHover = false;

            _backgroundSong = game.Content.Load<Song>("EmeraldHillClassic");
            if (MediaPlayer.Queue.ActiveSong != _backgroundSong || 
                MediaPlayer.State == MediaState.Stopped) 
                MediaPlayer.Play(_backgroundSong);

            _click = game.Content.Load<SoundEffect>("misc_menu_4");

            _logo = new GraphComponent( game, game.Content.Load<Texture2D>("logo"),
                new DrawState(game, new Vector4(0.2f, 0.3f, 0.6f, 0.4f), Color.White));
            _logo.AddState(0.5f, new DrawState(game, new Vector4(0.2f, 0.3f, 0.6f, 0.4f), Color.White));
            _logo.AddState(0.5f, new DrawState(game, new Vector4(0.2f, 0.05f, 0.6f, 0.4f), Color.White));

            _startGame = new FontComponent(game, game.Content.Load<SpriteFont>("Font"),
                new DrawState(game, new Vector4(0.4f, 0.5f, 0f, 0f), Color.White));
            _startGame.Content = "開始遊戲";
            _startGame.AddState(0.5f, new DrawState(game, new Vector4(0.4f, 0.6f, 0f, 0f), Color.White));
            _startGame.AddState(0.5f, new DrawState(game, new Vector4(0.4f, 0.6f, 0.2f, 0f), Color.White));

            _settingGame = new FontComponent(game, game.Content.Load<SpriteFont>("Font"),
                new DrawState(game, new Vector4(0.4f, 0.6f, 0f, 0f), Color.White));
            _settingGame.Content = "遊戲設定";
            _settingGame.AddState(0.5f, new DrawState(game, new Vector4(0.4f, 0.75f, 0f, 0f), Color.White));
            _settingGame.AddState(0.5f, new DrawState(game, new Vector4(0.4f, 0.75f, 0.2f, 0f), Color.White));

            /*
            _exitGame = new FontComponent(game, game.Content.Load<SpriteFont>("Font"),
                new DrawState(game, new Vector4(0.4f, 0.7f, 0f, 0f), Color.White));
            _exitGame.Content = "離開遊戲";
            _exitGame.AddState(0.5f, new DrawState(game, new Vector4(0.4f, 0.7f, 0f, 0f), Color.White));
            _exitGame.AddState(0.5f, new DrawState(game, new Vector4(0.4f, 0.7f, 0.2f, 0f), Color.White));
            */

            /*
            _copyRight = new FontComponent(game, game.Content.Load<SpriteFont>("font"),
                new DrawState(game, new Vector4(0.5f, 0.8f, 0.0f, 0.0f), Color.White));
            _copyRight.Content = "製作：灆洢（曹又霖）";
            _copyRight.AddState(0.5f, new DrawState(game, new Vector4(0.5f, 0.8f, 0.0f, 0.0f), Color.White));
            _copyRight.AddState(0.5f, new DrawState(game, new Vector4(0.25f, 0.9f, 0.5f, 0.0f), Color.White));
            */

            // AddComponent(_copyRight);
            AddComponent(_logo);
            AddComponent(_startGame);
            AddComponent(_settingGame);
            //AddComponent(_exitGame);
        }

        #region Method

        public override void Initialize()
        {
            _state.AddState(0.5f, new DrawState(Game, new Vector4(0, 0, 1, 1), Color.Gray));
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (_startGame.IsHover() && !_startHover)
            {
                _startGame.ClearAllAndAddState(0.2f, new DrawState(Game,
                    new Vector4(0.4f, 0.6f, 0.2f, 0f), Color.PowderBlue));
                _startHover = true;
            }
            else if( !_startGame.IsHover() && _startHover)
            {
                _startGame.ClearAllAndAddState(0.2f, new DrawState(Game,
                    new Vector4(0.4f, 0.6f, 0.2f, 0f), Color.White));
                _startHover = false;
            }

            if (_settingGame.IsHover() && !_settingHover)
            {
                _settingGame.ClearAllAndAddState(0.2f, new DrawState(Game,
                    new Vector4(0.4f, 0.75f, 0.2f, 0f), Color.PowderBlue));
                _settingHover = true;
            }
            else if (!_settingGame.IsHover() && _settingHover)
            {
                _settingGame.ClearAllAndAddState(0.2f, new DrawState(Game,
                    new Vector4(0.4f, 0.75f, 0.2f, 0f), Color.White));
                _settingHover = false;
            }
            /*
            if (_exitGame.IsHover() && !_exitHover)
            {
                _exitGame.ClearAllAndAddState(0.2f, new DrawState(Game,
                    new Vector4(0.4f, 0.7f, 0.2f, 0f), Color.PowderBlue));
                _exitHover = true;
            }
            else if (!_exitGame.IsHover() && _exitHover)
            {
                _exitGame.ClearAllAndAddState(0.2f, new DrawState(Game,
                    new Vector4(0.4f, 0.7f, 0.2f, 0f), Color.White));
                _exitHover = false;
            }
            */

            if (_startGame.IsHit())
            {
                _click.Play();
                MediaPlayer.Stop();
                NextScene = "Board";
            }
            if (_settingGame.IsHit())
            {
                _click.Play();
                SettingsPane.Show();
            }
            /*
            if (_exitGame.IsHit()) 
            {
                _click.Play();
                Game.Exit(); 
            }
            */
            base.Update(gameTime);
        }
        #endregion
    }
}

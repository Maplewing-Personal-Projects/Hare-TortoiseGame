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
using HareTortoiseGame.State;
using HareTortoiseGame.Component;

namespace HareTortoiseGame.PackageScene
{
    public class SettingScene : Scene
    {
        #region Field
        FontComponent _settingView;

        GraphComponent _plyAdd;
        GraphComponent _plyMinus;
        FontComponent _ply;

        GraphComponent _tortoise;
        GraphComponent _hare;
        GraphComponent _hareComputer;
        GraphComponent _hareUser;
        GraphComponent _tortoiseUser;
        GraphComponent _tortoiseComputer;

        GraphComponent _next;
        GraphComponent _back;

        Song _backgroundSong;
        SoundEffect _click;
        SoundEffect _clickError;
        SoundEffect _start;

        Board.Player[] _previousPlayers;

        bool _plyAddHover;
        bool _plyMinusHover;
        bool _tortoiseUserHover;
        bool _tortoiseComputerHover;
        bool _hareUserHover;
        bool _hareComputerHover;
        bool _nextHover;
        bool _backHover;

        #endregion

        #region Constructor

        public SettingScene(Game game)
            : base(game,
                game.Content.Load<Texture2D>("blank"),
                new DrawState(game, new Vector4(2f, 0f, 1f, 1f), Color.Gray))
        {
            _backgroundSong = LoadSong.Load(game,"EmeraldHillClassic");
            if (MediaPlayer.Queue.ActiveSong != _backgroundSong ||
                MediaPlayer.State == MediaState.Stopped)
                MediaPlayer.Play(_backgroundSong);

            _plyAddHover = false;
            _plyMinusHover = false;
            _tortoiseUserHover = false;
            _tortoiseComputerHover = false;
            _hareUserHover = false;
            _hareComputerHover = false;

            _click = game.Content.Load<SoundEffect>("misc_menu_4");
            _clickError = game.Content.Load<SoundEffect>("negative_2");
            _start = game.Content.Load<SoundEffect>("save");

            _settingView = new FontComponent( game, game.Content.Load<SpriteFont>( "font" ),
                new DrawState(game, new Vector4( 0.05f, 0.05f, 0.0f, 0.0f), Color.White));
            _settingView.AddState(0.5f, new DrawState(game, new Vector4(0.05f, 0.025f, 0.0f, 0.0f), Color.White));
            _settingView.AddState(0.5f, new DrawState(game, new Vector4(0.05f, 0.05f, 0.2f, 0.0f), Color.White));
            _settingView.Content = "開始設定";
            _ply = new FontComponent( game, game.Content.Load<SpriteFont>( "font" ),
                new DrawState(game, new Vector4(0.25f, 0.15f, 0.0f, 0.0f), Color.White));
            _ply.AddState(0.5f, new DrawState(game, new Vector4(0.25f, 0.15f, 0.0f, 0.0f), Color.White));
            _ply.AddState(0.5f, new DrawState(game, new Vector4(0.25f, 0.3f, 0.3f, 0.05f), Color.White));
            _ply.Content = "Alpha-Beta最高層數: " + Setting.MaxPly.ToString();
            _plyAdd = new GraphComponent(game, game.Content.Load<Texture2D>("Add"),
                new DrawState(game, new Vector4(0.8f, 0.28f, 0.0f, 0.0f), Color.White));
            _plyAdd.AddState(0.5f, new DrawState(game, new Vector4(0.8f, 0.28f, 0.0f, 0.0f), Color.White));
            _plyAdd.AddState(0.5f, new DrawState(game, new Vector4(0.8f, 0.28f, 0.1f, 0.1f), Color.White));
            _plyMinus = new GraphComponent(game, game.Content.Load<Texture2D>("Minus"),
                new DrawState(game, new Vector4(0.1f, 0.28f, 0.0f, 0.0f), Color.White));
            _plyMinus.AddState(0.5f, new DrawState(game, new Vector4(0.1f, 0.28f, 0.0f, 0.0f), Color.White));
            _plyMinus.AddState(0.5f, new DrawState(game, new Vector4(0.1f, 0.28f, 0.1f, 0.1f), Color.White));

            _tortoise = new GraphComponent(game, game.Content.Load<Texture2D>("Turtle"),
                new DrawState(game, new Vector4(0.2f, 0.45f, 0.13f, 0.2f), Color.White));
            _hare = new GraphComponent(game, game.Content.Load<Texture2D>("Rabbit"),
                new DrawState(game, new Vector4(0.2f, 0.65f, 0.13f, 0.2f), Color.White));
            _tortoiseUser = new GraphComponent(game, game.Content.Load<Texture2D>("User"),
                new DrawState(game, new Vector4(0.5f, 0.45f, 0.13f, 0.2f), Color.White));
            _tortoiseComputer = new GraphComponent(game, game.Content.Load<Texture2D>("Computer"),
                new DrawState(game, new Vector4(0.7f, 0.45f, 0.13f, 0.2f), Color.White));
            _hareUser = new GraphComponent(game, game.Content.Load<Texture2D>("User"),
                new DrawState(game, new Vector4(0.5f, 0.65f, 0.13f, 0.2f), Color.White));
            _hareComputer = new GraphComponent(game, game.Content.Load<Texture2D>("Computer"),
                new DrawState(game, new Vector4(0.7f, 0.65f, 0.13f, 0.2f), Color.White));
            _previousPlayers = null;

            _next = new GraphComponent(game, game.Content.Load<Texture2D>("Next"),
                new DrawState(game, new Vector4(0.83f, 0.05f, 0.13f, 0.2f), Color.White));
            _back = new GraphComponent(game, game.Content.Load<Texture2D>("Previous"),
                new DrawState(game, new Vector4(0.68f, 0.05f, 0.13f, 0.2f), Color.White));

            AddComponent(_settingView);
            AddComponent(_ply);
            AddComponent(_plyAdd);
            AddComponent(_plyMinus);
            AddComponent(_tortoise);
            AddComponent(_hare);
            AddComponent(_tortoiseComputer);
            AddComponent(_tortoiseUser);
            AddComponent(_hareUser);
            AddComponent(_hareComputer);
            AddComponent(_next);
            AddComponent(_back);
        }

        #endregion

        #region Method

        public override void Initialize()
        {
            _state.AddState(0.5f, new DrawState(Game, new Vector4(0, 0, 1, 1), Color.Gray));
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {

            if (_previousPlayers == null)
            {
                _previousPlayers = new Board.Player[2];
                _previousPlayers[0] = Setting.Players[0];
                _previousPlayers[1] = Setting.Players[1];
                if (_previousPlayers[0] == Board.Player.Computer)
                {
                    _tortoiseComputer.ClearAllAndAddState( 0.2f, 
                        new DrawState(Game, new Vector4(0.7f, 0.45f, 0.13f, 0.2f), Color.Red)); 
                }
                else
                {
                    _tortoiseUser.ClearAllAndAddState( 0.2f,
                        new DrawState(Game, new Vector4(0.5f, 0.45f, 0.13f, 0.2f), Color.Red));
                }
                if (_previousPlayers[1] == Board.Player.Computer)
                {
                    _hareComputer.ClearAllAndAddState( 0.2f,
                        new DrawState(Game, new Vector4(0.7f, 0.65f, 0.13f, 0.2f), Color.Red));
                }
                else
                {
                    _hareUser.ClearAllAndAddState( 0.2f,
                        new DrawState(Game, new Vector4(0.5f, 0.65f, 0.13f, 0.2f), Color.Red));
                }
            }

            if (_previousPlayers[0] != Setting.Players[0])
            {
                _previousPlayers[0] = Setting.Players[0];
                
                if (_previousPlayers[0] == Board.Player.Computer)
                {
                    _tortoiseUser.ClearAllAndAddState(0.2f,
                        new DrawState(Game, new Vector4(0.5f, 0.45f, 0.13f, 0.2f), Color.White));
                    _tortoiseComputer.ClearAllAndAddState(0.2f,
                        new DrawState(Game, new Vector4(0.7f, 0.45f, 0.13f, 0.2f), Color.Red));
                }
                else
                {
                    _tortoiseComputer.ClearAllAndAddState(0.2f,
                        new DrawState(Game, new Vector4(0.7f, 0.45f, 0.13f, 0.2f), Color.White));
                    _tortoiseUser.ClearAllAndAddState(0.2f,
                        new DrawState(Game, new Vector4(0.5f, 0.45f, 0.13f, 0.2f), Color.Red));
                }
            }

            if (_previousPlayers[1] != Setting.Players[1])
            {
                _previousPlayers[1] = Setting.Players[1];

                if (_previousPlayers[1] == Board.Player.Computer)
                {
                    _hareUser.ClearAllAndAddState(0.2f,
                        new DrawState(Game, new Vector4(0.5f, 0.65f, 0.13f, 0.2f), Color.White));
                    _hareComputer.ClearAllAndAddState(0.2f,
                        new DrawState(Game, new Vector4(0.7f, 0.65f, 0.13f, 0.2f), Color.Red));
                }
                else
                {
                    _hareComputer.ClearAllAndAddState(0.2f,
                        new DrawState(Game, new Vector4(0.7f, 0.65f, 0.13f, 0.2f), Color.White));
                    _hareUser.ClearAllAndAddState(0.2f,
                        new DrawState(Game, new Vector4(0.5f, 0.65f, 0.13f, 0.2f), Color.Red));
                }
            }

            _ply.Content = "Alpha-Beta最高層數: " + Setting.MaxPly.ToString();
            if (Setting.MaxPly == 12) _ply.Content += "（兔子建議）";
            if (Setting.MaxPly == 16) _ply.Content += "（烏龜建議）";
            if (_plyAdd.IsHit())
            {
                _plyAddHover = false;
                _click.Play();
                _plyAdd.ClearAllAndAddState(0.1f,
                    new DrawState(Game, new Vector4(0.8f, 0.28f, 0.1f, 0.1f), Color.Red));
                _plyAdd.AddState(0.1f,
                    new DrawState(Game, new Vector4(0.8f, 0.28f, 0.1f, 0.1f), Color.White));
                ++Setting.MaxPly;
            }
            else if (_plyAdd.IsHover() && !_plyAddHover && _plyAdd.IsFinish() )
            {
                _plyAddHover = true;
                _plyAdd.ClearAllAndAddState(0.2f,
                    new DrawState(Game, new Vector4(0.8f, 0.28f, 0.1f, 0.1f), Color.PowderBlue));
            }
            else if (!_plyAdd.IsHover() && _plyAddHover && _plyAdd.IsFinish())
            {
                _plyAddHover = false;
                _plyAdd.ClearAllAndAddState(0.2f,
                    new DrawState(Game, new Vector4(0.8f, 0.28f, 0.1f, 0.1f), Color.White));
            }

            if (_plyMinus.IsHit() && Setting.MaxPly >= 3)
            {
                _plyMinusHover = false;
                _click.Play();
                _plyMinus.ClearAllAndAddState(0.1f,
                    new DrawState(Game, new Vector4(0.1f, 0.28f, 0.1f, 0.1f), Color.Red));
                _plyMinus.AddState(0.1f,
                    new DrawState(Game, new Vector4(0.1f, 0.28f, 0.1f, 0.1f), Color.White));
                --Setting.MaxPly;
            }
            else if (_plyMinus.IsHit())
            {
                _clickError.Play();
            }
            else if (_plyMinus.IsHover() && !_plyMinusHover && _plyMinus.IsFinish())
            {
                _plyMinusHover = true;
                _plyMinus.ClearAllAndAddState(0.2f,
                    new DrawState(Game, new Vector4(0.1f, 0.28f, 0.1f, 0.1f), Color.PowderBlue));
            }
            else if (!_plyMinus.IsHover() && _plyMinusHover && _plyMinus.IsFinish())
            {
                _plyMinusHover = false;
                _plyMinus.ClearAllAndAddState(0.2f,
                    new DrawState(Game, new Vector4(0.1f, 0.28f, 0.1f, 0.1f), Color.White));
            }

            if (_tortoiseUser.IsHit())
            {
                _tortoiseUserHover = false;
                _click.Play();
                Setting.Players[0] = Board.Player.User;
            }
            else if (_tortoiseUser.IsHover() && !_tortoiseUserHover && Setting.Players[0] != Board.Player.User)
            {
                _tortoiseUserHover = true;
                _tortoiseUser.ClearAllAndAddState(0.2f,
                        new DrawState(Game, new Vector4(0.5f, 0.45f, 0.13f, 0.2f), Color.PowderBlue));
            }
            else if (!_tortoiseUser.IsHover() && _tortoiseUserHover && Setting.Players[0] != Board.Player.User)
            {
                _tortoiseUserHover = false;
                _tortoiseUser.ClearAllAndAddState(0.2f,
                        new DrawState(Game, new Vector4(0.5f, 0.45f, 0.13f, 0.2f), Color.White));
            }

            if (_tortoiseComputer.IsHit()) {
                _tortoiseComputerHover = false;
                _click.Play(); 
                Setting.Players[0] = Board.Player.Computer;
            }
            else if (_tortoiseComputer.IsHover() && !_tortoiseComputerHover && Setting.Players[0] != Board.Player.Computer)
            {
                _tortoiseComputerHover = true;
                _tortoiseComputer.ClearAllAndAddState(0.2f,
                        new DrawState(Game, new Vector4(0.7f, 0.45f, 0.13f, 0.2f), Color.PowderBlue));
            }
            else if (!_tortoiseComputer.IsHover() && _tortoiseComputerHover && Setting.Players[0] != Board.Player.Computer)
            {
                _tortoiseComputerHover = false;
                _tortoiseComputer.ClearAllAndAddState(0.2f,
                        new DrawState(Game, new Vector4(0.7f, 0.45f, 0.13f, 0.2f), Color.White));
            }

            if (_hareUser.IsHit()) {
                _hareUserHover = false;
                _click.Play(); 
                Setting.Players[1] = Board.Player.User; 
            }
            else if (_hareUser.IsHover() && !_hareUserHover && Setting.Players[1] != Board.Player.User)
            {
                _hareUserHover = true;
                _hareUser.ClearAllAndAddState(0.2f,
                        new DrawState(Game, new Vector4(0.5f, 0.65f, 0.13f, 0.2f), Color.PowderBlue));
            }
            else if (!_hareUser.IsHover() && _hareUserHover && Setting.Players[1] != Board.Player.User)
            {
                _hareUserHover = false;
                _hareUser.ClearAllAndAddState(0.2f,
                        new DrawState(Game, new Vector4(0.5f, 0.65f, 0.13f, 0.2f), Color.White));
            }

            if (_hareComputer.IsHit()) 
            {
                _hareComputerHover = false;
                _click.Play();
                Setting.Players[1] = Board.Player.Computer; 
            }
            else if (_hareComputer.IsHover() && !_hareComputerHover && Setting.Players[1] != Board.Player.Computer)
            {
                _hareComputerHover = true;
                _hareComputer.ClearAllAndAddState(0.2f,
                        new DrawState(Game, new Vector4(0.7f, 0.65f, 0.13f, 0.2f), Color.PowderBlue));
            }
            else if (!_hareComputer.IsHover() && _hareComputerHover && Setting.Players[1] != Board.Player.Computer)
            {
                _hareComputerHover = false;
                _hareComputer.ClearAllAndAddState(0.2f,
                        new DrawState(Game, new Vector4(0.7f, 0.65f, 0.13f, 0.2f), Color.White));
            }

            if (_next.IsHit())
            {
                _nextHover = false;
                _start.Play();
                MediaPlayer.Stop();
                NextScene = "Board";
            }
            else if (_next.IsHover() && !_nextHover)
            {
                _nextHover = true;
                _next.AddState(0.2f,
                    new DrawState(Game, new Vector4(0.83f, 0.05f, 0.13f, 0.2f), Color.PowderBlue));
            }
            else if (!_next.IsHover() && _nextHover)
            {
                _nextHover = false;
                _next.AddState(0.2f,
                    new DrawState(Game, new Vector4(0.83f, 0.05f, 0.13f, 0.2f), Color.White));
            }
            

            if (_back.IsHit())
            {
                _start.Play();
                NextScene = "GameStart";
            }
            else if (_back.IsHover() && !_backHover)
            {
                _backHover = true;
                _back.AddState(0.2f,
                    new DrawState(Game, new Vector4(0.68f, 0.05f, 0.13f, 0.2f), Color.PowderBlue));
            }
            else if (!_back.IsHover() && _backHover)
            {
                _backHover = false;
                _back.AddState(0.2f,
                    new DrawState(Game, new Vector4(0.68f, 0.05f, 0.13f, 0.2f), Color.White));
            }

            base.Update(gameTime);
        }

        #endregion

    }
}

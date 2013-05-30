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
    public class DisplaySoundSetting : Scene
    {
        #region Field
        FontComponent _settingView;

        GraphComponent _soundAdd;
        GraphComponent _soundMinus;
        FontComponent _sound;

        GraphComponent _musicAdd;
        GraphComponent _musicMinus;
        FontComponent _music;

        GraphComponent _back;

        Song _backgroundSong;
        SoundEffect _click;
        SoundEffect _clickError;
        SoundEffect _start;

        bool _soundAddHover;
        bool _soundMinusHover;
        bool _musicAddHover;
        bool _musicMinusHover;
        bool _backHover;

        #endregion

        #region Constructor

        public DisplaySoundSetting(Game game)
            : base(game,
                game.Content.Load<Texture2D>("blank"),
                new DrawState(game, new Vector4(2f, 0f, 1f, 1f), Color.Gray))
        {
            _backgroundSong = LoadSong.Load(game, "EmeraldHillClassic");
            if (MediaPlayer.Queue.ActiveSong != _backgroundSong ||
                MediaPlayer.State == MediaState.Stopped)
                MediaPlayer.Play(_backgroundSong);

            _soundAddHover = false;
            _soundMinusHover = false;
            _musicAddHover = false;
            _musicMinusHover = false;
            _backHover = false;

            _click = game.Content.Load<SoundEffect>("misc_menu_4");
            _clickError = game.Content.Load<SoundEffect>("negative_2");
            _start = game.Content.Load<SoundEffect>("save");

            _settingView = new FontComponent(game, game.Content.Load<SpriteFont>("font"),
                new DrawState(game, new Vector4(0.05f, 0.05f, 0.0f, 0.0f), Color.White));
            _settingView.AddState(0.5f, new DrawState(game, new Vector4(0.05f, 0.025f, 0.0f, 0.0f), Color.White));
            _settingView.AddState(0.5f, new DrawState(game, new Vector4(0.05f, 0.05f, 0.1f, 0.0f), Color.White));
            _settingView.Content = "設定";
            _sound = new FontComponent(game, game.Content.Load<SpriteFont>("font"),
                new DrawState(game, new Vector4(0.4f, 0.15f, 0.0f, 0.0f), Color.White));
            _sound.AddState(0.5f, new DrawState(game, new Vector4(0.4f, 0.15f, 0.0f, 0.0f), Color.White));
            _sound.AddState(0.5f, new DrawState(game, new Vector4(0.4f, 0.3f, 0.2f, 0.0f), Color.White));
            _sound.Content = "音效: " + Setting.SoundVolume.ToString() + "%";
            _soundAdd = new GraphComponent(game, game.Content.Load<Texture2D>("Add"),
                new DrawState(game, new Vector4(0.8f, 0.28f, 0.0f, 0.0f), Color.White));
            _soundAdd.AddState(0.5f, new DrawState(game, new Vector4(0.8f, 0.28f, 0.0f, 0.0f), Color.White));
            _soundAdd.AddState(0.5f, new DrawState(game, new Vector4(0.8f, 0.28f, 0.1f, 0.1f), Color.White));
            _soundMinus = new GraphComponent(game, game.Content.Load<Texture2D>("Minus"),
                new DrawState(game, new Vector4(0.1f, 0.28f, 0.0f, 0.0f), Color.White));
            _soundMinus.AddState(0.5f, new DrawState(game, new Vector4(0.1f, 0.28f, 0.0f, 0.0f), Color.White));
            _soundMinus.AddState(0.5f, new DrawState(game, new Vector4(0.1f, 0.28f, 0.1f, 0.1f), Color.White));

            _music = new FontComponent(game, game.Content.Load<SpriteFont>("font"),
                new DrawState(game, new Vector4(0.4f, 0.25f, 0.0f, 0.0f), Color.White));
            _music.AddState(0.5f, new DrawState(game, new Vector4(0.4f, 0.25f, 0.0f, 0.0f), Color.White));
            _music.AddState(0.5f, new DrawState(game, new Vector4(0.4f, 0.4f, 0.2f, 0.0f), Color.White));
            _music.Content = "音樂: " + Setting.MusicVolume.ToString() + "%";
            _musicAdd = new GraphComponent(game, game.Content.Load<Texture2D>("Add"),
                new DrawState(game, new Vector4(0.8f, 0.38f, 0.0f, 0.0f), Color.White));
            _musicAdd.AddState(0.5f, new DrawState(game, new Vector4(0.8f, 0.38f, 0.0f, 0.0f), Color.White));
            _musicAdd.AddState(0.5f, new DrawState(game, new Vector4(0.8f, 0.38f, 0.1f, 0.1f), Color.White));
            _musicMinus = new GraphComponent(game, game.Content.Load<Texture2D>("Minus"),
                new DrawState(game, new Vector4(0.1f, 0.38f, 0.0f, 0.0f), Color.White));
            _musicMinus.AddState(0.5f, new DrawState(game, new Vector4(0.1f, 0.38f, 0.0f, 0.0f), Color.White));
            _musicMinus.AddState(0.5f, new DrawState(game, new Vector4(0.1f, 0.38f, 0.1f, 0.1f), Color.White));

            _back = new GraphComponent(game, game.Content.Load<Texture2D>("Previous"),
                new DrawState(game, new Vector4(0.68f, 0.05f, 0.13f, 0.2f), Color.White));

            AddComponent(_settingView);
            AddComponent(_sound);
            AddComponent(_soundAdd);
            AddComponent(_soundMinus);
            AddComponent(_music);
            AddComponent(_musicAdd);
            AddComponent(_musicMinus);
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
            _sound.Content = "音效: " + Setting.SoundVolume.ToString() + "%";
            if (_soundAdd.IsHit() && Setting.SoundVolume < 100)
            {
                _soundAddHover = false;
                _click.Play();
                _soundAdd.ClearAllAndAddState(0.1f,
                    new DrawState(Game, new Vector4(0.8f, 0.28f, 0.1f, 0.1f), Color.Red));
                _soundAdd.AddState(0.1f,
                    new DrawState(Game, new Vector4(0.8f, 0.28f, 0.1f, 0.1f), Color.White));
                Setting.SoundVolume += 10;
            }
            else if (_soundAdd.IsHit())
            {
                _clickError.Play();
            }
            else if (_soundAdd.IsHover() && !_soundAddHover && _soundAdd.IsFinish())
            {
                _soundAddHover = true;
                _soundAdd.ClearAllAndAddState(0.2f,
                    new DrawState(Game, new Vector4(0.8f, 0.28f, 0.1f, 0.1f), Color.PowderBlue));
            }
            else if (!_soundAdd.IsHover() && _soundAddHover && _soundAdd.IsFinish())
            {
                _soundAddHover = false;
                _soundAdd.ClearAllAndAddState(0.2f,
                    new DrawState(Game, new Vector4(0.8f, 0.28f, 0.1f, 0.1f), Color.White));
            }


            if (_soundMinus.IsHit() && Setting.SoundVolume > 0)
            {
                _soundMinusHover = false;
                _click.Play();
                _soundMinus.ClearAllAndAddState(0.1f,
                    new DrawState(Game, new Vector4(0.1f, 0.28f, 0.1f, 0.1f), Color.Red));
                _soundMinus.AddState(0.1f,
                    new DrawState(Game, new Vector4(0.1f, 0.28f, 0.1f, 0.1f), Color.White));
                Setting.SoundVolume -= 10;
            }
            else if (_soundMinus.IsHit())
            {
                _clickError.Play();
            }
            else if (_soundMinus.IsHover() && !_soundMinusHover && _soundMinus.IsFinish())
            {
                _soundMinusHover = true;
                _soundMinus.ClearAllAndAddState(0.2f,
                    new DrawState(Game, new Vector4(0.1f, 0.28f, 0.1f, 0.1f), Color.PowderBlue));
            }
            else if (!_soundMinus.IsHover() && _soundMinusHover && _soundMinus.IsFinish())
            {
                _soundMinusHover = false;
                _soundMinus.ClearAllAndAddState(0.2f,
                    new DrawState(Game, new Vector4(0.1f, 0.28f, 0.1f, 0.1f), Color.White));
            }

            _music.Content = "音樂: " + Setting.MusicVolume.ToString() + "%";
            if (_musicAdd.IsHit() && Setting.MusicVolume < 100)
            {
                _click.Play();
                _musicAdd.ClearAllAndAddState(0.1f,
                    new DrawState(Game, new Vector4(0.8f, 0.38f, 0.1f, 0.1f), Color.Red));
                _musicAdd.AddState(0.1f,
                    new DrawState(Game, new Vector4(0.8f, 0.38f, 0.1f, 0.1f), Color.White));
                Setting.MusicVolume += 10;
            }
            else if (_musicAdd.IsHit())
            {
                _clickError.Play();
            }
            else if (_musicAdd.IsHover() && !_musicAddHover && _musicAdd.IsFinish())
            {
                _musicAddHover = true;
                _musicAdd.ClearAllAndAddState(0.2f,
                    new DrawState(Game, new Vector4(0.8f, 0.38f, 0.1f, 0.1f), Color.PowderBlue));
            }
            else if (!_musicAdd.IsHover() && _musicAddHover && _musicAdd.IsFinish())
            {
                _musicAddHover = false;
                _musicAdd.ClearAllAndAddState(0.2f,
                    new DrawState(Game, new Vector4(0.8f, 0.38f, 0.1f, 0.1f), Color.White));
            }


            if (_musicMinus.IsHit() && Setting.MusicVolume > 0)
            {
                _click.Play();
                _musicMinus.ClearAllAndAddState(0.1f,
                    new DrawState(Game, new Vector4(0.1f, 0.38f, 0.1f, 0.1f), Color.Red));
                _musicMinus.AddState(0.1f,
                    new DrawState(Game, new Vector4(0.1f, 0.38f, 0.1f, 0.1f), Color.White));
                Setting.MusicVolume -= 10;
            }
            else if (_musicMinus.IsHit())
            {
                _clickError.Play();
            }
            else if (_musicMinus.IsHover() && !_musicMinusHover && _musicMinus.IsFinish())
            {
                _musicMinusHover = true;
                _musicMinus.ClearAllAndAddState(0.2f,
                    new DrawState(Game, new Vector4(0.1f, 0.38f, 0.1f, 0.1f), Color.PowderBlue));
            }
            else if (!_musicMinus.IsHover() && _musicMinusHover && _musicMinus.IsFinish())
            {
                _musicMinusHover = false;
                _musicMinus.ClearAllAndAddState(0.2f,
                    new DrawState(Game, new Vector4(0.1f, 0.38f, 0.1f, 0.1f), Color.White));
            }

            if (_back.IsHit())
            {
                _backHover = false;
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

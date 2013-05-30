using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using HareTortoiseGame.State;
using HareTortoiseGame.Component;

namespace HareTortoiseGame.PackageScene
{
    public class GameBoardScene : Scene
    {
        #region Field

        Board _board;
        Container _panel;
        FontComponent _view;
        FontComponent _stateView;
        FontComponent _alertMessage;
        GraphComponent _tortoise;
        GraphComponent _hare;
        GraphComponent _rightArrow;
        GraphComponent _back;

        GraphComponent _hareWin;
        GraphComponent _tortoiseWin;

        Song _backgroundSong;
        Song _victory;

        SoundEffect _start;
        SoundEffect _clickError;

        Board.Turn _turn;
        bool _nomove;
        #endregion
        
        #region Constructor

        public GameBoardScene(Game game)
            : base(game,
                game.Content.Load<Texture2D>("blank"),
                new DrawState(game, new Vector4(2f, 0f, 1f, 1f), Color.Gray))
        {
            _backgroundSong = LoadSong.Load(game,"SunsetParkModern");
            _victory = LoadSong.Load(game,"25");

            _start = game.Content.Load<SoundEffect>("save");
            _clickError = game.Content.Load<SoundEffect>("negative_2");

            _board = new Board(game, game.Content.Load<Texture2D>("blank"),
                new DrawState(game, new Vector4(0.65f, 0.5f, 0f, 0f), Color.Gray));
            _board.AddState(0.5f, new DrawState(game, new Vector4(0.65f, 0.5f, 0f, 0f), Color.Gray));
            _board.AddState(0.5f, new DrawState(game, new Vector4(0.35f, 0.1f, 0.6f, 0.8f), Color.Gray));

            _hareWin = new GraphComponent(game, game.Content.Load<Texture2D>("RabbitWin"),
                new DrawState(game, new Vector4(0.5f, 0.5f, 0.0f, 0.0f), Color.PowderBlue));
            _tortoiseWin = new GraphComponent(game, game.Content.Load<Texture2D>("TurtleWin"),
                new DrawState(game, new Vector4(0.5f, 0.5f, 0.0f, 0.0f), Color.PowderBlue));
            _board.AddComponent(_hareWin);
            _board.AddComponent(_tortoiseWin);

            _panel = new Container(game, game.Content.Load<Texture2D>("blank"),
                new DrawState(game, new Vector4(0f, 0f, 0f, 1.0f), new Color(0.0f, 0.0f, 0.0f, 0.3f)));
            _panel.AddState(0.5f, new DrawState(game, new Vector4(0f, 0f, 0f, 1.0f), new Color(0.0f, 0.0f, 0.0f, 0.3f)));
            _panel.AddState(0.5f, new DrawState(game, new Vector4(0f, 0f, 0.3f, 1.0f), new Color(0.0f, 0.0f, 0.0f, 0.3f)));

            _alertMessage = new FontComponent(game, game.Content.Load<SpriteFont>("Font"),
                new DrawState(game, new Vector4(0.1f, 0.25f, 0.8f, 0f), new Color(0.0f, 0.0f, 0.0f, 0.0f)));
            _alertMessage.Content = "不能移動狀況產生！";
            _view = new FontComponent(game, game.Content.Load<SpriteFont>("Font"), 
                new DrawState( game, new Vector4( 0.2f, 0.05f, 0.0f, 0.0f ), Color.White));
            _view.AddState(0.5f, new DrawState(game, new Vector4(0.2f, 0.05f, 0.0f, 0.0f), Color.White));
            _view.AddState(0.5f, new DrawState(game, new Vector4(0.25f, 0.05f, 0.3f, 0.0f), Color.White));
            _view.Content = "狀態";
            _stateView = new FontComponent(game, game.Content.Load<SpriteFont>("Font"),
                new DrawState(game, new Vector4(0.1f, 0.15f, 0.0f, 0.0f), Color.White));
            _stateView.AddState(0.5f, new DrawState(game, new Vector4(0.1f, 0.15f, 0.0f, 0.0f), Color.White));
            _stateView.AddState(0.5f, new DrawState(game, new Vector4(0.1f, 0.15f, 0.5f, 0.0f), Color.White));
            _stateView.Content = "初始化中...";
            _tortoise = new GraphComponent(game, game.Content.Load<Texture2D>("Turtle"),
                new DrawState(game, new Vector4(0.4f, 0.3f, 0.4f, 0.2f), Color.White));
            _hare = new GraphComponent(game, game.Content.Load<Texture2D>("Rabbit"),
                new DrawState(game, new Vector4(0.4f, 0.5f, 0.4f, 0.2f), Color.White));
            _rightArrow = new GraphComponent(game, game.Content.Load<Texture2D>("rightarrow"),
                new DrawState(game, new Vector4(0.1f, 0.3f, 0.4f, 0.2f), Color.White));
            _turn = Board.Turn.TortoiseTurn;
            _nomove = false;

            _back = new GraphComponent(game, game.Content.Load<Texture2D>("Previous"),
                new DrawState(game, new Vector4(0.3f, 0.75f, 0.4f, 0.2f), Color.White));

            AddComponent(_board);
            _panel.AddComponent(_view);
            _panel.AddComponent(_stateView);
            _panel.AddComponent(_alertMessage);
            _panel.AddComponent(_tortoise);
            _panel.AddComponent(_hare);
            _panel.AddComponent(_rightArrow);
            _panel.AddComponent(_back);
            AddComponent(_panel);
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
            if (_board.IsFinish())
            {
                if (MediaPlayer.State == MediaState.Stopped)
                {
                    MediaPlayer.Play(_backgroundSong);
                }
            }

            if (_back.IsHit())
            {
                _start.Play();
                MediaPlayer.Stop();
                _back.ClearAllAndAddState(0.2f,
                    new DrawState(Game, new Vector4(0.3f, 0.75f, 0.4f, 0.2f), Color.Red));
                NextScene = "Setting";
            }

            if (_board.NowState == Board.BoardState.WaitIO)
            {
                _stateView.Content = "玩家回合中...";
            }
            else if (_board.NowState == Board.BoardState.Animation)
            {
                _stateView.Content = "移動中...";
            }
            else
            {
                _stateView.Content = "電腦思考中...";
            }

            if (_turn != _board.NowTurn)
            {
                _turn = _board.NowTurn;
                if (_turn == Board.Turn.TortoiseTurn)
                {
                    _rightArrow.ClearAllAndAddState(0.2f, new DrawState(Game, new Vector4(0.1f, 0.3f, 0.4f, 0.2f), Color.White));
                }
                else
                {
                    _rightArrow.ClearAllAndAddState(0.2f, new DrawState(Game, new Vector4(0.1f, 0.5f, 0.4f, 0.2f), Color.White));
                }
            }
            if (_board.TortoiseVictory())
            {
                if (MediaPlayer.State == MediaState.Playing && MediaPlayer.Queue.ActiveSong == _backgroundSong)
                {
                    MediaPlayer.Stop();
                    MediaPlayer.Play(_victory);
                }
                _alertMessage.Content = "烏龜獲勝！";
                _alertMessage.ClearAllAndAddState(0.2f, new DrawState(Game, new Vector4(0.1f, 0.25f, 0.8f, 0.0f), Color.Red));
                _tortoiseWin.ClearAllAndAddState(0.2f, new DrawState(Game, new Vector4(0f, 0f, 1f, 1f), Color.PowderBlue));
            }
            else if (_board.HareVictory())
            {
                if (MediaPlayer.State == MediaState.Playing && MediaPlayer.Queue.ActiveSong == _backgroundSong)
                {
                    MediaPlayer.Stop();
                    MediaPlayer.Play(_victory);
                }
                _alertMessage.Content = "兔子獲勝！";
                _alertMessage.ClearAllAndAddState(0.2f, new DrawState(Game, new Vector4(0.1f, 0.25f, 0.8f, 0.0f), Color.Red));
                _hareWin.ClearAllAndAddState(0.2f, new DrawState(Game, new Vector4(0f, 0f, 1f, 1f), Color.PowderBlue));
            }
            else if (_nomove != _board.NoMove)
            {
                _nomove = _board.NoMove;
                if (_nomove)
                {
                    _clickError.Play();
                    _alertMessage.Content = "不能移動狀況產生！";
                    _alertMessage.ClearAllAndAddState(0.2f, new DrawState(Game, new Vector4(0.1f, 0.25f, 0.8f, 0.0f), Color.Red));
                }
                else
                {
                    _alertMessage.Content = "不能移動狀況產生！";
                    _alertMessage.ClearAllAndAddState(0.2f, new DrawState(Game, new Vector4(0.1f, 0.25f, 0.8f, 0.0f), new Color(0.0f, 0.0f, 0.0f, 0.0f)));
                }
            }
            base.Update(gameTime);
        }
        #endregion

    }
}

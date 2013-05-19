using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        Board.Player[] _previousPlayers;
        #endregion

        #region Constructor

        public SettingScene(Game game)
            : base(game,
                game.Content.Load<Texture2D>("blank"),
                new DrawState(game, new Vector4(2f, 0f, 1f, 1f), Color.Gray))
        {
            _settingView = new FontComponent( game, game.Content.Load<SpriteFont>( "font" ),
                new DrawState(game, new Vector4( 0.05f, 0.05f, 0.0f, 0.0f), Color.White));
            _settingView.AddState(0.5f, new DrawState(game, new Vector4(0.05f, 0.025f, 0.0f, 0.0f), Color.White));
            _settingView.AddState(0.5f, new DrawState(game, new Vector4(0.05f, 0.05f, 0.5f, 0.0f), Color.White));
            _settingView.Content = "龜兔賽跑棋：設定";
            _ply = new FontComponent( game, game.Content.Load<SpriteFont>( "font" ),
                new DrawState(game, new Vector4(0.35f, 0.15f, 0.0f, 0.0f), Color.White));
            _ply.AddState(0.5f, new DrawState(game, new Vector4(0.35f, 0.15f, 0.0f, 0.0f), Color.White));
            _ply.AddState(0.5f, new DrawState(game, new Vector4(0.35f, 0.3f, 0.3f, 0.0f), Color.White));
            _ply.Content = "Alpha-Beta最高層數: " + Setting.MaxPly.ToString();
            _plyAdd = new GraphComponent(game, game.Content.Load<Texture2D>("Add"),
                new DrawState(game, new Vector4(0.8f, 0.28f, 0.0f, 0.0f), Color.White));
            _plyAdd.AddState(0.5f, new DrawState(game, new Vector4(0.8f, 0.28f, 0.0f, 0.0f), Color.White));
            _plyAdd.AddState(0.5f, new DrawState(game, new Vector4(0.8f, 0.28f, 0.1f, 0.1f), Color.White));
            _plyMinus = new GraphComponent(game, game.Content.Load<Texture2D>("Minus"),
                new DrawState(game, new Vector4(0.1f, 0.28f, 0.0f, 0.0f), Color.White));
            _plyMinus.AddState(0.5f, new DrawState(game, new Vector4(0.1f, 0.28f, 0.0f, 0.0f), Color.White));
            _plyMinus.AddState(0.5f, new DrawState(game, new Vector4(0.1f, 0.28f, 0.1f, 0.1f), Color.White));
            
            /*_user = new GraphComponent(game, game.Content.Load<Texture2D>("User"),
                new DrawState(game, new Vector4(0.1f, 0.5f, 0.1f, 0.15f), Color.White));
            */
            
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
            if (_plyAdd.IsHit())
            {
                _plyAdd.ClearAllAndAddState(0.05f,
                    new DrawState(Game, new Vector4(0.8f, 0.28f, 0.1f, 0.1f), Color.White));
                _plyAdd.AddState(0.1f,
                    new DrawState(Game, new Vector4(0.8f, 0.28f, 0.1f, 0.1f), Color.Red));
                _plyAdd.AddState(0.1f,
                    new DrawState(Game, new Vector4(0.8f, 0.28f, 0.1f, 0.1f), Color.White));
                ++Setting.MaxPly;
            }
            if (_plyMinus.IsHit() && Setting.MaxPly >= 3)
            {
                _plyMinus.ClearAllAndAddState(0.05f,
                    new DrawState(Game, new Vector4(0.1f, 0.28f, 0.1f, 0.1f), Color.White));
                _plyMinus.AddState(0.1f,
                    new DrawState(Game, new Vector4(0.1f, 0.28f, 0.1f, 0.1f), Color.Red));
                _plyMinus.AddState(0.1f,
                    new DrawState(Game, new Vector4(0.1f, 0.28f, 0.1f, 0.1f), Color.White));
                --Setting.MaxPly;
            }

            if (_tortoiseUser.IsHit()) { Setting.Players[0] = Board.Player.User; }
            if (_tortoiseComputer.IsHit()) { Setting.Players[0] = Board.Player.Computer; }
            if (_hareUser.IsHit()) { Setting.Players[1] = Board.Player.User; }
            if (_hareComputer.IsHit()) { Setting.Players[1] = Board.Player.Computer; }

            if (_next.IsHit())
            {
                _next.AddState( 0.2f,
                    new DrawState(Game, new Vector4(0.83f, 0.05f, 0.13f, 0.2f), Color.Red));
                NextScene = "Board";
            }

            base.Update(gameTime);
        }

        #endregion

    }
}

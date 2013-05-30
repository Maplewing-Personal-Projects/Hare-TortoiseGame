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
using System.Threading.Tasks;
using HareTortoiseGame.State;
using HareTortoiseGame.Component;

namespace HareTortoiseGame.PackageScene
{
    public class OPScene : Scene
    {
        #region Field
        FontComponent _nowLoading;
        Task _loadMusic;
        int _count;
        #endregion

        #region Constructor

        public OPScene(Game game)
            : base(game,
                game.Content.Load<Texture2D>("blank"),
                new DrawState(game, new Vector4(2f, 0f, 1f, 1f), Color.Gray))
        {
            _nowLoading = new FontComponent(game, game.Content.Load<SpriteFont>("Font"),
                new DrawState(game, new Vector4(0.2f, 0.4f, 0.6f, 0f), Color.White));
            _nowLoading.Content = "Now Loading...";
            AddComponent(_nowLoading);
            _count = 200;
        }

        public override void Initialize()
        {
            _loadMusic = new Task(() => Setting.LoadMusic(Game) );
            _loadMusic.Start();
            _state.AddState(0.5f, new DrawState(Game, new Vector4(0, 0, 1, 1), Color.Gray));
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (_nowLoading.IsFinish())
            {
                _nowLoading.AddState(0.5f, new DrawState(Game, new Vector4(0.2f, 0.1f, 0.6f, 0f), Color.White));
                _nowLoading.AddState(0.5f, new DrawState(Game, new Vector4(0.2f, 0.8f, 0.6f, 0f), Color.White));
                _nowLoading.AddState(0.5f, new DrawState(Game, new Vector4(0.2f, 0.4f, 0.6f, 0f), Color.White));
            }

            --_count;
            if (_loadMusic.IsCompleted && _count <= 0 )
            {
                NextScene = "GameStart";
            }
            base.Update(gameTime);
        }

        #endregion

    }
}

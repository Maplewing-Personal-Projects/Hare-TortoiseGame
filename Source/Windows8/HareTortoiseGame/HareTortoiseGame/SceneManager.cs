// 人工智慧作業三 龜兔賽跑程式之研發                
// 資工系103級 499470098 曹又霖
// 使用方法：                                       
// 使用Visual Studio 2012並灌入MonoGame後即可打開並編譯。
// 執行方法：
// 需先灌入.NET Framework 4 和 OpenAL 後，即可打開。
// 信箱：sinmaplewing@gmail.com

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using HareTortoiseGame.Component;
using HareTortoiseGame.PackageScene;
namespace HareTortoiseGame.Manager
{
    public class SceneManager : BasicComponent
    {
        #region Field

        private Scene CurrentScene;
        private Scene _previousScene;

        #endregion

        #region Constructor

        public SceneManager(Game game, string sceneName) : base(game)
        {
            CurrentScene = MakeScene(sceneName);
        }

        #endregion

        #region Method

        private Scene MakeScene(string sceneName)
        {
            Scene scene = null;
            switch (sceneName)
            {
                case "OP":
                    scene = new OPScene(Game);
                    break;
                case "GameStart":
                    SettingParameters.GameWait = true;
                    scene = new GameStartMenu(Game);
                    break;
                case "DisplaySetting":
                    scene = new DisplaySoundSetting(Game);
                    break;
                case "Board":
                    SettingParameters.GameWait = false;
                    scene = new GameBoardScene(Game);
                    break;
                case "Setting":
                    scene = new SettingScene(Game);
                    break;
                default:
                    break;
            }

            return scene;
        }

        public override void Initialize()
        {
            CurrentScene.Start();
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (CurrentScene.NextScene != null)
            {
                _previousScene = CurrentScene;
                _previousScene.ClearAllAndAddState( 0.2f, 
                    new State.DrawState(Game, new Vector4(2f, 0f, 1f, 1f), Color.White));
                Scene scene = MakeScene(CurrentScene.NextScene);
                scene.Start();
                CurrentScene = scene;
            }

            if (_previousScene != null)
            {
                if (_previousScene.IsFinish())
                {
                    _previousScene.End();
                    _previousScene = null;
                }
            }


            CurrentScene.PreviousBounds = PreviousBounds;
            base.Update(gameTime);
        }

        #endregion

    }
}

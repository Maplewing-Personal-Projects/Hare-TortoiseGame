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
                    scene = new GameStartMenu(Game);
                    break;
                case "DisplaySetting":
                    scene = new DisplaySoundSetting(Game);
                    break;
                case "Board":
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

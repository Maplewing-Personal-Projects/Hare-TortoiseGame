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

        #endregion

        #region Constructor

        public SceneManager(Game game, string sceneName) : base(game)
        {
            CurrentScene = MakeScene(sceneName);
        }

        private Scene MakeScene(string sceneName)
        {
            Scene scene = null;
            switch (sceneName)
            {
                case "OP":
                    scene = new OPScene(Game);
                    break;
                case "Board":
                    scene = new GameBoardScene(Game);
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
                Scene scene = MakeScene(CurrentScene.NextScene);
                scene.Start();
                CurrentScene.End();
                CurrentScene = scene;
            }

            CurrentScene.PreviousBounds = PreviousBounds;
            base.Update(gameTime);
        }

        #endregion

    }
}

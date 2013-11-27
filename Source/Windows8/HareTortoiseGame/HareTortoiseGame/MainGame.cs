// 人工智慧作業三 龜兔賽跑程式之研發                
// 資工系103級 499470098 曹又霖
// 使用方法：                                       
// 使用Visual Studio 2012並灌入MonoGame後即可打開並編譯。
// 執行方法：
// 需先灌入.NET Framework 4 和 OpenAL 後，即可打開。
// 信箱：sinmaplewing@gmail.com


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using HareTortoiseGame.Manager;

namespace HareTortoiseGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainGame : Game
    {
        public GraphicsDeviceManager Graphics;
        SpriteBatch _spriteBatch;
        SceneManager _sceneManager;
        Texture2D _logo;

        public MainGame()
        {
            IsMouseVisible = true;
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            MediaPlayer.Volume = ((float)SettingParameters.MusicVolume) / 100f;
            SoundEffect.MasterVolume = ((float)SettingParameters.SoundVolume) / 100f;
            MediaPlayer.IsRepeating = true;
            // TODO: Add your initialization logic here
            _sceneManager = new SceneManager(this, "OP");
            _sceneManager.Start();

            GameState.Initialize();
            base.Initialize();
            _logo = Content.Load<Texture2D>("logo");
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GameState._windowState != WindowState.Snap1Quarter)
            {
                MediaPlayer.Volume = ((float)SettingParameters.MusicVolume) / 100f;
                SoundEffect.MasterVolume = ((float)SettingParameters.SoundVolume) / 100f;
                // TODO: Add your update logic here
                _sceneManager.PreviousBounds = Graphics.GraphicsDevice.Viewport;

                TouchControl.Update(gameTime);
                base.Update(gameTime);
            }
            else
            {
                MediaPlayer.Volume = 0.0f;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            // TODO: Add your drawing code here
            if (GameState._windowState != WindowState.Snap1Quarter)
            {
                base.Draw(gameTime);
            }
            else
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(_logo, new Rectangle((int)(GameState._windowsBounds.Width) / 2, (int)(GameState._windowsBounds.Height) / 2, 200, 100), null, Color.White, 1.0f,
                    new Vector2(_logo.Width / 2, _logo.Height / 2), SpriteEffects.None, 1.0f);
                _spriteBatch.End();
            }
        }
    }
}

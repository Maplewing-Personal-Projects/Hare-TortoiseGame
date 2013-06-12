// 人工智慧作業三 龜兔賽跑程式之研發                
// 資工系103級 499470098 曹又霖
// 使用方法：                                       
// 使用Visual Studio 2012並灌入MonoGame後即可打開並編譯。
// 執行方法：
// 需先灌入.NET Framework 4 和 OpenAL 後，即可打開。
// 信箱：sinmaplewing@gmail.com

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using HareTortoiseGame.Manager;
using HareTortoiseGame.State;

namespace HareTortoiseGame.Component
{
    public class BasicComponent : DrawableGameComponent
    {
        #region Field

        public Viewport PreviousBounds;

        #endregion

        #region Constructor

        public BasicComponent(Game game)
            : base(game)
        {
            PreviousBounds = game.GraphicsDevice.Viewport;
        }

        #endregion

        #region Method
        public virtual void Start()
        {
            Game.Components.Add(this);
        }

        public virtual void End()
        {
            if (Game.Components.Contains(this))
            {
                Game.Components.Remove(this);
            }
        }

        #endregion
    }
}

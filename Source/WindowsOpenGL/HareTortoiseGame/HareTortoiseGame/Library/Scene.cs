// 人工智慧作業三 龜兔賽跑程式之研發                
// 資工系103級 499470098 曹又霖
// 使用方法：                                       
// 使用Visual Studio 2012並灌入MonoGame後即可打開並編譯。
// 執行方法：
// 需先灌入.NET Framework 4 和 OpenAL 後，即可打開。
// 信箱：sinmaplewing@gmail.com

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HareTortoiseGame.State;
using HareTortoiseGame.Component;

namespace HareTortoiseGame
{
    public class Scene : Container
    {
        #region Field

        public string NextScene = null;

        #endregion

        #region Constructor

        public Scene(Game game, Texture2D background, DrawState state)
            : base(game, background, state)
        {
        }

        #endregion

    }
}

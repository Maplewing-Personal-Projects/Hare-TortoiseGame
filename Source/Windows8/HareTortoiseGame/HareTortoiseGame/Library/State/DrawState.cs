using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HareTortoiseGame.State
{
    public class DrawState
    {
        #region Field

        Game _game;
        Vector4 _bounds;

        #endregion

        #region Property

        public Vector4 Bounds
        {
            get { return _bounds; }
            set { _bounds = value; }
        }
        public Vector2 Position
        {
            get
            {
                return new Vector2(_bounds.X, _bounds.Y);
            }
            set { _bounds.X = value.X; _bounds.Y = value.Y; }
        }
        public Vector2 Size
        {
            get
            {
                return new Vector2(_bounds.Z, _bounds.W);
            }

            set { _bounds.Z = value.X; _bounds.W = value.Y; }
        }
        public Rectangle? SourcePosition { get; set; }
        public Color Color { get; set; }
        public Single RotateAngle { get; set; }
        public SpriteEffects SpriteEffects { get; set; }
        public Single Depth { get; set; }

        #endregion

        #region Constructor

        public DrawState(Game game, Vector4 bounds, Color color, Rectangle? sourcePosition = null,
            Single rotateAngle = 0, SpriteEffects spriteEffects = SpriteEffects.None,
            Single depth = 0)
        {
            _game = game;
            _bounds = bounds;
            Color = color;
            SourcePosition = sourcePosition;
            RotateAngle = rotateAngle;
            SpriteEffects = spriteEffects;
            Depth = depth;
        }

        #endregion

    }
}

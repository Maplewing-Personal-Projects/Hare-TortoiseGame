using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace HareTortoiseGame
{
    public static class TouchControl
    {
        #region Field

        static MouseState _previousMouseState;
        static MouseState _currentMouseState;

        static GestureSample? _currentGestureSample;

        #endregion

        #region Property
        #endregion

        #region Method

        public static void Update(GameTime gameTime)
        {
            TouchPanel.EnabledGestures = GestureType.Tap | GestureType.Pinch | GestureType.HorizontalDrag | GestureType.VerticalDrag;
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
            if (TouchPanel.IsGestureAvailable) _currentGestureSample = TouchPanel.ReadGesture();
            else _currentGestureSample = null;
        }

        public static bool IsMouseClick()
        {
            return (_currentMouseState.LeftButton == ButtonState.Released &&
                _previousMouseState.LeftButton == ButtonState.Pressed);
        }

        public static bool IsTouchClick()
        {
            return (_currentGestureSample.HasValue &&
                _currentGestureSample.Value.GestureType == GestureType.Tap);
        }

        public static Rectangle MousePosition()
        {
            return new Rectangle(_previousMouseState.X, _previousMouseState.Y, 1, 1);
        }

        public static Rectangle TouchPosition()
        {
            if (_currentGestureSample.HasValue)
            {
                return new Rectangle((int)_currentGestureSample.Value.Position.X,
                    (int)_currentGestureSample.Value.Position.Y,
                    1, 1);
            }
            else return Rectangle.Empty;
        }

        #endregion
    }
}

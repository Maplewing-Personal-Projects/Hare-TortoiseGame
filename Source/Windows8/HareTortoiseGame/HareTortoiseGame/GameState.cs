using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;

namespace HareTortoiseGame
{
    public enum WindowState { Full = 0, Snap1Quarter = 1, Snap3Quarter = 2 };
    public static class GameState
    {
        public static WindowState _windowState;
        public static CoreWindow _window;
        public static Rect _windowsBounds;

        public static void Initialize()
        {
            _window = CoreWindow.GetForCurrentThread();
            _windowsBounds = _window.Bounds;
            _windowState = WindowState.Full;
            _window.SizeChanged += _window_SizeChanged;
        }

        static void _window_SizeChanged(CoreWindow sender, WindowSizeChangedEventArgs args)
        {
            if (args.Size.Width == _windowsBounds.Width)
            {
                _windowState = WindowState.Full;
            }
            else if (args.Size.Width <= 320.00)
            {
                _windowState = WindowState.Snap1Quarter;
            }
            else
            {
                _windowState = WindowState.Snap3Quarter;
            }

            _windowsBounds.Height = args.Size.Height;
            _windowsBounds.Width = args.Size.Width;
        }
    }
}

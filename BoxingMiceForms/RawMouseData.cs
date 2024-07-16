using Linearstar.Windows.RawInput.Native;
using System;
using Color = Microsoft.Xna.Framework.Color;

namespace Editor {
    public static class RawMouseData {

        public enum MouseEvents {
            WM_INPUT = 0x00FF, // Mouse move
            WM_LMB_DOWN = 0x201, // Mouse click
            WM_LMB_UP = 0x202,
            WM_RMB_DOWN = 0x204,
            WM_RMB_UP = 0x205,
            WM_MMB_DOWN = 0x207,
            WM_MMB_UP = 0x208,
            WM_SETCURSOR = 0x20,
            WM_MOUSELEAVE = 0x2a3,
            WM_MOUSEACTIVATE = 0x0021,
            WM_MOUSEWHEEL = 0x020A
        }

        static Random rnd = new Random();

        public class MouseData {
            public RawMouseButtonFlags mouseButtonFlags => _mouseButtonFlags;
            private RawMouseButtonFlags _mouseButtonFlags;

            public int playerId => _playerId;
            private int _playerId;

            public float X;
            public float Y;

            public float deltaX = 0;
            public float deltaY = 0;

            public float lastDeltaX = 0;
            public float lastDeltaY = 0;

            public Color color;

            public bool leftButton => _leftButton;
            private bool _leftButton;
            public bool rightButton => _rightButton;
            private bool _rightButton;
            public bool middleButton => _middleButton;
            private bool _middleButton;

            public MouseData(int playerId, float x = 0, float y = 0) {
                this._playerId = playerId;
                this.X = x;
                this.Y = y;
                color = new Color(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));

            }
            public void UpdateKeys(RawMouseButtonFlags mouseButtons) {
                this._mouseButtonFlags = mouseButtons;

                switch (mouseButtons) {
                    case RawMouseButtonFlags.LeftButtonDown:
                        _leftButton = true;
                        break;
                    case RawMouseButtonFlags.LeftButtonUp:
                        _leftButton = false;
                        break;
                    case RawMouseButtonFlags.RightButtonDown:
                        _rightButton = true;
                        break;
                    case RawMouseButtonFlags.RightButtonUp:
                        _rightButton = false;
                        break;
                    case RawMouseButtonFlags.MiddleButtonDown:
                        _middleButton = true;
                        break;
                    case RawMouseButtonFlags.MiddleButtonUp:
                        _middleButton = false;
                        break;
                }
            }
        }
    }
}

using Linearstar.Windows.RawInput;
using Linearstar.Windows.RawInput.Native;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

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

        public class MouseData {
            public RawMouseButtonFlags mouseButtonFlags => _mouseButtonFlags;
            private RawMouseButtonFlags _mouseButtonFlags;

            public float deltaX = 0;
            public float deltaY = 0;

            public float nDX = 0;
            public float nDY = 0;

            private float x = 0, y = 0;
            public float nX = 0;
            public float nY = 0;

            public float lastDeltaX = 0;
            public float lastDeltaY = 0;

            public float nLDX = 0;
            public float nLDY = 0;

            public int scroll = 0;

            public bool leftButton => _leftButton;
            private bool _leftButton;
            public bool rightButton => _rightButton;
            private bool _rightButton;
            public bool middleButton => _middleButton;
            private bool _middleButton;

            public readonly int playerId;

            public MouseData(int playerId) {
                this.playerId = playerId;
            }

            public void Update(GameTime gameTime) {
                this.deltaX = 0;
                this.deltaY = 0;
                this.nDX = 0;
                this.nDY = 0;
                this.x = 0;
                this.y = 0;
                this.nLDX = 0;
                this.nLDY = 0;
            }

            public void UpdateKeys(RawMouse mouse) {
                this._mouseButtonFlags = mouse.Buttons;

                this.deltaX = mouse.LastX;
                this.deltaY = mouse.LastY;

                x += deltaX;
                y += deltaY;

                (nX, nY) = Normalize(x, y);
                (nDX, nDY) = Normalize(deltaX, deltaY);
                (nLDX, nLDY) = Normalize(lastDeltaX, lastDeltaY);

                this.scroll = mouse.ButtonData;

                if (mouse.LastX != 0 && mouse.LastY != 0) {
                    this.lastDeltaX = mouse.LastX;
                    this.lastDeltaY = mouse.LastY;
                }

                switch (this._mouseButtonFlags) {
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

            (float, float) Normalize(float x, float y) {
                if (x == 0 && y == 0) return (0f, 0f);

                float len = (float)Math.Sqrt(x * x + y * y);
                return (x / len, y / len);
            }
        }
    }
}

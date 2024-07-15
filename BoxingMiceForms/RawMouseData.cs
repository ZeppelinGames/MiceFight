using System;
using Color = Microsoft.Xna.Framework.Color;

namespace Editor {
    public static class RawMouseData {

        public enum MouseKeys {
            CONTROL = 0x0008,
            LBUTTON = 0x0001,
            MBUTTON = 0x0010,
            RBUTTON = 0x0002,
            SHIFT = 0x0004,
            XBTN1 = 0x0020,
            XBTN2 = 0x0040
        }

        public enum MouseEvents {
            WM_MOUSEMOVE = 0x200,
            WM_SETCURSOR = 0x20,
            WM_MOUSELEAVE = 0x2a3,
            WM_NCMOUSEMOVE = 0xa0,
            WM_MOUSEACTIVATE = 0x0021,
            WM_MOUSEWHEEL = 0x020A,
            TEST = 0x00FF
        }

        static Random rnd = new Random();

        public class MouseData {
            public MouseKeys Keys;

            public float X;
            public float Y;

            public float deltaX;
            public float deltaY;

            public float lastDeltaX;
            public float lastDeltaY;

            public Color color;

            public MouseData(float x = 0, float y = 0) {
                this.X = x;
                this.Y = y;
                color = new Color(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
            }
        }
    }
}

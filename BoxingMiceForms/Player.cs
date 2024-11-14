using System;
using System.Collections.Generic;
using Editor.Sprites;
using Microsoft.Xna.Framework;

namespace Editor {
    public class Player : Actor {
        static Random rnd = new Random();

        public RawMouseData.MouseData mouseData => _mouseData;
        private RawMouseData.MouseData _mouseData;

        public bool isAlive => _isAlive;
        private bool _isAlive = true;

        public bool isReady;

        public readonly int id;

        public float x, y;
        public int X => (int)x;
        public int Y => (int)y;

        private Sprite _sprite;

        public Color color {
            get => _color;
            set {
                if (value != _color) {
                    _color = value;
                    shieldSprite.SetMainColour(value);
                    _sprite = new CursorSprite(_color);
                }
            }
        }

        private Color _color;
        public int playerColorIndex = -1;

        public RectSprite shieldSprite = new RectSprite(16);
        public float shieldRotation = 0f;

        public static int size = 4;

        public Player(int id, int x = 0, int y = 0) {
            this.id = id;
            this.x = x;
            this.y = y;
            _mouseData = new RawMouseData.MouseData(id);
        }

        public override void Update(GameTime gameTime) {
            this.mouseData.deltaX = 0;
            this.mouseData.deltaY = 0;
            this.mouseData.nDX = 0;
            this.mouseData.nDY = 0;
            this.mouseData.x = 0;
            this.mouseData.y = 0;

            shieldRotation += (float)gameTime.ElapsedGameTime.TotalSeconds * 3f;
        }

        public override void Draw(RenderCanvas canvas) {
            canvas.DrawSprite(_sprite, X, Y);
        }

        public void Kill() {
            _isAlive = false;
        }
    }
}

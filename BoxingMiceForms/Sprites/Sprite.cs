using System;
using System.Collections.Generic;
using Editor.Sprites;
using Microsoft.Xna.Framework;

namespace Editor {
    public class Sprite {
        public enum SPRITE_ID {
            TICK
        }

        static Dictionary<SPRITE_ID, Sprite> _sprites = new Dictionary<SPRITE_ID, Sprite>()
        {
            { SPRITE_ID.TICK, new TickSprite()}
        };

        private Color[] _palette;

        public int[][] sprite => _sprite;
        private int[][] _sprite;

        public int spriteWidth => _spriteWidth;
        public int spriteHeight => _spriteHeight;

        private int _spriteWidth = 0;
        private int _spriteHeight = 0;

        public Sprite(int[][] sprite, Color[] palette) {
            UpdateSprite(sprite, palette);
        }
        protected Sprite() { }

        protected void UpdateSprite(int[][] sprite, Color[] palette) {
            this._sprite = sprite;
            this._palette = palette;

            this._spriteHeight = sprite.Length;
            this._spriteWidth = 0;
            for (int i = 0; i < sprite.Length; i++) {
                if (sprite[i].Length > this._spriteWidth) {
                    this._spriteWidth = sprite[i].Length;
                }
            }
        }

        public Color this[int x, int y] {
            get {
                if (_sprite == null ||
                    x < 0 ||
                    x >= _sprite.Length ||
                    y < 0 ||
                    y >= _sprite[x].Length ||
                    _sprite[x][y] >= _palette.Length) return Color.Pink;

                if (_sprite[x][y] < 0) return Color.Transparent;

                return _palette[_sprite[x][y]];
            }
        }

        public static bool GetSprite(SPRITE_ID spriteId, out Sprite sprite) {
            sprite = null;
            if (_sprites.ContainsKey(spriteId)) {
                sprite = _sprites[spriteId];
                return true;
            }
            return false;
        }
    }
}

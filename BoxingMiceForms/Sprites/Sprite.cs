using System;
using System.Collections.Generic;
using Editor.Sprites;
using Microsoft.Xna.Framework;

namespace Editor
{
    public class Sprite
    {
        public enum SPRITE_ID
        {
            TICK
        }

        static Dictionary<SPRITE_ID, Sprite> _sprites = new Dictionary<SPRITE_ID, Sprite>()
        {
            { SPRITE_ID.TICK, new TickSprite()}
        };

        protected Color[] _palette;

        public int[][] sprite => _sprite;
        protected int[][] _sprite;

        public Color this[int x, int y]
        {
            get
            {
                return _palette[_sprite[x][y]];
            }
        }

        public static bool GetSprite(SPRITE_ID spriteId, out Sprite sprite)
        {
            sprite = null;
            if(_sprites.ContainsKey(spriteId))
            {
                sprite = _sprites[spriteId];
                return true;
            }
            return false;
        }
    }
}

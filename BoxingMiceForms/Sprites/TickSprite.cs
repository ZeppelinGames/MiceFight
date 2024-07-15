using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Sprites
{
    public class TickSprite : Sprite
    {
        public TickSprite()
        {
            this._palette = new Color[]
            {
                Color.Green,
            };
            this._sprite = new int[][] {
                new int[] { -1,-1,-1,-1,-1 },
                new int[] { -1,-1,-1,-1,0 },
                new int[] { -1,-1,-1,0,-1 },
                new int[] { 0,-1,0,-1,-1 },
                new int[] { -1,0,-1,-1,-1 },
            };
        }
    }
}

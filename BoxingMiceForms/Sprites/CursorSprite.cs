using System;
using Microsoft.Xna.Framework;

namespace Editor.Sprites {
    public class CursorSprite : Sprite {
        public CursorSprite(Color c) {
            Color dark = new Color(c.R - 10, c.G - 10, c.B - 10);
            UpdateSprite(
                new int[][] {
                    new int[] { 1,1,0,0,0,0,0 },
                    new int[] { 1,2,1,0,0,0,0 },
                    new int[] { 1,2,2,1,0,0,0 },
                    new int[] { 1,2,2,2,1,0,0 },
                    new int[] { 1,2,2,2,2,1,0 },
                    new int[] { 1,2,2,2,2,3,1 },
                    new int[] { 1,2,2,2,1,1,1 },
                    new int[] { 1,3,1,2,2,1,0 },
                    new int[] { 1,1,1,3,2,1,0 },
                    new int[] { 0,0,0,1,1,0,0 },
                },
                new Color[]
                {
                    Color.Transparent,
                    Color.Black,
                    c,
                    dark
                }
            );
        }
    }
}

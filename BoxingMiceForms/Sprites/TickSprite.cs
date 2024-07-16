using System;
using Microsoft.Xna.Framework;

namespace Editor.Sprites {
    public class TickSprite : Sprite {
        public TickSprite() {
            UpdateSprite(
                new int[][] {
                    new int[] { -1, -1, -1, -1, -1 },
                    new int[] { -1, -1, -1, -1, 0 },
                    new int[] { -1, -1, -1, 0, -1 },
                    new int[] { 0, -1, 0, -1, -1 },
                    new int[] { -1, 0, -1, -1, -1 },
                },
                new Color[]
                {
                    Color.Green,
                }
            );
        }
    }
}

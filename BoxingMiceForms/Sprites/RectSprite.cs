using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Sprites {
    public class RectSprite : Sprite {
        public RectSprite(int size) : this(size, Color.Black) { }

        public RectSprite(int size, Color c) {
            int[][] rect = new int[size][];
            for (int i = 0; i < rect.Length; i++) {
                rect[i] = new int[size];
                for (int j = 0; j < rect[i].Length; j++) {
                    rect[i][j] = (i == 0 || j == 0 || i == size - 1 || j == size - 1) ? 0 : -1;
                }
            }

            UpdateSprite(
               rect,
               new Color[]
               {
                   c
               }
           );
        }

        public void SetMainColour(Color c) {
            Color[] palette = this.palette;
            palette[0] = c;
            UpdatePalette(palette);
        }
    }
}

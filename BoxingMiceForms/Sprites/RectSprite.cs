using Microsoft.Xna.Framework;

namespace Editor.Sprites {
    public class RectSprite : Sprite {
        public RectSprite(int size) : this(size, Color.Black) { }
        public RectSprite(int size, Color c) : this(size, size, c) { }
        public RectSprite(int width, int height, Color c)
        {
            int[][] rect = new int[width][];
            for (int i = 0; i < rect.Length; i++)
            {
                rect[i] = new int[height];
                for (int j = 0; j < rect[i].Length; j++)
                {
                    rect[i][j] = (i == 0 || j == 0 || i == width - 1 || j == height - 1) ? 0 : -1;
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

        public override void SetMainColour(Color c) {
            Color[] palette = this.palette;
            palette[0] = c;
            UpdatePalette(palette);
        }
    }
}

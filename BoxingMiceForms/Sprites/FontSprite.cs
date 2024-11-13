using System;
using Microsoft.Xna.Framework;

namespace Editor.Sprites {
    public static class FontSprite {

        public static Sprite GetText(string text, Color color, BitFont font, int letterSpacing = 2) {
            int fontWidth = font.GetCharWidth();
            int fontHeight = font.GetCharHeight();
            int textWidth = text.Length * (fontWidth + letterSpacing);

            int[][] sprite = new int[fontHeight][];
            for (int i = 0; i < sprite.Length; i++) {
                sprite[i] = new int[textWidth];
            }
            Color[] palette = { Color.Transparent, color };

            int cOffset = 0;
            for (int i = 0; i < text.Length; i++) {
                if (font.GetCharData(text[i], out int charData)) {
                    int y = fontHeight - 1;
                    while (charData > 0) {
                        int rowNum = charData % 10;
                        charData /= 10;
                        for (int j = 0; j < fontWidth; j++) {
                            int b = rowNum & 1;
                            sprite[y][(fontWidth - 1 - j) + cOffset] = b;
                            rowNum >>= 1;
                        }
                        y--;
                    }

                    cOffset += fontWidth + letterSpacing;
                }
            }

            return new Sprite(sprite, palette);
        }
    }
}

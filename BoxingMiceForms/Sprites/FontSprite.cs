using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Editor.Sprites {
    public class FontSprite {
        protected Dictionary<char, int[]> _charactersBit = new Dictionary<char, int[]>();
        public readonly int WIDTH;
        public readonly int HEIGHT;

        public FontSprite(Dictionary<char, int[]> chars, int width, int height) {
            this._charactersBit = chars;
            this.WIDTH = width;
            this.HEIGHT = height;
        }
        public bool GetCharData(char c, out int[] d) {
            bool hasChar = _charactersBit.ContainsKey(c);
            d = hasChar ? _charactersBit[c] : new int[0];
            return hasChar;
        }

        public static Sprite GetText(string text, Color color, FontSprite font, int letterSpacing = 1) {
            int textWidth = text.Length * (font.WIDTH + letterSpacing);

            int[][] sprite = new int[font.HEIGHT][];
            for (int i = 0; i < sprite.Length; i++) {
                sprite[i] = new int[textWidth];
            }
            Color[] palette = { Color.Transparent, color };

            int cOffset = 0;
            for (int i = 0; i < text.Length; i++) {
                if (font.GetCharData(text[i], out int[] charData)) {
                    for(int j = 0; j < charData.Length; j++) {
                        int rowNum = charData[j];
                        for (int k = 0; k < font.WIDTH; k++) {
                            int b = rowNum & 1;
                            sprite[j][(font.WIDTH - 1 - k) + cOffset] = b;
                            rowNum >>= 1;
                        }
                    }

                    cOffset += font.WIDTH + letterSpacing;
                }
            }

            return new Sprite(sprite, palette);
        }
    }
}

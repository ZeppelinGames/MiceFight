using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Sprites {
    public static class FontSprite {

        static int _characterWidth = 3;
        static int _characterHeight = 4;

        readonly static Dictionary<char, int[][]> _characters = new Dictionary<char, int[][]> {
            { 'A', new int[][]{
                new int []{ 1,1,1 },
                new int []{ 1,0,1 },
                new int []{ 1,1,1 },
                new int []{ 1,0,1 },
            }},
            { 'B', new int[][]{
                new int []{ 1,1,0 },
                new int []{ 1,1,1 },
                new int []{ 1,0,1 },
                new int []{ 1,1,1 },
            }},
            { 'C', new int[][]{
                new int []{ 1,1,1 },
                new int []{ 1,0,0 },
                new int []{ 1,0,0 },
                new int []{ 1,1,1 },
            }},
            { 'D', new int[][]{
                new int []{ 1,1,0 },
                new int []{ 1,0,1 },
                new int []{ 1,0,1 },
                new int []{ 1,1,0 },
            }},
            { 'E', new int[][]{
                new int []{ 1,1,1 },
                new int []{ 1,1,0 },
                new int []{ 1,0,0 },
                new int []{ 1,1,1 },
            }},
            { 'F', new int[][]{
                new int []{ 1,1,1 },
                new int []{ 1,0,0 },
                new int []{ 1,1,0 },
                new int []{ 1,0,0 },
            }},
            { 'G', new int[][]{
                new int []{ 1,1,1 },
                new int []{ 1,0,0 },
                new int []{ 1,0,1 },
                new int []{ 1,1,1 },
            }},
            { 'H', new int[][]{
                new int []{ 1,0,1 },
                new int []{ 1,1,1 },
                new int []{ 1,0,1 },
                new int []{ 1,0,1 },
            }},
            { 'I', new int[][]{
                new int []{ 1,1,1 },
                new int []{ 0,1,0 },
                new int []{ 0,1,0 },
                new int []{ 1,1,1 },
            }},
            { 'J', new int[][]{
                new int []{ 1,1,1 },
                new int []{ 0,1,0 },
                new int []{ 0,1,0 },
                new int []{ 1,1,0 },
            }},
            { 'K', new int[][]{
                new int []{ 1,0,1 },
                new int []{ 1,1,0 },
                new int []{ 1,1,0 },
                new int []{ 1,0,1 },
            }},
            { 'L', new int[][]{
                new int []{ 1,0,0 },
                new int []{ 1,0,0 },
                new int []{ 1,0,0 },
                new int []{ 1,1,1 },
            }},
            { 'M', new int[][]{
                new int []{ 1,1,1 },
                new int []{ 1,1,1 },
                new int []{ 1,0,1 },
                new int []{ 1,0,1 },
            }},
            { 'N', new int[][]{
                new int []{ 1,1,1 },
                new int []{ 1,0,1 },
                new int []{ 1,0,1 },
                new int []{ 1,0,1 },
            }},
            { 'O', new int[][]{
                new int []{ 1,1,1 },
                new int []{ 1,0,1 },
                new int []{ 1,0,1 },
                new int []{ 1,1,1 },
            }},
            { 'P', new int[][]{
                new int []{ 1,1,1 },
                new int []{ 1,0,1 },
                new int []{ 1,1,1 },
                new int []{ 1,0,0 },
            }},
            { 'Q', new int[][]{
                new int []{ 1,1,1 },
                new int []{ 1,0,1 },
                new int []{ 1,1,1 },
                new int []{ 0,0,1 },
            }},
            { 'R', new int[][]{
                new int []{ 1,1,1 },
                new int []{ 1,1,1 },
                new int []{ 1,1,0 },
                new int []{ 1,0,1 },
            }},
            { 'S', new int[][]{
                new int []{ 1,1,1 },
                new int []{ 1,0,0 },
                new int []{ 0,1,1 },
                new int []{ 1,1,1 },
            }},
            { 'T', new int[][]{
                new int []{ 1,1,1 },
                new int []{ 0,1,0 },
                new int []{ 0,1,0 },
                new int []{ 0,1,0 },
            }},
            { 'U', new int[][]{
                new int []{ 1,0,1 },
                new int []{ 1,0,1 },
                new int []{ 1,0,1 },
                new int []{ 1,1,1 },
            }},
            { 'V', new int[][]{
                new int []{ 1,0,1 },
                new int []{ 1,0,1 },
                new int []{ 1,0,1 },
                new int []{ 0,1,0 },
            }},
            { 'W', new int[][]{
                new int []{ 1,0,1 },
                new int []{ 1,0,1 },
                new int []{ 1,1,1 },
                new int []{ 1,1,1 },
            }},
            { 'X', new int[][]{
                new int []{ 1,0,1 },
                new int []{ 1,1,0 },
                new int []{ 0,1,1 },
                new int []{ 1,0,1 },
            }},
            { 'Y', new int[][]{
                new int []{ 1,0,1 },
                new int []{ 1,1,1 },
                new int []{ 0,1,0 },
                new int []{ 0,1,0 },
            }},
            { 'Z', new int[][]{
                new int []{ 1,1,1 },
                new int []{ 0,1,1 },
                new int []{ 1,0,0 },
                new int []{ 1,1,1 },
            }},
            { '0', new int[][]{
                new int []{ 1,1,1 },
                new int []{ 1,0,1 },
                new int []{ 1,0,1 },
                new int []{ 1,1,1 },
            }},
            { '1', new int[][]{
                new int []{ 1,1,0 },
                new int []{ 0,1,0 },
                new int []{ 0,1,0 },
                new int []{ 0,1,0 },
            }},
            { '2', new int[][]{
                new int []{ 1,1,1 },
                new int []{ 0,0,1 },
                new int []{ 1,1,0 },
                new int []{ 1,1,1 },
            }},
            { '3', new int[][]{
                new int []{ 1,1,1 },
                new int []{ 0,1,1 },
                new int []{ 0,0,1 },
                new int []{ 1,1,1 },
            }},
            { '4', new int[][]{
                new int []{ 1,0,1 },
                new int []{ 1,0,1 },
                new int []{ 1,1,1 },
                new int []{ 0,0,1 },
            }},
            { '5', new int[][]{
                new int []{ 1,1,1 },
                new int []{ 1,1,0 },
                new int []{ 0,0,1 },
                new int []{ 1,1,1 },
            }},
            { '6', new int[][]{
                new int []{ 1,1,1 },
                new int []{ 1,0,0 },
                new int []{ 1,1,1 },
                new int []{ 1,1,1 },
            }},
            { '7', new int[][]{
                new int []{ 1,1,1 },
                new int []{ 0,0,1 },
                new int []{ 0,1,1 },
                new int []{ 0,0,1 },
            }},
            { '8', new int[][]{
                new int []{ 1,1,1 },
                new int []{ 1,1,1 },
                new int []{ 1,0,1 },
                new int []{ 1,1,1 },
            }},
            { '9', new int[][]{
                new int []{ 1,1,1 },
                new int []{ 1,1,1 },
                new int []{ 0,0,1 },
                new int []{ 1,1,1 },
            }},
            { '!', new int[][]{
                new int []{ 0,1,0 },
                new int []{ 0,1,0 },
                new int []{ 0,0,0 },
                new int []{ 0,1,0 },
            }},
            { ' ', new int[][]{
                new int []{ 0,0,0 },
                new int []{ 0,0,0 },
                new int []{ 0,0,0 },
                new int []{ 0,0,0 },
            }},
             { '+', new int[][]{
                new int []{ 0,1,0 },
                new int []{ 1,1,1 },
                new int []{ 0,1,0 },
                new int []{ 0,0,0 },
            }},
        };

        public static Sprite GetText(string text, Color color, int letterSpacing = 2) {
            int textWidth = text.Length * (_characterWidth + letterSpacing);

            int[][] sprite = new int[_characterHeight][];
            for (int i = 0; i < sprite.Length; i++) {
                sprite[i] = new int[textWidth];
            }
            Color[] palette = { Color.Transparent, color };

            int cOffset = 0;
            for (int i = 0; i < text.Length; i++) {
                if (_characters.ContainsKey(text[i])) {
                    int[][] cMap = _characters[text[i]];
                    for (int y = 0; y < cMap.Length; y++) {
                        for (int x = 0; x < cMap[y].Length; x++) {
                            sprite[y][x + cOffset] = cMap[y][x];
                        }
                    }
                    cOffset += _characterWidth + letterSpacing;
                }
            }

            return new Sprite(sprite, palette);
        }
    }
}

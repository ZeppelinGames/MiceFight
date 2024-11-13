using System;
using System.Collections.Generic;

namespace Editor.Sprites.Fonts {
    public class FontSmall : BitFont {
        public static FontSmall Font = new FontSmall();

        const int FONT_WIDTH = 3;
        const int FONT_HEIGHT = 4;

        // 000 - 0
        // 001 - 1
        // 010 - 2
        // 011 - 3
        // 100 - 4
        // 101 - 5
        // 110 - 6
        // 111 - 7
        readonly static Dictionary<char, int> _charactersBit = new Dictionary<char, int> {
            {'A', 7575},
            {'B', 6757},
            {'C', 7447},
            {'D', 6556},
            {'E', 7647},
            {'F', 7464},
            {'G', 7457},
            {'H', 5755},
            {'I', 7227},
            {'J', 7226},
            {'K', 5665},
            {'L', 4447},
            {'M', 7755},
            {'N', 7555},
            {'O', 7557},
            {'P', 7574},
            {'Q', 7571},
            {'R', 7765},
            {'S', 7437},
            {'T', 7222},
            {'U', 5557},
            {'V', 5552},
            {'W', 5577},
            {'X', 5635},
            {'Y', 5722},
            {'Z', 7347},
            { '0', 7557},
            { '1', 6222},
            { '2', 7167},
            { '3', 7317},
            { '4', 5571},
            { '5', 7617},
            { '6', 7477},
            { '7', 7131},
            { '8', 7757},
            { '9', 7717},
            { '!', 2202},
            { ' ', 0},
            { '+', 2720},
        };

        public override bool GetCharData(char c, out int d) {
            bool hasChar = _charactersBit.ContainsKey(c);
            d = hasChar ? _charactersBit[c] : 0;
            return hasChar;
        }

        public override int GetCharWidth() {
            return FONT_WIDTH;
        }
        public override int GetCharHeight() {
            return FONT_HEIGHT;
        }
    }
}
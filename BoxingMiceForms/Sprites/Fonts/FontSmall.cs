using System;
using System.Collections.Generic;

namespace Editor.Sprites.Fonts {
    public static class FontSmall {
        const int WIDTH = 3;
        const int HEIGHT = 4;

        public static FontSprite Font = new FontSprite(
            new Dictionary<char, int[]> {
            {'A', new int[]{7,5,7,5}},
            {'B', new int[]{6,7,5,7}},
            {'C', new int[]{7,4,4,7}},
            {'D', new int[]{6,5,5,6}},
            {'E', new int[]{7,6,4,7}},
            {'F', new int[]{7,4,6,4}},
            {'G', new int[]{7,4,5,7}},
            {'H', new int[]{5,7,5,5}},
            {'I', new int[]{7,2,2,7}},
            {'J', new int[]{7,2,2,6}},
            {'K', new int[]{5,6,6,5}},
            {'L', new int[]{4,4,4,7}},
            {'M', new int[]{7,7,5,5}},
            {'N', new int[]{7,5,5,5}},
            {'O', new int[]{7,5,5,7}},
            {'P', new int[]{7,5,7,4}},
            {'Q', new int[]{7,5,7,1}},
            {'R', new int[]{7,7,6,5}},
            {'S', new int[]{7,4,3,7}},
            {'T', new int[]{7,2,2,2}},
            {'U', new int[]{5,5,5,7}},
            {'V', new int[]{5,5,5,2}},
            {'W', new int[]{5,5,7,7}},
            {'X', new int[]{5,6,3,5}},
            {'Y', new int[]{5,7,2,2}},
            {'Z', new int[]{7,3,4,7}},
            {'0', new int[]{7,5,5,7}},
            {'1', new int[]{6,2,2,2}},
            {'2', new int[]{7,1,6,7}},
            {'3', new int[]{7,3,1,7}},
            {'4', new int[]{5,5,7,1}},
            {'5', new int[]{7,6,1,7}},
            {'6', new int[]{7,4,7,7}},
            {'7', new int[]{7,1,3,1}},
            {'8', new int[]{7,7,5,7}},
            {'9', new int[]{7,7,1,7}},
            {'!', new int[]{2,2,0,2}},
            {'+', new int[]{2,7,2,0}},
            {' ', new int[]{0}},
        },
        WIDTH,
        HEIGHT);
    }
}
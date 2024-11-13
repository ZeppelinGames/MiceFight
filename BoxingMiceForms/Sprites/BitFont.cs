using System;
namespace Editor.Sprites {
    public abstract class BitFont {

        public abstract int GetCharWidth();
        public abstract int GetCharHeight();

        public abstract bool GetCharData(char c, out int d);
    }
}

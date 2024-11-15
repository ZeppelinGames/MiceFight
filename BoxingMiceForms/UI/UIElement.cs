using Editor.Controls;
using System;

namespace Editor
{
    public abstract class UIElement
    {
        protected int x, y;
        protected int width, height;
        protected MainControl _main;

        public UIElement(MainControl mc)
        {
            _main = mc;
        }

        protected virtual void OnClick(Player p, int x, int y)
        {

        }
        public abstract void Draw(RenderCanvas canvas);
    }
}
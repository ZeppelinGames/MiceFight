using System;

namespace Editor
{
    public abstract class UIElement
    {
        protected float x, y;
        protected float width, height;

        protected virtual void OnClick(Player p, int x, int y)
        {

        }
        public abstract void Draw(RenderCanvas canvas);
    }
}
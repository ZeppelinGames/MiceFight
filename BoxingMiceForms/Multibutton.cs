using Editor.Sprites;
using Editor.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Editor
{
    public class Multibutton : UIElement
    {
        Dictionary<int, bool> _playersClicked = new Dictionary<int, bool>();
        Sprite s;

        public Multibutton()
        {
            UIManager.OnClick += OnClick;
            s = new RectSprite(50, 20, Color.White);
        }

        protected override void OnClick(Player p, int x, int y)
        {
            if (x > this.x && x < this.x + this.width)
            {
                if (y > this.y && y < this.y + this.height)
                {
                    // clicked
                    if (_playersClicked.ContainsKey(p.id))
                    {
                        _playersClicked[p.id] = !_playersClicked[p.id];
                    }
                    else
                    {
                        _playersClicked.Add(p.id, true);
                    }
                }
            }
        }

        public override void Draw(RenderCanvas canvas)
        {
            canvas.DrawSprite(s, (int)this.x, (int)this.y);
        }
    }
}

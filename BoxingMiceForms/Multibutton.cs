using Editor.Controls;
using Editor.Sprites;
using Editor.Sprites.Fonts;
using Editor.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Editor
{
    public class Multibutton : UIElement
    {
        public Action OnButtonClick;
        public readonly List<int> playersClicked = new List<int>();
        Sprite s;

        int hWidth, hHeight;
        Sprite buttonText;
        const int buttonPadding = 8;

        public bool Active = true;

        public Multibutton(MainControl mc, int x, int y, string text, Color c) : base(mc)
        {
            UIManager.OnClick += OnClick;

            this.buttonText = FontSprite.GetText(text, c, FontMedium.Font);
            
            this.width = this.buttonText.spriteWidth + buttonPadding;
            this.height = this.buttonText.spriteHeight + buttonPadding;

            this.hWidth = this.width / 2;
            this.hHeight = this.height / 2;

            s = new RectSprite(this.width, this.height, c);
            this.x = x;
            this.y = y;
        }

        protected override void OnClick(Player p, int x, int y)
        {
            if (!Active) return;

            if (x > this.x - hWidth && x < this.x + hWidth)
            {
                if (y > this.y - hHeight && y < this.y + hHeight)
                {
                    // clicked
                    if (playersClicked.Contains(p.id))
                    {
                        playersClicked.Remove(p.id);
                    }
                    else
                    {
                        playersClicked.Add(p.id);
                    }
                    OnButtonClick?.Invoke();
                }
            }
        }

        public override void Draw(RenderCanvas canvas)
        {
            if(!Active) return; 

            canvas.DrawSpriteCentered(s, this.x, this.y);
            for (int i = 0; i < playersClicked.Count; i++)
            {
                RectSprite rs = new RectSprite(
                    this.width + ((i + 1) * 4),
                    this.height + ((i + 1) * 4),
                    _main._players[playersClicked[i]].color);

                canvas.DrawSpriteCentered(rs, this.x, this.y);
            }
            canvas.DrawSpriteCentered(buttonText, this.x, this.y);
        }
    }
}

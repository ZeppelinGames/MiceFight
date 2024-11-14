using Editor.Sprites.Fonts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Sprites
{
    public static class Text
    {
        public static readonly Sprite TitleText = FontSprite.GetText("MICE FIGHT", Color.Black, FontMedium.Font);
        public static readonly Sprite TitleJoinText = FontSprite.GetText("CLICK LMB TO JOIN!", Color.Black, FontMedium.Font);
        public static readonly Sprite TestText = FontSprite.GetText("0123456789! +A B AB", Color.Black, FontMedium.Font);
        public static readonly Sprite TitleHint = FontSprite.GetText("LMB + RMB TO READY", Color.Black, FontMedium.Font);
        public static readonly Sprite GameoverText = FontSprite.GetText("GAMEOVER!", Color.Black, FontMedium.Font);
    }
}

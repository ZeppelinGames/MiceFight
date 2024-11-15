using Editor.Controls;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Gamestates
{
    public class HowToPlayState : GameState
    {
        public HowToPlayState(MainControl mc) : base(mc){}

        // Hold LMB to shoot
        // Hold RMB to block
        // Move to move

        public override void Draw(RenderCanvas canvas)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}

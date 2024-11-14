using System;
using System.Linq;
using System.Collections.Generic;
using Editor.Sprites.Fonts;
using Editor.Sprites;
using Linearstar.Windows.RawInput;
using Microsoft.Xna.Framework;
using Editor.Controls;

namespace Editor.Gamestates {
    public class GameoverGameState : GameState {
        public GameoverGameState(MainControl mc): base(mc) { }

        public override void Update(GameTime gameTime) {
            for (int i = 0; i < _main._players.Count; i++) {
                Player player = _main._players[i];
                if (player.mouseData.leftButton && player.mouseData.rightButton) {
                    MainControl.Instance.ResetGame();
                }
            }
        }
        public override void Draw(RenderCanvas canvas) {
            canvas.DrawSpriteCentered(Text.GameoverText, (int)(RenderCanvas.TARGET_WIDTH * 0.5f), (int)(RenderCanvas.TARGET_HEIGHT * 0.5f));
            canvas.DrawSpriteCentered(Text.TitleHint, (int)(RenderCanvas.TARGET_WIDTH * 0.5f), (int)(RenderCanvas.TARGET_HEIGHT * 0.5f) + 8);

            for (int i = 0; i < _main._players.Count; i++) {
                Player player = _main._players[i];
                player.Draw(canvas);
            }
        }
    }
}

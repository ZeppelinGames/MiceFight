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
        static readonly Sprite titleHint = FontSprite.GetText("LMB + RMB TO READY", Color.Black, FontMedium.Font);
        static readonly Sprite gameoverText = FontSprite.GetText("GAMEOVER!", Color.Black, FontMedium.Font);

        public override void Update(GameTime gameTime) {
            for (int i = 0; i < _players.Count; i++) {
                Player player = _players[i];
                if (player.mouseData.leftButton && player.mouseData.rightButton) {
                    MainControl.Instance.ResetGame();
                }
            }
        }
        public override void Draw(RenderCanvas canvas) {
            canvas.DrawSpriteCentered(gameoverText, (int)(RenderCanvas.TARGET_WIDTH * 0.5f), (int)(RenderCanvas.TARGET_HEIGHT * 0.5f));
            canvas.DrawSpriteCentered(titleHint, (int)(RenderCanvas.TARGET_WIDTH * 0.5f), (int)(RenderCanvas.TARGET_HEIGHT * 0.5f) + 8);

            for (int i = 0; i < _players.Count; i++) {
                Player player = _players[i];
                player.Draw(canvas);
            }
        }
    }
}

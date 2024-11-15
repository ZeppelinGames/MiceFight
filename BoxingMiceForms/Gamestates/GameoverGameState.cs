using System;
using System.Linq;
using System.Collections.Generic;
using Editor.Sprites.Fonts;
using Editor.Sprites;
using Linearstar.Windows.RawInput;
using Microsoft.Xna.Framework;
using Editor.Controls;
using static Editor.RawMouseData;

namespace Editor.Gamestates
{
    public class GameoverGameState : GameState
    {
        public static GameoverGameState Instance; 

        Multibutton restartButton;
        Player winner;
        Sprite winnerText;
        public GameoverGameState(MainControl mc) : base(mc)
        {
            Instance = this;
            restartButton = new Multibutton(mc, RenderCanvas.TARGET_WIDTH / 2, 170, "RESTART?", Color.Black);
            restartButton.OnButtonClick += RestartClick;
            restartButton.Active = false;
        }

        public void SetWinner(Player p)
        {
            winner = p;
            winnerText = FontSprite.GetText($"PLAYER WINS!", p.color, FontMedium.Font);
        }

        public void RestartClick()
        {
            if (_main._players.Count >= 2 &&
               restartButton.playersClicked.Count == _main._players.Count)
            {
                restartButton.playersClicked.Clear();
                _main.ResetGame();
            }
        }

        public override void OnActive()
        {
            if (restartButton != null)
            {
                restartButton.playersClicked.Clear();
                restartButton.Active = true;
            }
        }

        public override void OnInactive()
        {
            if (restartButton != null)
                restartButton.Active = false;
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < _main._players.Count; i++)
            {
                Player player = _main._players[i];
                MouseData mouseData = player.mouseData;

                player.x += mouseData.nDX;
                player.y += mouseData.nDY;
            }
            this.UpdatePlayers(gameTime);
        }
        public override void Draw(RenderCanvas canvas)
        {
            canvas.DrawSpriteCentered(Text.GameoverText, (int)(RenderCanvas.TARGET_WIDTH * 0.5f), (int)(RenderCanvas.TARGET_HEIGHT * 0.5f));
            // winner text
            canvas.DrawSpriteCentered(winnerText, (int)(RenderCanvas.TARGET_WIDTH * 0.5f), (int)(RenderCanvas.TARGET_HEIGHT * 0.5f) + 10);
            
            for (int i = 0; i < _main._players.Count; i++)
            {
                Player player = _main._players[i];
                player.Draw(canvas);
            }

            restartButton.Draw(canvas);
        }
    }
}

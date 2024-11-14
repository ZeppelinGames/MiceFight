using System;
using System.Collections.Generic;
using Editor.Controls;
using Editor.Sprites.Fonts;
using Editor.Sprites;
using Linearstar.Windows.RawInput;
using Linearstar.Windows.RawInput.Native;
using Microsoft.Xna.Framework;
using static Editor.RawMouseData;

namespace Editor.Gamestates
{
    public class TitleGameState : GameState
    {
        public TitleGameState(MainControl mc) : base(mc) { }

        public override void OnMouseInput(RawInputMouseData mouse)
        {
            RawInputDevice mouseDevice = mouse.Device;
            // The data will be an instance of RawInputMouseData
            // They contain the raw input data in their properties.
            if (mouse.Mouse.Buttons == RawMouseButtonFlags.LeftButtonDown)
            {
                if (MainControl.Instance.RegisterMouse(mouseDevice, out Player newPlayer))
                {
                    newPlayer.SetSprite(new CursorSprite(_main.GetPlayerColor()));
                }
            }
            if (mouseDevice == null || mouseDevice.DevicePath == null) return;
            if (mouse == null || !_main._playerPath.ContainsKey(mouseDevice.DevicePath)) return;

            MouseData connected = _main._playerPath[mouseDevice.DevicePath].mouseData;
            connected.UpdateKeys(mouse.Mouse);

            Player p = _main._players[connected.playerId];

            // Ready up
            if (connected.leftButton && connected.rightButton)
            {
                p.isReady = !p.isReady;
            }
        }

        public override void Update(GameTime gameTime)
        {
            int playerCount = _main._players.Count;
            for (int i = 0; i < playerCount; i++)
            {
                Player player = _main._players[i];
                MouseData mouseData = player.mouseData;

                player.x += mouseData.nDX;
                player.y += mouseData.nDY;
            }
            this.UpdatePlayers(gameTime);

            // Check if all players ready
            // Goto game state 
        }

        public override void Draw(RenderCanvas canvas)
        {
            int playerCount = _main._players.Count;

            for (int i = 0; i < playerCount; i++)
            {
                Player p = _main._players[i];
                p.Draw(canvas);
            }
            canvas.DrawSpriteCentered(Text.TitleText, (int)(RenderCanvas.TARGET_WIDTH * 0.5f), (int)(RenderCanvas.TARGET_HEIGHT * 0.5f));
            canvas.DrawSpriteCentered(Text.TitleJoinText, (int)(RenderCanvas.TARGET_WIDTH * 0.5f), (int)(RenderCanvas.TARGET_HEIGHT * 0.5f) + 8);
            canvas.DrawSpriteCentered(Text.TitleHint, (int)(RenderCanvas.TARGET_WIDTH * 0.5f), RenderCanvas.TARGET_HEIGHT - 12);
        }
    }
}
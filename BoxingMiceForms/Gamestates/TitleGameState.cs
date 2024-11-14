using System;
using System.Collections.Generic;
using Editor.Controls;
using Editor.Sprites.Fonts;
using Editor.Sprites;
using Linearstar.Windows.RawInput;
using Linearstar.Windows.RawInput.Native;
using Microsoft.Xna.Framework;
using static Editor.RawMouseData;
using static Editor.Sprite;

namespace Editor.Gamestates {
    public class TitleGameState : GameState {

        static readonly Sprite titleText = FontSprite.GetText("MICE FIGHT", Color.Black, FontMedium.Font);
        static readonly Sprite titleJoinText = FontSprite.GetText("CLICK LMB TO JOIN!", Color.Black, FontMedium.Font);
        static readonly Sprite testText = FontSprite.GetText("0123456789! +A B AB", Color.Black, FontMedium.Font);

        public override void OnMouseInput(RawInputMouseData mouse) {
            RawInputDevice mouseDevice = mouse.Device;
            // The data will be an instance of RawInputMouseData
            // They contain the raw input data in their properties.
            if (mouse.Mouse.Buttons == RawMouseButtonFlags.LeftButtonDown) {
                MainControl.Instance.RegisterMouse(mouseDevice);
            }
            if (mouseDevice == null || mouseDevice.DevicePath == null) return;
            if (mouse == null || !_playerPath.ContainsKey(mouseDevice.DevicePath)) return;

            MouseData connected = _playerPath[mouseDevice.DevicePath].mouseData;
            connected.UpdateKeys(mouse.Mouse);

            Player p = MainControl.Instance.GetPlayer(connected.playerId);
            if (!p.isReady) {
                if (connected.scroll > 0) {
                    p.playerColorIndex++;
                    p.playerColorIndex = p.playerColorIndex >= MainControl.PlayerColors.Length ? 0 : p.playerColorIndex;

                    p.color = MainControl.PlayerColors[p.playerColorIndex];
                }
                if (connected.scroll < 0) {
                    p.playerColorIndex--;
                    p.playerColorIndex = p.playerColorIndex < 0 ? MainControl.PlayerColors.Length - 1 : p.playerColorIndex;
                    p.color = MainControl.PlayerColors[p.playerColorIndex];
                }
            }

            // Ready up
            if (connected.leftButton && connected.rightButton) {
                p.isReady = !p.isReady;
            }
        }

        public override void Update(GameTime gameTime) {
            float step = (float)(Math.PI * 2) / Math.Max(1, _players.Count);
            int min = Math.Min(RenderCanvas.TARGET_WIDTH, RenderCanvas.TARGET_HEIGHT);
            int hmin = (int)(min * 0.5f);
            int radius = (int)(min * 0.25f);
            int moveRadius = 10;

            int playerCount = MainControl.Instance.PlayerCount;
            bool allReady = playerCount >= 2;
            for (int i = 0; i < playerCount; i++) {
                Player player = MainControl.Instance.GetPlayer(i);
                MouseData mouseData = player.mouseData;
                float a = step * i;

                float posX = ((float)Math.Cos(a) * radius) + hmin;
                float posY = ((float)Math.Sin(a) * radius) + hmin;
                player.x = posX;
                player.y = posY;

                if (!player.isReady) {
                    player.x += (int)(mouseData.nLDX * moveRadius);
                    player.y += (int)(mouseData.nLDY * moveRadius);
                }

                if (player.isReady) {
                    allReady = false;
                }
            }

            if (allReady) {
                _gameState = GAMESTATE.IN_GAME;
            }
        }

        public override void Draw(RenderCanvas canvas) {
            int playerCount = MainControl.Instance.PlayerCount;
            
            for (int i = 0; i < playerCount; i++) {
                _players[i].Draw(_canvas);

                if (_players[i].isReady) {
                    _canvas.DrawSpriteCentered(SPRITE_ID.TICK, (int)player.x, (int)player.y + 5);
                }
            }
            canvas.DrawSprite(testText, 25, 25);
            canvas.DrawSpriteCentered(titleText, (int)(RenderCanvas.TARGET_WIDTH * 0.5f), (int)(RenderCanvas.TARGET_HEIGHT * 0.5f));
            canvas.DrawSpriteCentered(titleJoinText, (int)(RenderCanvas.TARGET_WIDTH * 0.5f), (int)(RenderCanvas.TARGET_HEIGHT * 0.5f) + 8);
            canvas.DrawSpriteCentered(titleHint, (int)(RenderCanvas.TARGET_WIDTH * 0.5f), RenderCanvas.TARGET_HEIGHT - 12);
        }
    }
}

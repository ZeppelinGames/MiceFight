using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using Linearstar.Windows.RawInput;
using Linearstar.Windows.RawInput.Native;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Forms.Controls;
using Gma.System.MouseKeyHook;

using static Editor.RawMouseData;
using Color = Microsoft.Xna.Framework.Color;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using static Editor.Sprite;
using System.Diagnostics;
using Editor.Sprites;

namespace Editor.Controls {
    public enum GAMESTATE {
        TITLE,
        WAITING_FOR_PLAYERS,
        IN_GAME,
        GAMEOVER
    };

    public class MainControl : MonoGameControl {
        static Random rnd = new Random();
        private IKeyboardMouseEvents _mouseHook;

        private const int DEFAULT_SCREEN_WIDTH = 1000;
        private const int DEFAULT_SCREEN_HEIGHT = 1000;

        private const int TARGET_WIDTH = 256;
        private const int TARGET_HEIGHT = 256;

        private int _screenWidth;
        private int _screenHeight;

        private int _renderWidth;
        private int _renderHeight;

        private float _moveSpeed = 0.5f;

        private static Color BackgroundColor = new Color(241, 233, 210);

        Texture2D _renderTarget;
        Rectangle _renderTargetRect;

        Color[] _render = new Color[TARGET_WIDTH * TARGET_HEIGHT];
        Color[] _clearRender = new Color[TARGET_WIDTH * TARGET_HEIGHT];

        Dictionary<string, Player> _playerPath = new Dictionary<string, Player>();
        List<Player> _players = new List<Player>();

        // Game state vars
        GAMESTATE _gameState = GAMESTATE.TITLE;

        Sprite titleText, titleJoinText, titleHint;

        Color[] _playerColors = new Color[] {
            new Color(222, 110, 79),
            new Color(175, 160, 69),
            new Color(124, 127, 82),
            new Color(148, 181, 164),
            new Color(109, 114, 142),
            new Color(231, 184, 92),
            new Color(218, 141, 73),
            new Color(195, 129, 167),
            new Color(132, 100, 139),
            new Color(229, 138, 133),
        };

        public MainControl() {
            _screenWidth = DEFAULT_SCREEN_WIDTH;
            _screenHeight = DEFAULT_SCREEN_HEIGHT;

            _mouseHook = Hook.GlobalEvents();
            _mouseHook.MouseDownExt += GlobalHookMousePress;

            for (int i = 0; i < _clearRender.Length; i++) {
                _clearRender[i] = BackgroundColor;
            }

            titleText = FontSprite.GetText("BOXING MICE", Color.Black);
            titleJoinText = FontSprite.GetText("CLICK LMB TO JOIN!", Color.Black);
            titleHint = FontSprite.GetText("LMB + RMB TO READY", Color.Black);

            UpdateWindow();
        }

        private void GlobalHookMousePress(object sender, MouseEventExtArgs e) {
            e.Handled = true;
            return;
        }

        public void DeregisterHook() {
            _mouseHook.MouseDownExt -= GlobalHookMousePress;

            //It is recommened to dispose it
            _mouseHook.Dispose();
        }

        protected override void WndProc(ref Message m) {
            switch (m.Msg) {
                case (int)MouseEvents.WM_INPUT:
                    RawInputData inputData = RawInputData.FromHandle(m.LParam);
                    switch (inputData) {
                        case RawInputMouseData mouse:
                            RawInputDevice mouseDevice = mouse.Device;

                            // The data will be an instance of RawInputMouseData
                            // They contain the raw input data in their properties.
                            if (mouse.Mouse.Buttons == RawMouseButtonFlags.LeftButtonDown) {
                                RegisterMouse(mouseDevice);
                            }
                            if (mouse == null || !_playerPath.ContainsKey(mouseDevice.DevicePath)) return;

                            MouseData connected = _playerPath[mouseDevice.DevicePath].mouseData;
                            connected.UpdateKeys(mouse.Mouse);

                            Player p = _players[connected.playerId];
                            if (!p.isReady) {
                                if (connected.scroll > 0) {
                                    p.playerColorIndex++;
                                    p.playerColorIndex = p.playerColorIndex >= _playerColors.Length ? 0 : p.playerColorIndex;

                                    p.color = _playerColors[p.playerColorIndex];
                                }
                                if (connected.scroll < 0) {
                                    p.playerColorIndex--;
                                    p.playerColorIndex = p.playerColorIndex < 0 ? _playerColors.Length - 1 : p.playerColorIndex;
                                    p.color = _playerColors[p.playerColorIndex];
                                }
                            }

                            // Ready up
                            if (connected.leftButton && connected.rightButton) {
                                _players[connected.playerId].isReady = !_players[connected.playerId].isReady;
                                Debug.WriteLine("READY!");
                            }

                            break;
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        int playerId = 0;
        void RegisterMouse(RawInputDevice device) {
            if (!_playerPath.ContainsKey(device.DevicePath)) {
                Debug.WriteLine($"Added mouse: {device.DevicePath} {device.ProductName}");

                Player newPlayer = new Player(playerId);
                newPlayer.playerColorIndex = rnd.Next(0, _playerColors.Length);
                newPlayer.color = _playerColors[newPlayer.playerColorIndex];
                playerId++;

                _players.Add(newPlayer);
                _playerPath.Add(device.DevicePath, newPlayer);
            }
        }

        protected override void Initialize() {
            _renderTarget = new Texture2D(Editor.GraphicsDevice, TARGET_WIDTH, TARGET_HEIGHT);
            _renderTarget.SetData(_render);

            UpdateWindow();
            base.Initialize();
        }

        public void UpdateWindow() {
            if (MainForm.instance == null || MainForm.instance.mainControl == null) return;

            Size s = MainForm.instance.mainControl.Size;
            _screenWidth = s.Width;
            _screenHeight = s.Height;

            float scale = Math.Min(_screenWidth / TARGET_WIDTH, _screenHeight / TARGET_HEIGHT);
            int newWidth = (int)(TARGET_WIDTH * scale);
            int newHeight = (int)(TARGET_HEIGHT * scale);

            _renderWidth = (int)Clamp(newWidth, 0, 2048);
            _renderHeight = (int)Clamp(newHeight, 0, 2048);

            _renderTargetRect = new Rectangle(
                (_screenWidth - _renderWidth) / 2,
                (_screenHeight - _renderHeight) / 2,
                _renderWidth,
                _renderHeight);
        }

        protected override void Update(GameTime gameTime) {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Escape)) {
                // close game
                MainForm.instance.CloseGame();
            }

            switch (_gameState) {
                case GAMESTATE.TITLE:
                    // Draw title
                    TitleUpdate(gameTime);
                    break;
                case GAMESTATE.WAITING_FOR_PLAYERS:
                    LobbyUpdate(gameTime);
                    break;
                case GAMESTATE.IN_GAME:
                    GameUpdate(gameTime);
                    break;
                case GAMESTATE.GAMEOVER:
                    break;
            }

            for (int i = 0; i < _players.Count; i++) {
                _players[i].mouseData.Update(gameTime);
            }

            // Lock mouse center screen
            System.Drawing.Point formCenter = new System.Drawing.Point(
             MainForm.instance.Location.X + (int)(MainForm.instance.Width * 0.5f),
             MainForm.instance.Location.Y + (int)(MainForm.instance.Height * 0.5f)
            );

            Cursor.Position = formCenter;

            base.Update(gameTime);
        }

        protected override void Draw() {
            // Update texture
            _renderTarget.SetData(_render);

            // Clear screen
            Editor.GraphicsDevice.Clear(Color.Black);

            // Draw render texture
            Editor.spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            Editor.spriteBatch.Draw(_renderTarget, _renderTargetRect, Color.White);
            Editor.spriteBatch.End();

            // Clear draw buffer
            Array.Copy(_clearRender, _render, _render.Length);

            base.Draw();
        }

        void TitleUpdate(GameTime gameTime) {
            float step = (float)(Math.PI * 2) / Math.Max(1, _players.Count);
            int min = Math.Min(TARGET_WIDTH, TARGET_HEIGHT);
            int hmin = (int)(min * 0.5f);
            int radius = (int)(min * 0.25f);
            int moveRadius = 10;

            for (int i = 0; i < _players.Count; i++) {
                Player player = _players[i];
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

                DrawCircle(player.X, player.Y, 4, player.color);

                if (_players[i].isReady) {
                    DrawSpriteCentered(SPRITE_ID.TICK, (int)player.x, (int)player.y + 5);
                }
            }

            DrawSpriteCentered(titleText, (int)(TARGET_WIDTH * 0.5f), (int)(TARGET_HEIGHT * 0.5f));
            DrawSpriteCentered(titleJoinText, (int)(TARGET_WIDTH * 0.5f), (int)(TARGET_HEIGHT * 0.5f) + 8);
            DrawSpriteCentered(titleHint, (int)(TARGET_WIDTH * 0.5f), TARGET_HEIGHT - 12);
        }

        void LobbyUpdate(GameTime gameTime) {

        }

        void GameUpdate(GameTime gameTime) {
            for (int i = 0; i < _players.Count; i++) {
                Player player = _players[i];
                MouseData mouseData = player.mouseData;
                player.x += mouseData.nDX * _moveSpeed;
                player.y += mouseData.nDY * _moveSpeed;

                DrawCircle(player.X, player.Y, 4, player.color);

                float rayCX = player.x + player.mouseData.nDX;
                float rayCY = player.Y + player.mouseData.nDY;
                DrawLine(player.X, player.Y, (int)rayCX, (int)rayCY, Color.Red);

                if (_players[i].isReady) {
                    DrawSpriteCentered(SPRITE_ID.TICK, (int)player.x, (int)player.y + 5);
                }

                player.mouseData.Update(gameTime);
            }
        }

        void DrawSprite(SPRITE_ID spriteId, int xPos, int yPos) {
            if (Sprite.GetSprite(spriteId, out Sprite sprite)) {
                DrawSprite(sprite, xPos, yPos);
            }
        }
        void DrawSpriteCentered(SPRITE_ID spriteId, int xPos, int yPos) {
            if (Sprite.GetSprite(spriteId, out Sprite sprite)) {
                DrawSpriteCentered(sprite, xPos, yPos);
            }
        }
        void DrawSpriteCentered(Sprite sprite, int xPos, int yPos) {
            DrawSprite(sprite, xPos - (int)(sprite.spriteWidth * 0.5f), yPos);
        }
        void DrawSprite(Sprite sprite, int xPos, int yPos) {
            for (int y = 0; y < sprite.sprite.Length; y++) {
                for (int x = 0; x < sprite.sprite[y].Length; x++) {
                    SetPixel(x + xPos, y + yPos, sprite[y, x]);
                }
            }
        }

        private void SetPixel(int x, int y, Color c) {
            if (c.A == 0) {
                return;
            }

            if (x >= 0 && x < TARGET_WIDTH) {
                if (y >= 0 && y < TARGET_HEIGHT) {
                    _render[x + (y * TARGET_WIDTH)] = c;
                }
            }
        }

        private void DrawCircle(int px, int py, int radius, Color c) {
            for (int y = -radius; y <= radius; y++)
                for (int x = -radius; x <= radius; x++)
                    if (x * x + y * y <= radius * radius)
                        SetPixel(px + x, py + y, c);
        }

        public void DrawLine(int x, int y, int x2, int y2, Color color) {
            int w = x2 - x;
            int h = y2 - y;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
            int longest = Math.Abs(w);
            int shortest = Math.Abs(h);
            if (!(longest > shortest)) {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            int numerator = longest >> 1;
            for (int i = 0; i <= longest; i++) {
                SetPixel(x, y, color);
                numerator += shortest;
                if (!(numerator < longest)) {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                } else {
                    x += dx2;
                    y += dy2;
                }
            }
        }

        float Clamp(float v, float min, float max) {
            return Math.Max(Math.Min(v, max), min);
        }

        (float, float) Normalise(float x, float y) {
            if (x == 0 && y == 0) return (x, y);

            float dist = (float)Math.Sqrt(x * x + y * y);
            return (x / dist, y / dist);
        }
    }
}

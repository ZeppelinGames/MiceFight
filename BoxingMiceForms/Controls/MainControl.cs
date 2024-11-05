using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

using Linearstar.Windows.RawInput;
using Linearstar.Windows.RawInput.Native;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Forms.Controls;
using Gma.System.MouseKeyHook;

using Keys = Microsoft.Xna.Framework.Input.Keys;
using Color = Microsoft.Xna.Framework.Color;
using Editor.Sprites;
using static Editor.Sprite;
using static Editor.RawMouseData;

namespace Editor.Controls {
    public enum GAMESTATE {
        TITLE,
        IN_GAME,
        GAMEOVER
    };

    public class MainControl : MonoGameControl {
        static Random rnd = new Random();
        private IKeyboardMouseEvents _mouseHook;

        private RenderCanvas _canvas;

        private float _moveSpeed = 0.5f;

        Dictionary<string, Player> _playerPath = new Dictionary<string, Player>();
        List<Player> _players = new List<Player>();
        int playerId = 0;

        List<Bullet> _bullets = new List<Bullet>();

        // Game state vars
        GAMESTATE _gameState = GAMESTATE.TITLE;

        static readonly Sprite titleText  = FontSprite.GetText("MICE FIGHT", Color.Black);
        static readonly Sprite titleJoinText = FontSprite.GetText("CLICK LMB TO JOIN!", Color.Black);
        static readonly Sprite titleHint = FontSprite.GetText("LMB + RMB TO READY", Color.Black);
        static readonly Sprite gameoverText = FontSprite.GetText("GAMEOVER!", Color.Black);

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
            _mouseHook = Hook.GlobalEvents();
            _mouseHook.MouseDownExt += GlobalHookMousePress;

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
                            InputState(mouse);
                            break;
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        void InputState(RawInputMouseData mouse) {
            switch (_gameState) {
                case GAMESTATE.TITLE:
                    // Draw title
                    TitleInput(mouse);
                    break;
                case GAMESTATE.IN_GAME:
                    GameInput(mouse);
                    break;
                case GAMESTATE.GAMEOVER:
                    GameInput(mouse);
                    break;
            }
        }

        void TitleInput(RawInputMouseData mouse) {
            RawInputDevice mouseDevice = mouse.Device;
            // The data will be an instance of RawInputMouseData
            // They contain the raw input data in their properties.
            if (mouse.Mouse.Buttons == RawMouseButtonFlags.LeftButtonDown) {
                RegisterMouse(mouseDevice);
            }
            if (mouseDevice == null) return;
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
            }
        }

        void GameInput(RawInputMouseData mouse) {
            RawInputDevice mouseDevice = mouse.Device;
            if (mouseDevice == null) return;
            if (mouse == null || !_playerPath.ContainsKey(mouseDevice.DevicePath)) return;

            MouseData connected = _playerPath[mouseDevice.DevicePath].mouseData;
            connected.UpdateKeys(mouse.Mouse);
        }

        void SpawnBullet(Player player) {
            if (player.mouseData.nLDX == 0 && player.mouseData.nLDY == 0) return;

            Bullet b = new Bullet(player);
            _bullets.Add(b);
        }

        void RegisterMouse(RawInputDevice device) {
            if (device == null) return;
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
            _canvas = new RenderCanvas(Editor.GraphicsDevice);

            UpdateWindow();
            base.Initialize();
        }

        public void UpdateWindow() {
            if (_canvas == null) return;
            _canvas.Resize();
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
                case GAMESTATE.IN_GAME:
                    GameUpdate(gameTime);
                    break;
                case GAMESTATE.GAMEOVER:
                    GameoverUpdate(gameTime);
                    break;
            }

            for (int i = 0; i < _bullets.Count; i++) {
                _bullets[i].Update(gameTime);
                _canvas.DrawLine(
                    _bullets[i].X,
                    _bullets[i].Y,
                    _bullets[i].X + (int)_bullets[i].dx,
                    _bullets[i].Y + (int)_bullets[i].dy,
                    _players[_bullets[i].playerId].color);
            }
            for (int i = 0; i < _players.Count; i++) {
                // Run last
                _players[i].Update(gameTime);
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
            // Canvas draw
            _canvas.Draw(Editor.spriteBatch);

            base.Draw();
        }

        void TitleUpdate(GameTime gameTime) {
            float step = (float)(Math.PI * 2) / Math.Max(1, _players.Count);
            int min = Math.Min(RenderCanvas.TARGET_WIDTH, RenderCanvas.TARGET_HEIGHT);
            int hmin = (int)(min * 0.5f);
            int radius = (int)(min * 0.25f);
            int moveRadius = 10;

            bool allReady = _players.Count >= 2;
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

                _canvas.DrawCircle(player.X, player.Y, 4, player.color);

                float rayCX = player.x + player.mouseData.nDX;
                float rayCY = player.Y + player.mouseData.nDY;
                _canvas.DrawLine(player.X, player.Y, (int)rayCX, (int)rayCY, Color.Red);

                if (_players[i].isReady) {
                    _canvas.DrawSpriteCentered(SPRITE_ID.TICK, (int)player.x, (int)player.y + 5);
                } else {
                    allReady = false;
                }
            }

            if (allReady) {
                _gameState = GAMESTATE.IN_GAME;
            }

            _canvas.DrawSpriteCentered(titleText, (int)(RenderCanvas.TARGET_WIDTH * 0.5f), (int)(RenderCanvas.TARGET_HEIGHT * 0.5f));
            _canvas.DrawSpriteCentered(titleJoinText, (int)(RenderCanvas.TARGET_WIDTH * 0.5f), (int)(RenderCanvas.TARGET_HEIGHT * 0.5f) + 8);
            _canvas.DrawSpriteCentered(titleHint, (int)(RenderCanvas.TARGET_WIDTH * 0.5f), RenderCanvas.TARGET_HEIGHT - 12);
        }

        void GameUpdate(GameTime gameTime) {
            for (int i = 0; i < _players.Count; i++) {
                Player player = _players[i];

                if (!player.isAlive) {
                    _canvas.DrawCircle(player.X, player.Y, Player.size, Color.Gray);
                    continue;
                }

                MouseData mouseData = player.mouseData;

                player.x += mouseData.nDX * _moveSpeed;
                player.y += mouseData.nDY * _moveSpeed;

                _canvas.DrawCircle(player.X, player.Y, Player.size, player.color);

                float rayCX = player.x + player.mouseData.nDX;
                float rayCY = player.Y + player.mouseData.nDY;
                _canvas.DrawLine(player.X, player.Y, (int)rayCX, (int)rayCY, Color.Red);

                if (_players[i].mouseData.rightButton) {
                    _canvas.DrawRotatedSprite(player.shieldSprite, player.X, player.Y, player.shieldRotation);
                } else {
                    if (_players[i].mouseData.leftButton) {
                        SpawnBullet(_players[i]);
                    }
                }
            }

            // Check if you shot someone
            List<Bullet> blockedBullets = new List<Bullet>();
            int alivePlayerCount = 0;
            for (int j = 0; j < _players.Count; j++) {
                for (int i = 0; i < _bullets.Count; i++) {
                    if (_bullets[i].playerId == _players[j].id) continue;
                    if (CirclePointIntersect(
                        _bullets[i].x,
                        _bullets[i].y,
                        _players[j].x,
                        _players[j].y,
                        Player.size)) {

                        // Blocking?
                        if (_players[j].mouseData.rightButton) {
                            blockedBullets.Add(_bullets[i]);
                        } else {
                            Debug.WriteLine($"{_bullets[i].playerId} killed {_players[j].id}");
                            _players[j].Kill();
                        }
                    }
                }

                if (_players[j].isAlive) {
                    alivePlayerCount++;
                }
            }

            // Get rid of them blocked ones
            for (int i = 0; i < blockedBullets.Count; i++) {
                _bullets.Remove(blockedBullets[i]);
            }

            if (alivePlayerCount <= 1) {
                _gameState = GAMESTATE.GAMEOVER;
            }
        }

        void GameoverUpdate(GameTime gameTime) {
            _canvas.DrawSpriteCentered(gameoverText, (int)(RenderCanvas.TARGET_WIDTH * 0.5f), (int)(RenderCanvas.TARGET_HEIGHT * 0.5f));
            _canvas.DrawSpriteCentered(titleHint, (int)(RenderCanvas.TARGET_WIDTH * 0.5f), (int)(RenderCanvas.TARGET_HEIGHT * 0.5f) + 8);

            for (int i = 0; i < _players.Count; i++) {
                Player player = _players[i];
                _canvas.DrawCircle(player.X, player.Y, Player.size, player.isAlive ? player.color : Color.Gray);

                if (player.mouseData.leftButton && player.mouseData.rightButton) {
                    ResetGame();
                }
            }
        }

        void ResetGame() {
            _players.Clear();
            _bullets.Clear();
            _playerPath.Clear();
            playerId = 0;
            _gameState = GAMESTATE.TITLE;
        }

        public bool CirclePointIntersect(float x, float y, float cx, float cy, float r) {
            return Math.Sqrt(Math.Pow(cx - x, 2) + Math.Pow(cy - y, 2)) < r;
        }

        bool CircleCircleIntersect() {
            return false;
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

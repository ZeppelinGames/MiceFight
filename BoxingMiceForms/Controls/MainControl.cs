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

namespace Editor.Controls {
    public enum GAMESTATE {
        TITLE,
        WAITING_FOR_PLAYERS,
        IN_GAME,
        GAMEOVER
    };

    public class MainControl : MonoGameControl {
        private IKeyboardMouseEvents _mouseHook;

        private const int DEFAULT_SCREEN_WIDTH = 1000;
        private const int DEFAULT_SCREEN_HEIGHT = 1000;

        private const int TARGET_WIDTH = 128;
        private const int TARGET_HEIGHT = 128;

        private int _screenWidth;
        private int _screenHeight;

        private int _renderWidth;
        private int _renderHeight;

        private float _mouseSens = 0.01f;

        private static Color BackgroundColor = Color.White;

        Texture2D _renderTarget;
        Rectangle _renderTargetRect;

        Color[] _render = new Color[TARGET_WIDTH * TARGET_HEIGHT];
        Color[] _clearRender = new Color[TARGET_WIDTH * TARGET_HEIGHT];

        Dictionary<string, MouseData> _mouseData = new Dictionary<string, MouseData>();
        List<string> _mouseIds = new List<string>();

        // Game state vars
        GAMESTATE _gameState = GAMESTATE.TITLE;
        int _playersReady = 0;

        public MainControl() {
            _screenWidth = DEFAULT_SCREEN_WIDTH;
            _screenHeight = DEFAULT_SCREEN_HEIGHT;

            _mouseHook = Hook.GlobalEvents();
            _mouseHook.MouseDownExt += GlobalHookMousePress;

            for (int i = 0; i < _clearRender.Length; i++) {
                _clearRender[i] = BackgroundColor;
            }

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
            const int WM_INPUT = 0x00FF;

            // You can read inputs by processing the WM_INPUT message.
            if (m.Msg == WM_INPUT) {
                // Create an RawInputData from the handle stored in lParam.
                var data = RawInputData.FromHandle(m.LParam);

                // You can identify the source device using Header.DeviceHandle or just Device.
                RawInputDeviceHandle sourceDeviceHandle = data.Header.DeviceHandle;
                RawInputDevice sourceDevice = data.Device;

                // The data will be an instance of RawInputMouseData
                // They contain the raw input data in their properties.
                switch (data) {
                    case RawInputMouseData mouse:
                        if (mouse == null) return;
                        RawInputDevice mouseDevice = mouse.Device;

                        RegisterMouse(mouseDevice);

                        MouseData connected = _mouseData[mouseDevice.DevicePath];

                        connected.Keys = (MouseKeys)m.WParam.ToInt32();

                        // Swap X & Y (it works, dont ask)
                        connected.X += mouse.Mouse.LastY * _mouseSens;
                        connected.Y += mouse.Mouse.LastX * _mouseSens;

                        connected.X = Math.Max(Math.Min(connected.X, this.Size.Width), 0);
                        connected.Y = Math.Max(Math.Min(connected.Y, this.Size.Height), 0);

                        connected.deltaX = mouse.Mouse.LastY;
                        connected.deltaY = mouse.Mouse.LastX;

                        if (mouse.Mouse.LastX != 0 && mouse.Mouse.LastY != 0) {
                            connected.lastDeltaX = mouse.Mouse.LastY;
                            connected.lastDeltaY = mouse.Mouse.LastX;
                        }

                        break;
                }
            }

            base.WndProc(ref m);
        }

        void RegisterMouse(RawInputDevice device) {
            if (!_mouseData.ContainsKey(device.DevicePath)) {
                _mouseIds.Add(device.DevicePath);
                _mouseData.Add(device.DevicePath, new MouseData(TARGET_WIDTH / 2, TARGET_HEIGHT / 2));
            }
        }

        protected override void Initialize() {
            _renderTarget = new Texture2D(Editor.GraphicsDevice, TARGET_WIDTH, TARGET_HEIGHT);

            /* Screen test
            for (int x = 0; x < TARGET_WIDTH; x++)
            {
                for (int y = 0; y < TARGET_HEIGHT; y++)
                {
                    SetPixel(x, y, x == y ? Color.Red : Color.White);
                }
            }
            SetPixel(0, 0, Color.Red);
            SetPixel(TARGET_WIDTH - 1, TARGET_HEIGHT - 1, Color.Red); 
            */

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
            float step = (float)Math.PI / (_mouseIds.Count - 1);
            int min = Math.Min(TARGET_WIDTH, TARGET_HEIGHT);
            int hmin = (int)(min * 0.5f);
            int radius = (int)(min * 0.25f);
            int moveRadius = 10;

            for (int i = 0; i < _mouseIds.Count; i++) {
                MouseData mouseData = _mouseData[_mouseIds[i]];
                (float nx, float ny) = Normalise(mouseData.lastDeltaX, mouseData.lastDeltaY);
                float a = step * i;
                DrawCircle((int)(Math.Cos(a) * radius) + hmin + (int)(nx * moveRadius), (int)(Math.Sin(a) * radius) + hmin + (int)(ny * moveRadius), 2, mouseData.color);
            }
        }

        void LobbyUpdate(GameTime gameTime) {

        }

        void GameUpdate(GameTime gameTime) {
            for (int i = 0; i < _mouseIds.Count; i++) {
                MouseData mouseData = _mouseData[_mouseIds[i]];
                DrawCircle((int)mouseData.X, (int)mouseData.Y, 2, mouseData.color);
            }
        }

        void DrawSprite(SPRITE_ID spriteId, int xPos, int yPos) {
            if (Sprite.GetSprite(spriteId, out Sprite sprite)) {
                for (int x = 0; x < sprite.sprite.Length; x++) {
                    for (int y = 0; y < sprite.sprite[x].Length; y++) {
                        if (sprite.sprite[x][y] != -1) {
                            SetPixel(x + xPos, y + yPos, sprite[x, y]);
                        }
                    }
                }
            }
        }

        private void SetPixel(int x, int y, Color c) {
            if (c.A == 0) {
                return;
            }

            if (x >= 0 && x < TARGET_WIDTH) {
                if (y >= 0 && y < TARGET_HEIGHT) {
                    _render[y + (x * TARGET_WIDTH)] = c;
                }
            }
        }

        private void DrawCircle(int px, int py, int radius, Color c) {
            for (int y = -radius; y <= radius; y++)
                for (int x = -radius; x <= radius; x++)
                    if (x * x + y * y <= radius * radius)
                        SetPixel(px + x, py + y, c);
        }

        float Clamp(float v, float min, float max) {
            return Math.Max(Math.Min(v, max), min);
        }

        (float, float) Normalise(float x, float y) {
            float dist = (float)Math.Sqrt(x * x + y * y);
            return (x / dist, y / dist);
        }
    }
}

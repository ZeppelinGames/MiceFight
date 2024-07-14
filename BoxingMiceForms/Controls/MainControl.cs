using Linearstar.Windows.RawInput;
using Linearstar.Windows.RawInput.Native;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Forms.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static Editor.RawMouseData;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Color = Microsoft.Xna.Framework.Color;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

using Gma.System.MouseKeyHook;
using SharpDX.DirectWrite;

namespace Editor.Controls {
    public class MainControl : MonoGameControl {
        //private GraphicsDeviceManager _graphics;
        private IKeyboardMouseEvents _mouseHook;
        private SpriteBatch _spriteBatch;

        private const int DEFAULT_SCREEN_WIDTH = 1000;
        private const int DEFAULT_SCREEN_HEIGHT = 1000;

        private const int TARGET_WIDTH = 96;
        private const int TARGET_HEIGHT = 96;

        private int _screenWidth;
        private int _screenHeight;

        private int _renderWidth;
        private int _renderHeight;

        private float _mouseSens = 0.01f;

        Texture2D renderTarget;
        Rectangle renderTargetRect;

        Color[] render = new Color[TARGET_WIDTH * TARGET_HEIGHT];

        Dictionary<string, MouseData> mousePositions = new Dictionary<string, MouseData>();
        List<string> mouseIds = new List<string>();

        public MainControl() {
            _screenWidth = DEFAULT_SCREEN_WIDTH;
            _screenHeight = DEFAULT_SCREEN_HEIGHT;

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
            const int WM_INPUT = 0x00FF;

            // You can read inputs by processing the WM_INPUT message.
            if (m.Msg == WM_INPUT) {
                // Create an RawInputData from the handle stored in lParam.
                var data = RawInputData.FromHandle(m.LParam);

                // You can identify the source device using Header.DeviceHandle or just Device.
                RawInputDeviceHandle sourceDeviceHandle = data.Header.DeviceHandle;
                RawInputDevice sourceDevice = data.Device;

                // The data will be an instance of either RawInputMouseData, RawInputKeyboardData, or RawInputHidData.
                // They contain the raw input data in their properties.
                switch (data) {
                    case RawInputMouseData mouse:
                        if (mouse == null) return;
                        RawInputDevice mouseDevice = mouse.Device;

                        RegisterMouse(mouseDevice);

                        MouseData connected = mousePositions[mouseDevice.DevicePath];

                        connected.Keys = (MouseKeys)m.WParam.ToInt32();

                        connected.X += mouse.Mouse.LastY * _mouseSens;
                        connected.Y += mouse.Mouse.LastX * _mouseSens;

                        connected.X = Math.Max(Math.Min(connected.X, this.Size.Width), 0);
                        connected.Y = Math.Max(Math.Min(connected.Y, this.Size.Height), 0);
                        break;
                }
            }

            base.WndProc(ref m);
        }

        void RegisterMouse(RawInputDevice device) {
            if (!mousePositions.ContainsKey(device.DevicePath)) {
                mouseIds.Add(device.DevicePath);
                mousePositions.Add(device.DevicePath, new MouseData(TARGET_WIDTH / 2, TARGET_HEIGHT / 2));
            }
        }

        protected override void Initialize() {
            //  base.Window.Title = "Boxing Mice";
            renderTarget = new Texture2D(Editor.GraphicsDevice, TARGET_WIDTH, TARGET_HEIGHT);

            for (int x = 0; x < TARGET_WIDTH; x++) {
                for (int y = 0; y < TARGET_HEIGHT; y++) {
                    SetPixel(x, y, x == y ? Color.Red : Color.White);
                }
            }
            SetPixel(0, 0, Color.Red);
            SetPixel(TARGET_WIDTH - 1, TARGET_HEIGHT - 1, Color.Red);

            renderTarget.SetData(render);

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

            renderTargetRect = new Rectangle(
                (_screenWidth - _renderWidth) / 2,
                (_screenHeight - _renderHeight) / 2,
                _renderWidth,
                _renderHeight);
        }

        protected void LoadContent() {
            _spriteBatch = new SpriteBatch(Editor.GraphicsDevice);
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) {
                // close game

                MainForm.instance.CloseGame();
            }

            for (int i = 0; i < mouseIds.Count; i++) {
                MouseData mouseData = mousePositions[mouseIds[i]];
                SetPixel((int)mouseData.X, (int)mouseData.Y, mouseData.color);
            }

            System.Drawing.Point formCenter = new System.Drawing.Point(
                MainForm.instance.Location.X + (int)(MainForm.instance.Width * 0.5f),
                MainForm.instance.Location.Y + (int)(MainForm.instance.Height* 0.5f)
            );

            //Cursor.Position = formCenter;

            base.Update(gameTime);
        }

        protected override void Draw() {
            // Update texture
            renderTarget.SetData(render);

            // Clear screen
            Editor.GraphicsDevice.Clear(Color.Black);

            // Draw render texture
            Editor.spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            Editor.spriteBatch.Draw(renderTarget, renderTargetRect, Color.White);
            Editor.spriteBatch.End();

            for (int x = 0; x < TARGET_WIDTH; x++) {
                for (int y = 0; y < TARGET_HEIGHT; y++) {
                    render[y + (x * TARGET_WIDTH)] = Color.Black;
                }
            }

            base.Draw();
        }

        private void SetPixel(int x, int y, Color c) {
            if (c.A == 0) {
                return;
            }

            if (x >= 0 && x < TARGET_WIDTH) {
                if (y >= 0 && y < TARGET_HEIGHT) {
                    render[y + (x * TARGET_WIDTH)] = c;
                }
            }
        }

        float Clamp(float v, float min, float max) {
            return Math.Max(Math.Min(v, max), min);
        }
    }
}

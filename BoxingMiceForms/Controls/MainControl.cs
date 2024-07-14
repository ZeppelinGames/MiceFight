using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Forms.Controls;
using MonoGame.Forms.Services;
using System;
using System.Drawing;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Editor.Controls {
    public class MainControl : MonoGameControl {
        //private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private const int DEFAULT_SCREEN_WIDTH = 640;
        private const int DEFAULT_SCREEN_HEIGHT = 640;

        private const int TARGET_WIDTH = 64;
        private const int TARGET_HEIGHT = 64;

        private int _screenWidth;
        private int _screenHeight;

        private int _renderWidth;
        private int _renderHeight;

        Texture2D renderTarget;
        Rectangle renderTargetRect;

        Color[] render = new Color[TARGET_WIDTH * TARGET_HEIGHT];

        public MainControl() {
            /*   _graphics = new GraphicsDeviceManager(this.);
               _graphics.IsFullScreen = false;*/

            _screenWidth = DEFAULT_SCREEN_WIDTH;
            _screenHeight = DEFAULT_SCREEN_HEIGHT;

            /*    _graphics.PreferredBackBufferWidth = _screenWidth;
               _graphics.PreferredBackBufferHeight = _screenHeight;

              Content.RootDirectory = "Content";
               IsMouseVisible = true;*/

            UpdateWindow();
            //  Window.ClientSizeChanged += ClientChangedWindowSize;
            //  Window.AllowUserResizing = true;
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
            if (Form1.instance == null || Form1.instance.mainControl == null) return;

            Size s = Form1.instance.mainControl.Size;
            _screenWidth = s.Width;
            _screenHeight = s.Height;

            float scale = Math.Min(_screenWidth / TARGET_WIDTH, _screenHeight / TARGET_HEIGHT);
            int newWidth = (int)(TARGET_WIDTH * scale);
            int newHeight = (int)(TARGET_HEIGHT * scale);

            _renderWidth = newWidth;
            _renderHeight = newHeight;

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
            }


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
    }
}

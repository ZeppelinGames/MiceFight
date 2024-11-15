using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Editor.Sprite;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Editor {
    public class RenderCanvas {
        private const int DEFAULT_SCREEN_WIDTH = 1000;
        private const int DEFAULT_SCREEN_HEIGHT = 1000;

        public const int TARGET_WIDTH = 256;
        public const int TARGET_HEIGHT = 256;

        private int _screenWidth;
        private int _screenHeight;

        private int _renderWidth;
        private int _renderHeight;

        Texture2D _renderTarget;
        Rectangle _renderTargetRect;

        Color[] _render = new Color[TARGET_WIDTH * TARGET_HEIGHT];
        Color[] _clearRender = new Color[TARGET_WIDTH * TARGET_HEIGHT];

        static readonly System.Drawing.Color WindowColor = System.Drawing.Color.FromArgb(61, 54, 69);
        static readonly Color BackgroundColor = new Color(241, 233, 210);

        GraphicsDevice _graphicsDevice;

        int _screenshakeAmplitude = 10;
        float _screenshakeTime = 2f;

        public RenderCanvas(GraphicsDevice graphics) {
            _screenWidth = DEFAULT_SCREEN_WIDTH;
            _screenHeight = DEFAULT_SCREEN_HEIGHT;

            // BG colour around main render
            MainForm.instance.mainFormLoaded += MainFormLoad;

            for (int i = 0; i < _clearRender.Length; i++) {
                _clearRender[i] = BackgroundColor;
            }

            this._graphicsDevice = graphics;
            _renderTarget = new Texture2D(this._graphicsDevice, TARGET_WIDTH, TARGET_HEIGHT);
            _renderTarget.SetData(_render);
        }

        void MainFormLoad() {
            MainForm.instance.SetBGColor(WindowColor);
        }

        public void Resize() {
            Rectangle s = _graphicsDevice.PresentationParameters.Bounds;
            _screenWidth = s.Width;
            _screenHeight = s.Height;

            float scale = Math.Min(_screenWidth / TARGET_WIDTH, _screenHeight / TARGET_HEIGHT);
            int newWidth = (int)(TARGET_WIDTH * scale);
            int newHeight = (int)(TARGET_HEIGHT * scale);

            _renderWidth = (int)MathHelper.Clamp(newWidth, 0, 2048);
            _renderHeight = (int)MathHelper.Clamp(newHeight, 0, 2048);

            _renderTargetRect = new Rectangle(
                (_screenWidth - _renderWidth) / 2,
                (_screenHeight - _renderHeight) / 2,
                _renderWidth,
                _renderHeight);
        }

        public void Draw(SpriteBatch spriteBatch) {
            // Update texture
            _renderTarget.SetData(_render);

            // Clear screen
            _graphicsDevice.Clear(BackgroundColor);

            // Draw render texture
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(_renderTarget, _renderTargetRect, Color.White);
            spriteBatch.End();

            // Clear draw buffer
            Array.Copy(_clearRender, _render, _render.Length);
        }

        public void SetPixel(int x, int y, Color c) {
            if (c.A == 0) {
                return;
            }

            if (x >= 0 && x < TARGET_WIDTH) {
                if (y >= 0 && y < TARGET_HEIGHT) {
                    _render[x + (y * TARGET_WIDTH)] = c;
                }
            }
        }

        public (float, float) RotatePoint(float x, float y, float angle) {
            return (x * (float)Math.Cos(angle) - y * (float)Math.Sin(angle),
                x * (float)Math.Sin(angle) + y * (float)Math.Cos(angle));
        }

        public void DrawCircle(int px, int py, int radius, Color c) {
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

        public void DrawSpriteCentered(SPRITE_ID spriteId, int xPos, int yPos) {
            if (Sprite.GetSprite(spriteId, out Sprite sprite)) {
                DrawSpriteCentered(sprite, xPos, yPos);
            }
        }
        public void DrawSpriteCentered(Sprite sprite, int xPos, int yPos) {
            DrawSprite(sprite, xPos - (int)(sprite.spriteWidth * 0.5f), yPos - (int)(sprite.spriteHeight * 0.5f));
        }
        public void DrawSprite(Sprite sprite, int xPos, int yPos) {
            for (int y = 0; y < sprite.sprite.Length; y++) {
                for (int x = 0; x < sprite.sprite[y].Length; x++) {
                    SetPixel(x + xPos, y + yPos, sprite[y, x]);
                }
            }
        }

        public void DrawRotatedSprite(Sprite sprite, int xPos, int yPos, float angle) {
            float hWidth = sprite.spriteWidth * 0.5f;
            float hHeight = sprite.spriteHeight * 0.5f;

            for (int y = 0; y < sprite.sprite.Length; y++) {
                for (int x = 0; x < sprite.sprite[y].Length; x++) {
                    // Rotate point
                    (float rx, float ry) = RotatePoint(x - hWidth, y - hHeight, angle);

                    // Draw rotated point
                    SetPixel((int)rx + xPos, (int)ry + yPos, sprite[y, x]);
                }
            }
        }
    }
}

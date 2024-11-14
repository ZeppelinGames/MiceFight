using Linearstar.Windows.RawInput;
using Microsoft.Xna.Framework;
using System;
using static Editor.RawMouseData;

namespace Editor.Gamestates {
    public abstract class GameState {
        public virtual void OnMouseInput(RawInputMouseData mouse) {
            RawInputDevice mouseDevice = mouse.Device;
            if (mouseDevice == null) return;
            if (mouse == null || !_playerPath.ContainsKey(mouseDevice.DevicePath)) return;

            MouseData connected = _playerPath[mouseDevice.DevicePath].mouseData;
            connected.UpdateKeys(mouse.Mouse);
        }
        public virtual void UpdateBullets(GameTime gameTime) {
            for (int i = 0; i < _bullets.Count; i++) {
                _bullets[i].Update(gameTime);
            }
        }
        public virtual void UpdatePlayers(GameTime gameTime) {
            for (int i = 0; i < _players.Count; i++) {
                // Run last
                _players[i].Update(gameTime);
            }
        }
        public abstract void Draw(RenderCanvas canvas);
        public abstract void Update(GameTime gameTime);
    }
}

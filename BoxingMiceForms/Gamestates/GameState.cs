using Editor.Controls;
using Linearstar.Windows.RawInput;
using Microsoft.Xna.Framework;
using System;
using static Editor.RawMouseData;

namespace Editor.Gamestates {
    public abstract class GameState {

        protected MainControl _main;
        public GameState(MainControl mc)
        {
            this._main = mc;
        }

        public virtual void OnActive() {}
        public virtual void OnInactive() {}

        public virtual void OnMouseInput(RawInputMouseData mouse) {
            RawInputDevice mouseDevice = mouse.Device;
            if (mouseDevice == null) return;
            if (mouse == null || !_main._playerPath.ContainsKey(mouseDevice.DevicePath)) return;

            MouseData connected = _main._playerPath[mouseDevice.DevicePath].mouseData;
            connected.UpdateKeys(mouse.Mouse);
        }
        public virtual void UpdateBullets(GameTime gameTime) {
            for (int i = 0; i < _main._bullets.Count; i++) {
                _main._bullets[i].Update(gameTime);
            }
        }
        public virtual void UpdatePlayers(GameTime gameTime) {
            for (int i = 0; i < _main._players.Count; i++) {
                // Run last
                _main._players[i].Update(gameTime);
            }
        }
        public abstract void Draw(RenderCanvas canvas);
        public abstract void Update(GameTime gameTime);
    }
}

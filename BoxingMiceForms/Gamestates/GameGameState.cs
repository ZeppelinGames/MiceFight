using System;
using Editor.Controls;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static Editor.RawMouseData;

namespace Editor.Gamestates {
    public class GameGameState : GameState {

        const float _moveSpeed = 0.1f;

        public GameGameState(MainControl mc) : base(mc){}

        public override void Draw(RenderCanvas canvas) {
            for (int i = 0; i < _main._bullets.Count; i++) {
                _main._bullets[i].Draw(canvas);
            }
            for (int i = 0; i < _main._players.Count; i++) {
                Player player = _main._players[i];

                if (!player.isAlive) {
                    canvas.DrawCircle(player.X, player.Y, Player.size, Color.Gray);
                    continue;
                }

                MouseData mouseData = player.mouseData;

                player.x += mouseData.nDX * _moveSpeed;
                player.y += mouseData.nDY * _moveSpeed;

                canvas.DrawCircle(player.X, player.Y, Player.size, player.color);

                float rayCX = player.x + player.mouseData.nDX;
                float rayCY = player.Y + player.mouseData.nDY;
                canvas.DrawLine(player.X, player.Y, (int)rayCX, (int)rayCY, Color.Red);

                if (_main._players[i].mouseData.rightButton) {
                    canvas.DrawRotatedSprite(player.shieldSprite, player.X, player.Y, player.shieldRotation);
                } else {
                    if (_main._players[i].mouseData.leftButton) {
                        _main.SpawnBullet(_main._players[i]);
                    }
                }
            }
        }

        public override void Update(GameTime gameTime) {

            // Check if you shot someone
            List<Bullet> blockedBullets = new List<Bullet>();
            int alivePlayerCount = 0;
            for (int j = 0; j < _main._players.Count; j++) {
                for (int i = 0; i < _main._bullets.Count; i++) {
                    if (_main._bullets[i].player.id == _main._players[j].id) continue;
                    if (Helpers.CirclePointIntersect(
                        _main._bullets[i].x,
                        _main._bullets[i].y,
                        _main._players[j].x,
                        _main._players[j].y,
                        Player.size)) {

                        // Blocking?
                        if (_main._players[j].mouseData.rightButton) {
                            blockedBullets.Add(_main._bullets[i]);
                        } else {
                            Debug.WriteLine($"{_main._bullets[i].player.id} killed {_main._players[j].id}");
                            _main._players[j].Kill();
                        }
                    }
                }

                if (_main._players[j].isAlive) {
                    alivePlayerCount++;
                }
            }

            // Get rid of them blocked ones
            for (int i = 0; i < blockedBullets.Count; i++) {
                _main._bullets.Remove(blockedBullets[i]);
            }

            if (alivePlayerCount <= 1) {
                MainControl.Instance.SetGameState(GAMESTATE.GAMEOVER);
            }

            this.UpdateBullets(gameTime);
            this.UpdatePlayers(gameTime);
        }
    }
}

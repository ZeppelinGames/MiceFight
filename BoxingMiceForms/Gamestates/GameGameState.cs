using System;
using Editor.Controls;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static Editor.RawMouseData;

namespace Editor.Gamestates {
    public class GameGameState : GameState {
        public override void Draw(RenderCanvas canvas) {
            for (int i = 0; i < _bullets.Count; i++) {
                _bullets[i].Draw(canvas);
            }
        }

        public override void Update(GameTime gameTime) {
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

            this.UpdateBullets(gameTime);
            this.UpdatePlayers(gameTime);
        }
    }
}

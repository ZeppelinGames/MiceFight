using System;
using Editor.Controls;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static Editor.RawMouseData;

namespace Editor.Gamestates
{
    public class GameGameState : GameState
    {

        const float _moveSpeed = 0.5f;

        public GameGameState(MainControl mc) : base(mc) { }

        public override void OnActive()
        {
            float step = (float)(Math.PI * 2) / Math.Max(1, _main._players.Count);
            int min = Math.Min(RenderCanvas.TARGET_WIDTH, RenderCanvas.TARGET_HEIGHT);
            int hmin = (int)(min * 0.5f);
            int radius = (int)(min * 0.25f);

            // Set player positions
            for (int i = 0; i < _main._players.Count; i++)
            {
                Player player = _main._players[i];
                float a = step * i;

                float posX = ((float)Math.Cos(a) * radius) + hmin;
                float posY = ((float)Math.Sin(a) * radius) + hmin;
                _main._players[i].SetPosition((int)posX, (int)posY);
            }
        }

        public override void Draw(RenderCanvas canvas)
        {
            for (int i = 0; i < _main._bullets.Count; i++)
            {
                _main._bullets[i].Draw(canvas);
            }
            for (int i = 0; i < _main._players.Count; i++)
            {
                Player player = _main._players[i];

                if (!player.isAlive)
                {
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

                if (_main._players[i].mouseData.rightButton)
                {
                    canvas.DrawRotatedSprite(player.shieldSprite, player.X, player.Y, player.shieldRotation);
                }
                else
                {
                    if (_main._players[i].mouseData.leftButton)
                    {
                        _main.SpawnBullet(_main._players[i]);
                    }
                }
            }
        }

        public override void Update(GameTime gameTime)
        {

            // Check if you shot someone
            List<Bullet> blockedBullets = new List<Bullet>();
            int alivePlayerCount = 0;
            Player lastAlivePlayer = null;
            for (int j = 0; j < _main._players.Count; j++)
            {
                for (int i = 0; i < _main._bullets.Count; i++)
                {
                    if (_main._bullets[i].player.id == _main._players[j].id) continue;
                    if (Helpers.CirclePointIntersect(
                        _main._bullets[i].x,
                        _main._bullets[i].y,
                        _main._players[j].x,
                        _main._players[j].y,
                        Player.size))
                    {

                        // Blocking?
                        if (_main._players[j].mouseData.rightButton)
                        {
                            blockedBullets.Add(_main._bullets[i]);
                        }
                        else
                        {
                            Debug.WriteLine($"{_main._bullets[i].player.id} killed {_main._players[j].id}");
                            _main._players[j].Kill();
                        }
                    }
                }

                if (_main._players[j].isAlive)
                {
                    lastAlivePlayer = _main._players[j];
                    alivePlayerCount++;
                }
            }

            // Get rid of them blocked ones
            for (int i = 0; i < blockedBullets.Count; i++)
            {
                _main._bullets.Remove(blockedBullets[i]);
            }

            if (alivePlayerCount <= 1)
            {
                MainControl.Instance.SetGameState(GAMESTATE.GAMEOVER);
                GameoverGameState.Instance.SetWinner(lastAlivePlayer);
            }

            this.UpdateBullets(gameTime);
            this.UpdatePlayers(gameTime);
        }
    }
}

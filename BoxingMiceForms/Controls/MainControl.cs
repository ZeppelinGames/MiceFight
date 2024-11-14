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
using static Editor.RawMouseData;
using Editor.Gamestates;
using Editor.UI;

namespace Editor.Controls
{
    public enum GAMESTATE
    {
        TITLE,
        IN_GAME,
        GAMEOVER
    };

    public class MainControl : MonoGameControl
    {
        public static MainControl Instance;
        public static Action<Player, RawMouseButtonFlags> onMouseClick;

        readonly static Random rnd = new Random();
        private IKeyboardMouseEvents _mouseHook;
        private RenderCanvas _canvas;

        private static Dictionary<GAMESTATE, GameState> _gameStates;

        private float _moveSpeed = 0.5f;

        public readonly Dictionary<string, Player> _playerPath = new Dictionary<string, Player>();
        public readonly List<Player> _players = new List<Player>();
        int playerId = 0;

        public readonly List<Bullet> _bullets = new List<Bullet>();

        // Game state vars
        GAMESTATE _gameState = GAMESTATE.TITLE;
        GameState _currentState;

        public static readonly Color[] PlayerColors = new Color[] {
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

        public MainControl()
        {
            _mouseHook = Hook.GlobalEvents();
            _mouseHook.MouseDownExt += GlobalHookMousePress;
            Instance = this;
            _gameStates = new Dictionary<GAMESTATE, GameState>() {
                { GAMESTATE.TITLE, new TitleGameState(Instance)},
                { GAMESTATE.IN_GAME, new GameGameState(Instance)},
                { GAMESTATE.GAMEOVER, new GameoverGameState(Instance) }
            };
            SetGameState(GAMESTATE.TITLE);

            UpdateWindow();
        }

        protected override void Initialize()
        {
            _canvas = new RenderCanvas(Editor.GraphicsDevice);

            UpdateWindow();
            base.Initialize();
        }

        public void UpdateWindow()
        {
            if (_canvas == null) return;
            _canvas.Resize();
        }

        private void GlobalHookMousePress(object sender, MouseEventExtArgs e)
        {
            e.Handled = true;
            return;
        }

        public void DeregisterHook()
        {
            _mouseHook.MouseDownExt -= GlobalHookMousePress;
            _mouseHook.Dispose();
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case (int)MouseEvents.WM_INPUT:
                    RawInputData inputData = RawInputData.FromHandle(m.LParam);
                    switch (inputData)
                    {
                        case RawInputMouseData mouse:
                            // Try get player
                            RawInputDevice mouseDevice = mouse.Device;
                            if (mouse != null && _playerPath.ContainsKey(mouseDevice.DevicePath))
                            {
                                // Update mouse
                                Player mousePlayer = _playerPath[mouseDevice.DevicePath];
                                mousePlayer.mouseData.UpdateKeys(mouse.Mouse);
                                MouseData mData = mousePlayer.mouseData;
                                if (mData.leftButton)
                                {
                                    UIManager.OnClick?.Invoke(mousePlayer, mousePlayer.X, mousePlayer.Y);
                                }
                            }

                            if (_currentState != null)
                                _currentState.OnMouseInput(mouse);
                            break;
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        public void SetGameState(GAMESTATE gameState)
        {
            this._gameState = gameState;
            this._currentState = _gameStates[gameState];
        }

        public void SpawnBullet(Player player)
        {
            if (player.mouseData.nLDX == 0 && player.mouseData.nLDY == 0) return;

            Bullet b = new Bullet(player);
            _bullets.Add(b);
        }

        public Color GetPlayerColor()
        {
            return PlayerColors[playerId % (PlayerColors.Length - 1)];
        }

        public bool RegisterMouse(RawInputDevice device, out Player p)
        {
            p = null;
            if (device == null || device.DevicePath == null) return false;
            if (!_playerPath.ContainsKey(device.DevicePath))
            {
                Debug.WriteLine($"Added mouse: {device.DevicePath} {device.ProductName}");

                Player newPlayer = new Player(playerId);
                newPlayer.playerColorIndex = rnd.Next(0, PlayerColors.Length);
                newPlayer.color = PlayerColors[newPlayer.playerColorIndex];
                playerId++;

                _players.Add(newPlayer);
                _playerPath.Add(device.DevicePath, newPlayer);
                p = newPlayer;
                return true;
            }
            return false;
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                // close game
                MainForm.instance.CloseGame();
            }

            // update game state here
            if (_currentState != null)
                _currentState.Update(gameTime);

            // Lock mouse center screen
            System.Drawing.Point formCenter = new System.Drawing.Point(
                MainForm.instance.Location.X + (int)(MainForm.instance.Width * 0.5f),
                MainForm.instance.Location.Y + (int)(MainForm.instance.Height * 0.5f)
            );

            Cursor.Position = formCenter;
            base.Update(gameTime);
        }

        protected override void Draw()
        {
            // Canvas draw
            // Draw game state here
            if (_currentState != null)
                _currentState.Draw(_canvas);
            _canvas.Draw(Editor.spriteBatch);
            base.Draw();
        }

        public void ResetGame()
        {
            _players.Clear();
            _bullets.Clear();
            _playerPath.Clear();
            playerId = 0;
            SetGameState(GAMESTATE.TITLE);
        }
    }
}

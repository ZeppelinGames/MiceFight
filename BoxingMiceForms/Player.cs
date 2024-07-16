using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Editor {
    public class Player { 
        static Random rnd = new Random();

        public RawMouseData.MouseData mouseData => _mouseData;
        private RawMouseData.MouseData _mouseData;

        public bool alive => _alive;
        private readonly bool _alive;

        public bool isReady;

        public readonly int id;

        public float x, y;
        public int X => (int)x;
        public int Y => (int)y;

        public Color color = Color.White;
        public int playerColorIndex = -1;

        public Player(int id, int x = 0, int y = 0) {
            this.id = id;
            this.x = x;
            this.y = y;
            _mouseData = new RawMouseData.MouseData(id);
        }
    }
}

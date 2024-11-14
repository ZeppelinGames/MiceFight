using Microsoft.Xna.Framework;

namespace Editor {
    public class Bullet : Actor {
        public float x;
        public int X => (int)x;
        public float y;
        public int Y => (int)y;
        public float dx;
        public int dX => (int)dx;
        public float dy;
        public int dY => (int)dy;

        public Player player;

        public Bullet(Player p) {
            this.player = p;
            this.x = p.x;
            this.y = p.y;
            this.dx = p.mouseData.nLDX;
            this.dy = p.mouseData.nLDY;
        }

        public override void Update(GameTime gameTime) {
            this.x += this.dx;
            this.y += this.dy;
        }

        public override void Draw(RenderCanvas canvas) {
            canvas.DrawLine(X, Y, X + (int)dx, Y + (int)dy, player.color);
        }
    }
}

using Microsoft.Xna.Framework;

namespace Editor {
    public abstract class Actor {
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(RenderCanvas canvas);
    }
}

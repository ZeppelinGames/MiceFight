using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor {
    public abstract class Actor {
        public abstract void Update(GameTime gameTime);
        public abstract void Draw();
    }
}

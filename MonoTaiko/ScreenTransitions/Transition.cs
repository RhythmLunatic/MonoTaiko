using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoTaiko.ScreenTransitions
{
    public abstract class Transition
    {
        public GraphicsDevice graphicsDevice;
        public SpriteBatch spriteBatch;

        public Transition(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            this.graphicsDevice = graphicsDevice;
            this.spriteBatch = spriteBatch;
        }

        public bool active { get; set; }

        public abstract bool Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime);
    }
}

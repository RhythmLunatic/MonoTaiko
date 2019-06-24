using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoTaiko.ScreenTransitions
{
    class FadeOut : Transition
    {
        float fadeLength = 1f;
        float alpha = 0;

        Texture2D rect;

        public FadeOut(GraphicsDevice gd, SpriteBatch sb) : base(gd, sb)
        {
            rect = new Texture2D(graphicsDevice, 1, 1);

            Color[] data = new Color[1];
            data[0] = Color.Black;

            rect.SetData(data);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(rect, new Rectangle(0, 0, 1280, 720), Color.White * alpha);
            spriteBatch.End();
        }

        public override bool Update(GameTime gameTime)
        {
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            alpha += timeDelta / fadeLength;

            if (alpha < 1) return true; 

            return false;
        }
    }
}

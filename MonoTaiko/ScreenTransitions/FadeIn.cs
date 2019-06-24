using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoTaiko.ScreenTransitions
{
    class FadeIn : Transition
    {
        const float FADE_LENGTH = 1f;
        float alpha = 1;

        Texture2D rect;

        public FadeIn(GraphicsDevice gd, SpriteBatch sb) : base(gd, sb)
        {
            active = true;

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

            alpha -= timeDelta / FADE_LENGTH;

            if (alpha > 0)
            {
                active = true;
                return true;
            }

            active = false;
            return false;
        }
    }
}
